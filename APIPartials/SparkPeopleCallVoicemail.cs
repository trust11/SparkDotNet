using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string CallVoicemailBaseUrl { get; } = "/v1/people/{0}/features/voicemail";

        /// <summary>
        /// Read Voicemail Settings for a Person
        /// Retrieve a person's Voicemail settings.
        /// The voicemail feature transfers callers to voicemail based on your settings.You can then retrieve voice messages via Voicemail.Voicemail audio is sent in Waveform Audio File Format, .wav, format
        /// Optionally, notifications can be sent to a mobile phone via text or email. These notifications will not include the voicemail files.
        /// This API requires a full, user, or read-only administrator auth token with a scope of spark-admin:people_read or a user auth token with spark:people_read scope can be used by a person to read their settings.
        /// </summary>
        public async Task<SparkApiConnectorApiOperationResult<PersonCallVoicemailSetting>> GetPersonVoicemailSettingAsync(string personId, string orgId = null)
        {
            return await GetPersonSettingAsync<PersonCallVoicemailSetting>(CallVoicemailBaseUrl, personId, orgId).ConfigureAwait(false);
        }

        public async Task<SparkApiConnectorApiOperationResult<PersonCallVoicemailSetting>> UpdatePersonVoicemailSettingAsync(string personId, PersonCallVoicemailSetting personCallVoicemailSetting, string orgId = null)
        {
            return await UpdatePersonSettingAsync(CallVoicemailBaseUrl, personId, personCallVoicemailSetting, orgId).ConfigureAwait(false);
        }

        /// <summary>
        /// Reset a voicemail PIN for a person.
        /// The voicemail feature transfers callers to voicemail based on your settings.You can then retrieve voice messages via Voicemail.A voicemail PIN is used to retrieve your voicemail messages.
        /// This API requires a full or user administrator auth token with the spark-admin:people_write scope.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult> ResetPersonVoicemailPin(string personId, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = GetURL(string.Format($"{CallVoicemailBaseUrl}/actions/resetPin/invoke", personId), queryParams);
            string data = null;
            return await PostItemAsync(path, data).ConfigureAwait(false);
        }
    }
}