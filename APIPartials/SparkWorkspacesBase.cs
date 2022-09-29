using SparkDotNet.ExceptionHandling;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private async Task<SparkApiConnectorApiOperationResult<T>> GetWorkspaceSettingAsync<T>(string url, string personId, string orgId = null, string profileId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = GetURL(string.Format(url, personId), queryParams);
            return await GetItemAsync<T>(path).ConfigureAwait(false);
        }

        private async Task UpdateWorkspaceSettingAsync<T>(string url, string personId, T personCallSetting, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = GetURL(string.Format(url, personId), queryParams);
            await UpdateItemAsync(path, personCallSetting).ConfigureAwait(false);
        }
    }
}