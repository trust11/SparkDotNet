using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string CallDoNotDisturbBaseUrl { get; } = "/v1/people/{0}/features/doNotDisturb";



        public async Task<SparkApiConnectorApiOperationResult<PersonCallDoNotDisturbSetting>> GetPersonDoNotDisturbSettingAsync(string personId, string orgId = null)
        {
            return await GetPersonSettingAsync<PersonCallDoNotDisturbSetting>(CallDoNotDisturbBaseUrl, personId, orgId);
        }

        public async Task UpdatePersonDoNotDisturbSettingAsync(string personId, PersonCallDoNotDisturbSetting personCallDoNotDisturbSetting, string orgId = null)
        {
            await UpdatePersonSettingAsync(CallDoNotDisturbBaseUrl, personId, personCallDoNotDisturbSetting, orgId);
        }
    }
}