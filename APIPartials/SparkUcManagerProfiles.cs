using SparkDotNet.ExceptionHandling;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private async Task<SparkApiConnectorApiOperationResult<T>> GetUcManagerProfilesAsync<T>(string url, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = GetURL(url, queryParams);
            return await GetItemAsync<T>(path);
        }
    }
}