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

        private HttpClient client;

        private HttpClientHandler clientHandler;

        public string HostName => baseURL;

        private readonly SemaphoreSlim tokenRefreshLock = new SemaphoreSlim(1, 1);

        public static TicketInformations TicketInformations { get; private set; } = new TicketInformations();

        public string AccessToken { get; private set; }

        public string Id => config?.Id;

        private readonly SparkConfiguration config;

        public Spark(string accessToken)
            : this(new SparkConfiguration { Token = accessToken })
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
        }

        private void SetupHttpClient()
        {
            clientHandler = new HttpClientHandler
            {
                //ServerCertificateCustomValidationCallback = valdiateCert
            };
            if (config.NoProxy)
            {
                Log($"Proxy for {HostName} has been disabled by configuration", 5);
                clientHandler.UseProxy = false;
            }
            else if (clientHandler.UseProxy)
            {
                if (clientHandler.Proxy != null)
                {
                    var proxy = clientHandler.Proxy.GetProxy(new Uri($"https://{HostName}"));
                    Log($"Using proxy {proxy?.AbsolutePath} for {HostName}", 5);
                }
                else
                    Log($"UseProxy is enabled on clientHandler for {HostName}, using system proxy", 5);
            }
            else
                Log($"Not using a proxy for API requests", 5);
            client = new HttpClient(clientHandler);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (!string.IsNullOrEmpty(config.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.Token);
        }

        public async Task<SparkApiConnectorApiOperationResult> Login(string login = null, string password = null, string applicationId = null, string secretKey = null, int timeout = 0)
        {
            var result = new SparkApiConnectorApiOperationResult();
            if (!string.IsNullOrEmpty(config.Token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.Token);
            AccessToken = config.Token;

            //HttpResponseMessage response = null;
            //try
            //{
            //    var auth = Base64Encode($"{login ?? config.Login}:{password ?? config.Password}");
            //    var authString = $"{secretKey ?? config.SecretKey}{password ?? config.Password}";
            //    var appAuth = Base64Encode($"{applicationId ?? config.ApplicationId}:{ComputeSha256Hash(authString)}");

            //    var uri = $"{RestUrl}/authentication/v1.0/login";
            //    var msg = new HttpRequestMessage(HttpMethod.Get, uri);
            //    msg.Headers.Add("x-rainbow-app-auth", $"Basic {appAuth}");
            //    msg.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", auth);
            //    msg.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            //    if (timeout > 0)
            //    {
            //        CancellationTokenSource src = new CancellationTokenSource(timeout * 1000);
            //        response = await httpClient.SendAsync(msg, src.Token).ConfigureAwait(false);
            //    }
            //    else
            //        response = await httpClient.SendAsync(msg).ConfigureAwait(false);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            //        result.Result = JsonConvert.DeserializeObject<LoginData>(content, serializerSettings);
            //        SetBearerToken(result.Result.Token);
            //        Log($"successfully logged into Rainbow server using login {login ?? config.Login}", 4);
            //        result.ResultCode = RainbowRestApiResultCode.OK;
            //    }
            //    else
            //    {
            //        result.ResultCode = await ProcessNon200HttpResponse(result, response).ConfigureAwait(false);
            //    }
            //}
            //catch (Exception e)
            //{
            //    ProcessException(result, e, response);
            //}
            result.ResultCode = SparkApiOperationResultCode.OK;
            return result;
        }

        public async Task<SparkApiConnectorApiOperationResult> Reinitialize(SparkConfiguration newConfig = null)
        {
            if (AccessToken != null)
            {
                var credentialsChanged = config == null
                    || (!string.IsNullOrEmpty(newConfig?.Token) && config.Token != newConfig.Token);
                //var credentialsChanged = config == null
                //    || (!string.IsNullOrEmpty(newConfig?.Login) && config.Login != newConfig.Login)
                //    || (!string.IsNullOrEmpty(newConfig?.Password) && config.Password != newConfig.Password)
                //    || (!string.IsNullOrEmpty(newConfig?.ApplicationId) && config.ApplicationId != newConfig.ApplicationId)
                //    || (!string.IsNullOrEmpty(newConfig?.SecretKey) && config.SecretKey != newConfig.SecretKey);
                UpdateConfiguration(newConfig);
                if (credentialsChanged) // need to relogin anyway
                {
                    var loginRes = await Login(newConfig?.Login, newConfig?.Password, newConfig?.ApplicationId, newConfig?.SecretKey).ConfigureAwait(false);
                    if (loginRes.IsSuccess)
                        UpdateCredentials(newConfig);
                    return loginRes;
                }
                else if (IsTokenRefreshRequired())
                {
                    return await RefreshTokenOrReLogin().ConfigureAwait(false);
                }
                return SparkApiConnectorApiOperationResult.Success;
            }
            else
            {
                var loginRes = await Login(newConfig?.Login, newConfig?.Password, newConfig?.ApplicationId, newConfig?.SecretKey).ConfigureAwait(false);
                if (loginRes.IsSuccess)
                    UpdateCredentials(newConfig);
                return loginRes;
            }
        }

        private void UpdateConfiguration(SparkConfiguration newConfig)
        {
            if (newConfig != null)
            {
                config.LogAction = newConfig.LogAction;
                config.NoProxy = newConfig.NoProxy;
            }
        }

        private void UpdateCredentials(SparkConfiguration newConfig)
        {
            if (newConfig != null)
            {
                config.Login = newConfig.Login;
                config.Password = newConfig.Password;
                config.ApplicationId = newConfig.ApplicationId;
                config.SecretKey = newConfig.SecretKey;
                if (config.Token != newConfig.Token && !string.IsNullOrEmpty(newConfig.Token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", newConfig.Token);
                    config.Token = newConfig.Token;
                }
            }
        }

        private bool IsTokenRefreshRequired()
        {
            //if (parsedToken != null)
            //{
            //    var doRefresh = parsedToken.ValidTo < DateTime.UtcNow;
            //    if (!doRefresh) // token is still valid
            //    {
            //        var timeSinceIssue = DateTime.UtcNow.Subtract(parsedToken.IssuedAt).TotalSeconds;
            //        var tokenLifeTime = parsedToken.ValidTo.Subtract(parsedToken.IssuedAt).TotalSeconds;
            //        if (timeSinceIssue * 2 > tokenLifeTime)
            //            doRefresh = true;
            //    }
            //    return doRefresh;
            //}
            return false;
        }

        private async Task<SparkApiConnectorApiOperationResult> RefreshTokenOrReLogin([CallerMemberName] string methodName = null)
        {
            var result = new SparkApiConnectorApiOperationResult();
            var lockAcquired = false;
            try
            {
                await tokenRefreshLock.WaitAsync().ConfigureAwait(false);
                lockAcquired = true;
                if (!IsTokenRefreshRequired()) // check again.. so in case if a request was queued and the refresh was done, we don't run it again
                {
                    Log($"Skipping token refresh triggered from {methodName} because token refresh is no longer required", 4);
                    result.ResultCode = SparkApiOperationResultCode.OK;
                    return result;
                }
                //if (rainbowToken.CountRenewed < rainbowToken.MaxTokenRenew)
                //{
                //    return await RenewToken().ConfigureAwait(false);
                //}
                //else
                //{
                //    Log($"Maximum number of token refreshes has been exhausted, logging in again", 4);
                //    return await Login().ConfigureAwait(false);
                //}
                return await Login().ConfigureAwait(false);
            }
            finally
            {
                if (lockAcquired)
                    tokenRefreshLock.Release();
            }
        }

        public async Task<SparkApiConnectorApiOperationResult> Logout(int timeout = 0)
        {
            return await Task.Run(() => SparkApiConnectorApiOperationResult.Success);
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
            var result = new SparkApiConnectorApiOperationResult<bool>();

            HttpResponseMessage response = null;
            try
            {
                var fullpath = $"{baseURL}{path}";
                response = await client.DeleteAsync(fullpath);
                await TicketInformations.FillRequestParameter(response);

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
            var result = new SparkApiConnectorApiOperationResult<T>();
            HttpResponseMessage response = null;
            try
            {
                response = await client.GetAsync(path).ConfigureAwait(false);
                await TicketInformations.FillRequestParameter(response);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
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
                await ProcessException(result, ex, response);
            }

            return result;
        }

        private async Task<SparkApiConnectorApiOperationResult<List<T>>> GetItemsAsync<T>(string path, string rootNode = "items")
        {
            List<T> items = null;
            JObject requestResult = null;
            List<JToken> results = null;

            var result = new SparkApiConnectorApiOperationResult<List<T>>();
            HttpResponseMessage response = null;
            try
            {
                response = await client.GetAsync(path).ConfigureAwait(true);
                await TicketInformations.FillRequestParameter(response);
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
                await ProcessException(result, ex, response);
            }

            return result;
        }

        private string GetURL(string path, Dictionary<string, string> dict, string basePath = baseURL)
        {
            UriBuilder uriBuilder = new UriBuilder(basePath);
            uriBuilder.Path = path;
            var queryString = new StringBuilder();
            if (dict.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in dict)
                {
                    queryString.Append(UrlEncode(kv.Key)).Append('=').Append(UrlEncode(kv.Value)).Append('&');
                }
            }
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

        private async Task<SparkApiConnectorApiOperationResult<T>> PostItemAsync<T>(string path, Dictionary<string, object> bodyParams)
        {
            var result = new SparkApiConnectorApiOperationResult<T>();
            HttpResponseMessage response = null;
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

                response = await client.PostAsync(path, content);
                await TicketInformations.FillRequestParameter(response);

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
            var result = new SparkApiConnectorApiOperationResult<T>();
            HttpResponseMessage response = null;
            StringContent content;
            try
            {
                var jsonBody = JsonConvert.SerializeObject(bodyParams);
                content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
                response = await client.PutAsync(path, content);
                await TicketInformations.FillRequestParameter(response);

                if (response.IsSuccessStatusCode)
                {
                    result.Result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    result.ResultCode = MapHttpStatusCode(response.StatusCode);
                }
                else
                {
                    result.ResultCode = await ProcessNon200HttpResponse(result, response).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await ProcessException(result, ex, response);
            }

            return result;
        }

        private async Task<SparkApiConnectorApiOperationResult<T>> UpdateItemAsync<T>(string path, T body)
        {
            var result = new SparkApiConnectorApiOperationResult<T>();

            var jsonBody = JsonConvert.SerializeObject(body);
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(path, content);
            await TicketInformations.FillRequestParameter(response);

            await CheckForErrorResponse(response);
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

        private async Task<SparkApiConnectorApiOperationResult<T>> UpdateItemAsync<T, U>(string path, U body)
        {
            var result = new SparkApiConnectorApiOperationResult<T>();

            var jsonBody = JsonConvert.SerializeObject(body);
            StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(path, content);
            await TicketInformations.FillRequestParameter(response);

            await CheckForErrorResponse(response);
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
            await UpdateItemAsync<Dictionary<string, object>>(path, bodyParams);
        }

        #endregion Private Helper Methods

        public async Task<PaginationResult<T>> GetItemsWithLinksAsync<T>(string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);
            await TicketInformations.FillRequestParameter(response);

            await CheckForErrorResponse(response);
            //todo:handle error
            //var errorResponse = await CheckForErrorResponse(response);
            //if (errorResponse is null)
            //{
            //    return null;
            //}

            PaginationResult<T> paginationResult = new PaginationResult<T>();
            JObject requestResult = JObject.Parse(await response.Content.ReadAsStringAsync());
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

            SparkErrorContent sparkErrorContent = JsonConvert.DeserializeObject<SparkErrorContent>(await response.Content.ReadAsStringAsync());
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