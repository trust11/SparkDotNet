using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string UcManagerProfileBaseUrl { get; } = "/v1/telephony/config/callingProfiles";

        /// <summary>
        /// Link to the <see href="https://developer.webex.com/docs/api/v1/webex-calling-organization-settings/read-the-list-of-uc-manager-profiles/">Webex Documentation</see>
        /// </summary>
        /// <param name="orgId">The organisation ID</param>
        /// <returns><c>UcManagerCallingProfiles</c></returns>
        public async Task<SparkApiConnectorApiOperationResult<UcManagerCallingProfiles>> GetUcCallingProfilesAsync(string orgId = null) =>
            await GetUcManagerProfilesAsync<UcManagerCallingProfiles>(UcManagerProfileBaseUrl, orgId);
    }
}