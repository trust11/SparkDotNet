using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string CallerIdBaseUrl { get; } = "/v1/people/{0}/features/callerId";

        /// <summary>
        /// Caller ID settings control how a person's information is displayed when making outgoing calls.
        /// This API requires a full, user, or read-only administrator authentication token with a scope of spark-admin:people_read
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="orgId"></param>
        /// <returns><c>PersonCallerIdSetting</c> object</returns>
        public async Task<SparkApiConnectorApiOperationResult<PersonCallerIdSetting>> GetPersonCallerIdAsync(string personId, string orgId = null)
        {
            return await GetPersonSettingAsync<PersonCallerIdSetting>(CallerIdBaseUrl, personId, orgId);
        }

        public async Task<SparkApiConnectorApiOperationResult<PersonCallerIdUpdateSetting>> UpdatePersonCallerIdAsync(string personId, PersonCallerIdUpdateSetting personCallerIdSetting, string orgId = null)
        {
            return await UpdatePersonSettingAsync<PersonCallerIdUpdateSetting>(CallerIdBaseUrl, personId, personCallerIdSetting, orgId);
        }
    }
}