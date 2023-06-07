using Newtonsoft.Json;

using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;

using System;
using SparkDotNet.Models.PersonCallVoicemail;

using System.Collections.Generic;
using System.Reflection;
using System.Runtime;
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
    }
}