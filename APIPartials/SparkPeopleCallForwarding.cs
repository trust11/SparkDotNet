using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string CallForwardingBaseUrl { get; } = "/v1/people/{0}/features/callForwarding";

        /// <summary>
        /// Read Forwarding Settings for a Person
        ///
        /// Retrieve a Person's Call Forwarding Settings Three types of call forwarding are
        /// supported:
        /// - Always – forwards all incoming calls to the destination you choose.
        /// - When busy – forwards all incoming calls to the destination you chose while the phone
        /// is in use or the person is busy.
        /// - When no answer – forwarding only occurs when you are away or not answering your phone.
        ///
        /// In addition, the Business Continuity feature will send calls to a destination of your
        /// choice if your phone is not connected to the network for any reason, such as power
        /// outage, failed Internet connection, or wiring problem
        ///
        /// This API requires a full, user, or read-only administrator auth token with a scope of
        /// spark-admin:people_read or a user auth token with spark:people_read scope can be used by
        /// a person to read their own settings.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="orgId"></param>
        /// <returns>PersonCallForwardingSetting object</returns>
        public async Task<SparkApiConnectorApiOperationResult<PersonCallForwardingSetting>> GetPersonCallForwardingSettingAsync(string personId, string orgId = null)
        {
            return await GetPersonSettingAsync<PersonCallForwardingSetting>(CallForwardingBaseUrl, personId, orgId).ConfigureAwait(false);
        }

        public async Task<SparkApiConnectorApiOperationResult<PersonCallForwardingSetting>> UpdatePersonCallForwardingSettingAsync(string personId, PersonCallForwardingSetting personCallForwardingSetting, string orgId = null)
        {
           return await UpdatePersonSettingAsync(CallForwardingBaseUrl, personId, personCallForwardingSetting, orgId).ConfigureAwait(false);
        }
    }
}