using SparkDotNet.ExceptionHandling;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private async Task<SparkApiConnectorApiOperationResult<T>> GetPersonSettingAsync<T>(string url, string personId, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = GetURL(string.Format(url, personId), queryParams);
            return await GetItemAsync<T>(path).ConfigureAwait(false);
        }

        private async Task<SparkApiConnectorApiOperationResult<T>> UpdatePersonSettingAsync<T>(string url, string personId, T setting, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = GetURL(string.Format(url, personId), queryParams);
            return await UpdateItemAsync(path, setting).ConfigureAwait(false);
        }

        private async Task<SparkApiConnectorApiOperationResult<T>> UpdatePersonSettingAsync<T,U>(string url, string personId, U setting, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = GetURL(string.Format(url, personId), queryParams);
            return await UpdateItemAsync<T,U>(path, setting).ConfigureAwait(false);
        }
    }
}