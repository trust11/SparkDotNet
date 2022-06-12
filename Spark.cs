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
using System.Threading.Tasks;
using static Newtonsoft.Json.JsonConvert;
using static SparkDotNet.ExceptionHandling.SparkApiOperationResultMapper;
using static System.Net.WebUtility;

namespace SparkDotNet
{
    public partial class Spark
    {
        private string accessToken { get; set; }

        private const string baseURL = "https://webexapis.com";

        private HttpClient client = new HttpClient();

        public Spark(string accessToken)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Populate
            };
            serializerSettings.Converters.Add(new StringEnumConverter(true));
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver(); // use camel case so we can use proper .net notation
            JsonConvert.DefaultSettings = () => serializerSettings;

            this.accessToken = accessToken;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", this.accessToken);
        }

        #region Private Helper Methods

        private async Task<SparkApiConnectorApiOperationResult<bool>> DeleteItemAsync(string path)
        {
            var result = new SparkApiConnectorApiOperationResult<bool>();

            HttpResponseMessage response = null;
            try
            {
                var fullpath = $"{baseURL}{path}";
                response = await client.DeleteAsync(fullpath);
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
                if (response.IsSuccessStatusCode)
                {
                    result.Result = DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
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

        private string GetURL(string path, Dictionary<string, string> dict, string basePath = baseURL)
        {
            UriBuilder uriBuilder = new UriBuilder(basePath);
            uriBuilder.Path = path;
            string queryString = "";
            if (dict.Count > 0)
            {
                foreach (KeyValuePair<string, string> kv in dict)
                {
                    queryString += UrlEncode(kv.Key);
                    queryString += "=";
                    queryString += UrlEncode(kv.Value);
                    queryString += "&";
                }
            }
            uriBuilder.Query = queryString;
            return uriBuilder.Uri.ToString();
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
                if (response.IsSuccessStatusCode)
                {
                    result.Result = DeserializeObject<T>(await response.Content.ReadAsStringAsync());
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
            await CheckForErrorResponse(response);
            if (response.IsSuccessStatusCode)
            {
                result.Result = DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
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

        private async Task<SparkApiConnectorApiOperationResult<T>> GetItemAsync<T>(string path)
        {
            var result = new SparkApiConnectorApiOperationResult<T>();
            HttpResponseMessage response = null;
            try
            {
                response = await client.GetAsync(path).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    result.Result = DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
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

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        items = new List<T>();
                        requestResult = JObject.Parse(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
                        results = requestResult[rootNode].Children().ToList();
                        foreach (JToken token in results)
                        {
                            T item = DeserializeObject<T>(token.ToString());
                            items.Add(item);
                        }
                        result.Result = items;
                    }
                    result.ResultCode = MapHttpStatusCode(response.StatusCode);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        result.ResultCode = SparkApiOperationResultCode.OK;
                    }
                    result.ResultCode = MapHttpStatusCode(response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                await ProcessException(result, ex, response);
            }

            return result;
        }

        private async Task ProcessException(GenericOperationResult result, Exception e, HttpResponseMessage response, [CallerMemberName] string methodName = null)
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
                result.ResultCode = MapHttpStatusCode(response.StatusCode);
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
                result.ResultCode = MapHttpStatusCode(response.StatusCode);
            }
        }

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

        #endregion Private Helper Methods

        public async Task<PaginationResult<T>> GetItemsWithLinksAsync<T>(string path)
        {
            HttpResponseMessage response = await client.GetAsync(path);
            await CheckForErrorResponse(response);
            //todo:handle error
            //var errorResponse = await CheckForErrorResponse(response);
            //if (errorResponse is null)
            //{
            //    return null;
            //}

            PaginationResult<T> paginationResult = new PaginationResult<T>();
            List<T> items = new List<T>();
            Links links = new Links();
            JObject requestResult = JObject.Parse(await response.Content.ReadAsStringAsync());
            List<JToken> results = requestResult["items"].Children().ToList();
            foreach (JToken result in results)
            {
                T item = DeserializeObject<T>(result.ToString());
                items.Add(item);
            }
            if (response.Headers.Contains("Link"))
            {
                var link = response.Headers.GetValues("Link").FirstOrDefault();
                if (link != null && !"".Equals(link))
                {
                    // borrowed regex from spark-java-sdk
                    // https://github.com/ciscospark/spark-java-sdk/blob/master/src/main/java/com/ciscospark/LinkedResponse.java
                    Regex r = new Regex("\\s*<(\\S+)>\\s*;\\s*rel=\"(\\S+)\",?", RegexOptions.Compiled);
                    MatchCollection regmatch = r.Matches(link);
                    foreach (Match item in regmatch)
                    {
                        var linktype = item.Groups[2].ToString().ToLower();
                        Uri linkUrl = new Uri(item.Groups[1].ToString());

                        switch (linktype)
                        {
                            case "next":
                                links.Next = linkUrl.PathAndQuery;
                                break;

                            case "prev":
                                links.Prev = linkUrl.PathAndQuery;
                                break;

                            case "first":
                                links.First = linkUrl.PathAndQuery;
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            paginationResult.Items = items;
            paginationResult.Links = links;

            return paginationResult;
        }

        private async Task<SparkErrorContent> CheckForErrorResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                return null;
            }

            SparkErrorContent sparkErrorContent = DeserializeObject<SparkErrorContent>(await response.Content.ReadAsStringAsync());
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
                string requestUri = response?.RequestMessage?.RequestUri?.AbsolutePath;
                try
                {
                    result.Error = JsonConvert.DeserializeObject<SparkErrorContent>(responseData);

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