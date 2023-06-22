using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using static SparkDotNet.ExceptionHandling.SparkApiOperationResultMapper;
using static System.Net.WebUtility;

namespace SparkDotNet
{
    public partial class Spark
    {
        private const string baseURL = "https://webexapis.com";

        private HttpClient Client { get; set; }

        private HttpClientHandler ClientHandler { get; set; }

        public string HostName => baseURL;

        private readonly SemaphoreSlim tokenRefreshLock = new SemaphoreSlim(1, 1);

        public static TicketInformations TicketInformations { get; set; } = new TicketInformations();

        public string Id => config?.Id;

        private readonly SparkConfiguration config;

        public Spark(string clientId, string secretKey, string accessToken, string refreshToken)
            : this(new SparkConfiguration { ApplicationId = clientId, SecretKey = secretKey, AccessToken = accessToken, RefreshToken = refreshToken })
        {
        }

        public Spark(SparkConfiguration config)
        {
            this.config = config;
            SetupHttpClient();
            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Populate
            };
            serializerSettings.Converters.Add(new StringEnumConverter(true));
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // use camel case so we can use proper .net notation
            JsonConvert.DefaultSettings = () => serializerSettings;
            refreshTokenPayload.Initialize(config);
        }

        private void SetupHttpClient()
        {
            ClientHandler = new HttpClientHandler
            {
                //ServerCertificateCustomValidationCallback = valdiateCert
            };
            if (config.NoProxy)
            {
                Log($"Proxy for {HostName} has been disabled by configuration", 5);
                ClientHandler.UseProxy = false;
            }
            else if (ClientHandler.UseProxy)
            {
                if (ClientHandler.Proxy != null)
                {
                    var proxy = ClientHandler.Proxy.GetProxy(new Uri($"{HostName}"));
                    Log($"Using proxy {proxy?.AbsolutePath} for {HostName}", 5);
                }
                else
                    Log($"UseProxy is enabled on clientHandler for {HostName}, using system proxy", 5);
            }
            else
                Log($"Not using a proxy for API requests", 5);
            Client = new HttpClient(ClientHandler);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(config.AccessToken))
                SetBearerToken();
        }

        public async Task<SparkApiConnectorApiOperationResult> RefreshToken(string applicationId = null, string secretKey = null)
        {
            return await RefreshTokenAsync().ConfigureAwait(false);
            /////by now
            //var result = new SparkApiConnectorApiOperationResult();
            //bool tokenPresent = !string.IsNullOrEmpty(config.Token);
            //if (tokenPresent)
            //    Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.Token);
            //AccessToken = config.Token;
            /////by now
        }

        private void UpdateConfiguration(SparkConfiguration newConfig)
        {
            if (newConfig != null)
            {
                config.LogAction = newConfig.LogAction;
                config.NoProxy = newConfig.NoProxy;
            }
        }

        #region Private Helper Methods

        private static bool IsLocalPath(object files)
        {
            List<string> filelist = (List<string>)files;
            var p = filelist[0];
            try
            {
                return new System.Uri(p).IsFile;
            }
            catch (Exception)
            {
                return true; // assume it's a local file if we can't create a URI out of it...
            }
        }

        private async Task<SparkApiConnectorApiOperationResult<bool>> DeleteItemAsync(string path)
        {
            await RefreshTokenIfRequired().ConfigureAwait(false);
            var result = new SparkApiConnectorApiOperationResult<bool>();
            HttpResponseMessage response = null;
            try
            {
                var fullpath = $"{baseURL}{path}";
                response = await Client.DeleteAsync(fullpath).ConfigureAwait(false);
                await TicketInformations.FillRequestParameter(response).ConfigureAwait(false);

                result.Result = MapHttpStatusCode(HttpStatusCode.NoContent) == SparkApiOperationResultCode.OK;
                result.ResultCode = MapHttpStatusCode(response.StatusCode);
            }
            catch (HttpRequestException ex)
            {
                result.Error = new SparkErrorContent();
                result.Error.Errors = new List<SparkErrorMessage>(1) { new SparkErrorMessage() { Description = ex.StackTrace } };
                result.Error.Message = ex.Message;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        private async Task<SparkApiConnectorApiOperationResult<T>> GetItemAsync<T>(string path)
        {
            await RefreshTokenIfRequired().ConfigureAwait(false);
            var result = new SparkApiConnectorApiOperationResult<T>();
            HttpResponseMessage response = null;
            try
            {
                response = await Client.GetAsync(path).ConfigureAwait(false);
                await TicketInformations.FillRequestParameter(response).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    result.Result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    result.ResultCode = MapHttpStatusCode(response.StatusCode);

                    var pagination = LinkHeader.CreateInstance(response);
                    if (pagination != null)
                    {
                        result.NextLink = pagination.NextLink;
                    }
                }
                else
                {
                    result.ResultCode = await ProcessNon200HttpResponse(result, response).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await ProcessException(result, ex, response).ConfigureAwait(false);
            }

            return result;
        }

        private async Task<SparkApiConnectorApiOperationResult<List<T>>> GetItemsAsync<T>(string path, string rootNode = "items")
        {
            await RefreshTokenIfRequired().ConfigureAwait(false);

            List<T> items;
            JObject requestResult;
            List<JToken> results;

            var result = new SparkApiConnectorApiOperationResult<List<T>>();
            HttpResponseMessage response = null;
            try
            {
                response = await Client.GetAsync(path).ConfigureAwait(true);
                await TicketInformations.FillRequestParameter(response).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        items = new List<T>();
                        requestResult = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                        results = requestResult[rootNode].Children().ToList();
                        foreach (JToken token in results)
                        {
                            T item = JsonConvert.DeserializeObject<T>(token.ToString());
                            items.Add(item);
                        }
                        result.Result = items;
                    }
                    result.ResultCode = MapHttpStatusCode(response.StatusCode);

                    var pagination = LinkHeader.CreateInstance(response);
                    if (pagination != null)
                    {
                        var pagRes = await GetItemsAsync<T>(pagination.NextLink, rootNode).ConfigureAwait(false);
                        if (pagRes.IsSuccess)
                        {
                            result.Result.AddRange(pagRes.Result);
                        }
                        else
                        {
                            result = pagRes;
                        }
                    }
                }
                else
                {
                    result.ResultCode = await ProcessNon200HttpResponse(result, response).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await ProcessException(result, ex, response).ConfigureAwait(false);
            }

            return result;
        }

        private string GetURL(string path, Dictionary<string, string> dict = null, string basePath = baseURL)
        {
            UriBuilder uriBuilder = new UriBuilder(basePath)
            {
                Path = path
            };
            var queryString = new StringBuilder();
            dict?.ToList().ForEach(
                kv => queryString.Append(UrlEncode(kv.Key)).Append('=').Append(UrlEncode(kv.Value)).Append('&')
            );
            uriBuilder.Query = queryString.ToString().Trim('&');
            return uriBuilder.Uri.ToString();
        }

        private void Log(string message, int severity)
        {
            if (config.LogAction != null)
                config.LogAction(message, severity);
            else if (severity < 5)
                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} {message}");
        }

        public void SetLogAction(Action<string, int> logAction)
        {
            config.LogAction = logAction;
        }

        private async Task<SparkApiConnectorApiOperationResult> PostItemAsync(string path, MultipartFormDataContent mpfdc,
    bool skipTokenRefresh = false)
        {
            if (!skipTokenRefresh)
                await RefreshTokenIfRequired().ConfigureAwait(false);

            var result = new SparkApiConnectorApiOperationResult();
            HttpContent content = mpfdc;
            try
            {
                var response = await Client.PostAsync(path, content).ConfigureAwait(false);
                await TicketInformations.FillRequestParameter(response).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    // result.Result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    result.ResultCode = MapHttpStatusCode(response.StatusCode);
                }
                else
                {
                    result.ResultCode = await ProcessNon200HttpResponse(result, response).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException ex)
            {
                result.Error = new SparkErrorContent();
                result.Error.Errors = new List<SparkErrorMessage>(1) { new SparkErrorMessage() { Description = ex.StackTrace } };
                result.Error.Message = ex.Message;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        private async Task<SparkApiConnectorApiOperationResult<T>> PostItemAsync<T>(string path, Dictionary<string, object> bodyParams,
            bool skipTokenRefresh = false)
        {
            if (!skipTokenRefresh)
                await RefreshTokenIfRequired().ConfigureAwait(false);

            var result = new SparkApiConnectorApiOperationResult<T>();
            HttpContent content;
            try
            {
                if (bodyParams.ContainsKey("files") && IsLocalPath(bodyParams["files"]))
                {
                    MultipartFormDataContent multiPartContent = new MultipartFormDataContent("----SparkDotNetBoundary");
                    List<string> filelist = (List<string>)bodyParams["files"];
                    bodyParams.Remove("files");
                    foreach (KeyValuePair<string, object> kv in bodyParams)
                    {
                        StringContent stringContent = new StringContent((string)kv.Value);
                        multiPartContent.Add(stringContent, kv.Key);
                    }
                    foreach (string file in filelist)
                    {
                        FileInfo fi = new FileInfo(file);
                        string fileName = fi.Name;
                        byte[] fileContents = File.ReadAllBytes(fi.FullName);
                        ByteArrayContent byteArrayContent = new ByteArrayContent(fileContents);
                        byteArrayContent.Headers.Add("Content-Type", MimeTypes.GetMimeType(fileName));
                        multiPartContent.Add(byteArrayContent, "files", fileName);
                    }
                    content = multiPartContent;
                }
                else
                {
                    string jsonBody = "";
                    if (bodyParams.ContainsKey("attachments"))
                    {
                        var attachments = JObject.Parse((string)bodyParams["attachments"]);
                        bodyParams.Remove("attachments");
                        var jsonParams = JsonConvert.SerializeObject(bodyParams);
                        var json = JObject.Parse(jsonParams);
                        json["attachments"] = attachments;
                        jsonBody = JsonConvert.SerializeObject(json);
                    }
                    else
                    {
                        jsonBody = JsonConvert.SerializeObject(bodyParams);
                    }
                    content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                }

                var response = await Client.PostAsync(path, content).ConfigureAwait(false);
                await TicketInformations.FillRequestParameter(response).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    result.Result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    result.ResultCode = MapHttpStatusCode(response.StatusCode);
                    result.StatusCode = response.StatusCode;
                }
                else
                {
                    result.ResultCode = await ProcessNon200HttpResponse(result, response).ConfigureAwait(false);
                }
            }
            catch (HttpRequestException ex)
            {
                result.Error = new SparkErrorContent();
                result.Error.Errors = new List<SparkErrorMessage>(1) { new SparkErrorMessage() { Description = ex.StackTrace } };
                result.Error.Message = ex.Message;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        private async Task ProcessException(SparkApiConnectorApiOperationResult result, Exception e, HttpResponseMessage response, [CallerMemberName] string methodName = null)
        {
            HttpStatusCode code = HttpStatusCode.Unused;
            string responseData = null;
            if (response != null)
            {
                code = response.StatusCode;
                try
                {
                    responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                catch (ObjectDisposedException) { } // no data can be read
            }
            if (!string.IsNullOrEmpty(responseData))
            {
                try
                {
                    var err = JsonConvert.DeserializeObject<SparkErrorMessage>(responseData);
                    result.ErrorMessage = err.Description;
                    result.ResultCode = MapHttpStatusCode(response.StatusCode);
                }
                catch (Exception)
                {
                    string requestUri = response?.RequestMessage?.RequestUri?.AbsolutePath;
                    result.ErrorMessage = $"Unable to parse response data from {requestUri}";
                    result.ErrorDetail = responseData;
                }
            }
            if (e is HttpRequestException h)
            {
                result.ErrorMessage = $"HttpRequestException {h.Message}";
                //Log($"Api call in {GetType().Name}.{methodName} failed: {h.Message}", 2);
                if (h.InnerException != null)
                {
                    //Log($"Inner exception: {h.InnerException.Message}", 2);
                    result.ErrorDetail = h.InnerException.Message;
                }
                result.ResultCode = SparkApiOperationResultCode.OtherError;
            }
            else
            {
                //Log($"Exception in {GetType().Name}.{methodName}: {e.Message}", 2);
                result.ErrorMessage = $"Generic exception: {e.Message}";
                if (e.InnerException != null)
                {
                    //Log($"Inner exception: {e.InnerException.Message}", 2);
                    result.ErrorDetail = e.InnerException.Message;
                }
                result.ResultCode = SparkApiOperationResultCode.OtherError;
            }
        }

        private async Task<SparkApiConnectorApiOperationResult<T>> UpdateItemAsync<T>(string path, Dictionary<string, object> bodyParams)
        {
            await RefreshTokenIfRequired().ConfigureAwait(false);

            var result = new SparkApiConnectorApiOperationResult<T>();
            HttpResponseMessage response = null;
            StringContent content;
            try
            {
                var jsonBody = JsonConvert.SerializeObject(bodyParams);
                content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                response = await Client.PutAsync(path, content).ConfigureAwait(false);
                await TicketInformations.FillRequestParameter(response, jsonBody).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    result.Result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    result.ResultCode = MapHttpStatusCode(response.StatusCode);
                }
                else
                {
                    result.ResultCode = await ProcessNon200HttpResponse(result, response).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await ProcessException(result, ex, response).ConfigureAwait(false);
            }

            return result;
        }

        private async Task<SparkApiConnectorApiOperationResult<T>> PatchItemAsync<T>(string path, Dictionary<string, object> bodyParams, string contentType = ContentJsonTypes.ApplicationJson)
        {
            await RefreshTokenIfRequired().ConfigureAwait(false);

            var result = new SparkApiConnectorApiOperationResult<T>();
            HttpResponseMessage response = null;
            StringContent content;
            try
            {
                var jsonBody = JsonConvert.SerializeObject(bodyParams);
                content = new StringContent(jsonBody, Encoding.UTF8, contentType);

                var method = "PATCH";
                var httpVerb = new HttpMethod(method);
                var httpRequestMessage = new HttpRequestMessage(httpVerb, path)
                {
                    Content = content
                };

                response = await Client.SendAsync(httpRequestMessage).ConfigureAwait(false);
                await TicketInformations.FillRequestParameter(response).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    result.Result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                    result.ResultCode = MapHttpStatusCode(response.StatusCode);
                    var pagination = LinkHeader.CreateInstance(response);
                    if (pagination != null)
                    {
                        result.NextLink = pagination.NextLink;
                    }
                }
                else
                {
                    result.ResultCode = await ProcessNon200HttpResponse(result, response).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await ProcessException(result, ex, response).ConfigureAwait(false);
            }

            return result;
        }

        private async Task<SparkApiConnectorApiOperationResult<T>> UpdateItemAsync<T>(string path, T body)
        {
            await RefreshTokenIfRequired().ConfigureAwait(false);

            var result = new SparkApiConnectorApiOperationResult<T>();

            var jsonBody = JsonConvert.SerializeObject(body);
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PutAsync(path, content).ConfigureAwait(false);
            await TicketInformations.FillRequestParameter(response).ConfigureAwait(false);

            await CheckForErrorResponse(response).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                result.Result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                result.ResultCode = MapHttpStatusCode(response.StatusCode);
            }
            else
            {
                result.ResultCode = await ProcessNon200HttpResponse(result, response).ConfigureAwait(false);
            }
            return result;
        }

        private async Task<SparkApiConnectorApiOperationResult<T>> UpdateItemAsync<T, U>(string path, U body, bool ignoreNull = false)
        {
            await RefreshTokenIfRequired().ConfigureAwait(false);

            var result = new SparkApiConnectorApiOperationResult<T>();

            string jsonBody = string.Empty;
            if (ignoreNull)
                jsonBody = JsonConvert.SerializeObject(body, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            else
                jsonBody = JsonConvert.SerializeObject(body);

            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await Client.PutAsync(path, content).ConfigureAwait(false);
            await TicketInformations.FillRequestParameter(response).ConfigureAwait(false);

            await CheckForErrorResponse(response).ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                result.Result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                result.ResultCode = MapHttpStatusCode(response.StatusCode);
            }
            else
            {
                result.ResultCode = await ProcessNon200HttpResponse(result, response).ConfigureAwait(false);
            }
            return result;
        }

        private async Task UpdateItemAsync(string path, Dictionary<string, object> bodyParams)
        {
            await UpdateItemAsync<Dictionary<string, object>>(path, bodyParams).ConfigureAwait(false);
        }

        #endregion Private Helper Methods

        public async Task<PaginationResult<T>> GetItemsWithLinksAsync<T>(string path)
        {
            await RefreshTokenIfRequired().ConfigureAwait(false);

            HttpResponseMessage response = await Client.GetAsync(path).ConfigureAwait(false);
            await TicketInformations.FillRequestParameter(response).ConfigureAwait(false);

            await CheckForErrorResponse(response).ConfigureAwait(false);
            //todo:handle error
            //var errorResponse = await CheckForErrorResponse(response).ConfigureAwait(false);
            //if (errorResponse is null)
            //{
            //    return null;
            //}

            PaginationResult<T> paginationResult = new PaginationResult<T>();
            JObject requestResult = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            List<JToken> results = requestResult["items"].Children().ToList();
            foreach (JToken result in results)
            {
                T item = JsonConvert.DeserializeObject<T>(result.ToString());
                paginationResult.Items.Add(item);
            }
            if (response.Headers.Contains("Link"))
            {
                var link = response.Headers.GetValues("Link").FirstOrDefault();
                if (link != null && !"".Equals(link))
                {
                    // borrowed regex from spark-java-sdk
                    // https://github.com/ciscospark/spark-java-sdk/blob/master/src/main/java/com/ciscospark/LinkedResponse.java
                    var regex = new Regex("\\s*<(\\S+)>\\s*;\\s*rel=\"(\\S+)\",?", RegexOptions.Compiled);
                    var regmatch = regex.Matches(link);
                    foreach (Match matchingItem in regmatch)
                    {
                        var linkUrl = new Uri(matchingItem.Groups[1].ToString());
                        var linktype = matchingItem.Groups[2].ToString().ToLower();
                        switch (linktype)
                        {
                            case "next":
                                paginationResult.Links.Next = linkUrl.PathAndQuery;
                                break;

                            case "prev":
                                paginationResult.Links.Prev = linkUrl.PathAndQuery;
                                break;

                            case "first":
                                paginationResult.Links.First = linkUrl.PathAndQuery;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }

            return paginationResult;
        }

        private async Task<SparkErrorContent> CheckForErrorResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return null;
            }

            SparkErrorContent sparkErrorContent = JsonConvert.DeserializeObject<SparkErrorContent>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
            if (sparkErrorContent.Message == null)
                sparkErrorContent.Message = response.ReasonPhrase;
            return sparkErrorContent;
        }

        private async Task<SparkApiOperationResultCode> ProcessNon200HttpResponse(SparkApiConnectorApiOperationResult result, HttpResponseMessage response)
        {
            string responseData = null;
            if (response != null)
            {
                try
                {
                    responseData = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                catch (ObjectDisposedException) { } // no data that can be read
            }
            if (!string.IsNullOrEmpty(responseData))
            {
                string requestUri = response?.RequestMessage?.RequestUri?.AbsoluteUri;
                try
                {
                    result.Error = JsonConvert.DeserializeObject<SparkErrorContent>(responseData);
                    result.ErrorMessage = result.Error?.Message;
                    result.ErrorDetail = $"TrackingId : {result.Error?.TrackingId}";

                    if (string.IsNullOrEmpty(result.Error.Message))
                    {
                        result.ErrorMessage = $"Parsing response data from {requestUri} as error object returned an empty message";
                        result.ErrorDetail = responseData;
                    }
                }
                catch (Exception)
                {
                    result.ErrorMessage = $"Unable to parse response data from {requestUri}";
                    result.ErrorDetail = responseData;
                }
            }
            return MapHttpStatusCode(response.StatusCode);
        }
    }
}