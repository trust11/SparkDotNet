using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string ApplicationBaseUrl { get; } = "/v1/people/{0}/features/applications";

        /// <summary>
        /// Application services let you determine the ringing behavior for calls made to people in
        /// certain scenarios.You can also specify which devices can download the Webex Calling app.
        /// This API requires a full, user, or read-only administrator auth token with a scope of 
        /// spark-admin:people_read.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="orgId"></param>
        /// <returns><c>PersonBargeInSetting</c> object</returns>
        public async Task<SparkApiConnectorApiOperationResult<PersonApplicationServicesSettings>> GetPersonApplicationServicesSettingsAsync(string personId, string orgId = null)
        {
            return await GetPersonSettingAsync<PersonApplicationServicesSettings>(ApplicationBaseUrl, personId, orgId).ConfigureAwait(false);
        }

        public async Task<SparkApiConnectorApiOperationResult<PersonApplicationServicesSettings>> UpdatePersonApplicationServicesSettingsAsync(string personId, PersonApplicationServicesSettings setting, string orgId = null)
        {
            return await UpdatePersonSettingAsync(ApplicationBaseUrl, personId, setting, orgId).ConfigureAwait(false);
        }
    }
}