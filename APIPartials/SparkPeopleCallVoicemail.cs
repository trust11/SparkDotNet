using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string CallVoicemailBaseUrl { get; } = "/v1/people/{0}/features/voicemail";


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