using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string CallWaitingBaseUrl { get; } = "/v1/people/{0}/features/callWaiting";

        /// <summary>
        /// Retrieve a Person's Barge In Settings The Barge In feature enables you to use a Feature
        /// Access Code(FAC) to answer a call that was directed to another subscriber, or barge-in
        /// on the call if it was already answered.Barge In can be used across locations.
        ///
        /// This API requires a full, user, or read-only administrator auth token with a scope of
        /// spark-admin:people_read or a user auth token with spark:people_read scope can be used by
        /// a person to read their own settings.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="orgId"></param>
        /// <returns><c>PersonBargeInSetting</c> object</returns>
        public async Task<SparkApiConnectorApiOperationResult<PersonCallWaiting>> GetPersonCallWaitingAsync(string personId, string orgId = null)
        {
            return await GetPersonSettingAsync<PersonCallWaiting>(CallWaitingBaseUrl, personId, orgId).ConfigureAwait(false);
        }

        public async Task<SparkApiConnectorApiOperationResult<PersonCallWaiting>> UpdatePersonCallWaitingAsync(string personId, PersonCallWaiting personCallWaiting, string orgId = null)
        {
            return await UpdatePersonSettingAsync(CallWaitingBaseUrl, personId, personCallWaiting, orgId).ConfigureAwait(false);
        }
    }
}