using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string UcManagerProfileBaseUrl { get; } = "/v1/telephony/config/callingProfiles";

        public async Task<SparkApiConnectorApiOperationResult<UcManagerCallingProfiles>> GetUcCallingProfilesAsync(string orgId = null)
        {
            return await GetUcManagerProfilesAsync<UcManagerCallingProfiles>(UcManagerProfileBaseUrl, orgId);
        }
    }
}