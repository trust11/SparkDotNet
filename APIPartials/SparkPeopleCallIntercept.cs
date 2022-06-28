using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{

    public partial class Spark
    {
        private static string CallInterceptBaseUrl { get; } = "/v1/people/{0}/features/intercept";


        /// <summary>
        /// Read Call Intercept Settings for a Person Retrieves Person's Call Intercept Settings The
        /// intercept feature gracefully takes a person’s phone out of service, while providing
        /// callers with informative announcements and alternative routing options.Depending on the
        /// service configuration, none, some, or all incoming calls to the specified person are
        /// intercepted.Also depending on the service configuration, outgoing calls are intercepted
        /// or rerouted to another location.
        ///
        /// This API requires a full, user, or read-only administrator auth token with a scope of
        /// spark-admin:people_read.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult<PersonCallInterceptSetting>> GetPersonCallInterceptSettingAsync(string personId, string orgId = null)
        {
            return await GetPersonSettingAsync<PersonCallInterceptSetting>(CallInterceptBaseUrl, personId, orgId);
        }

        public async Task<SparkApiConnectorApiOperationResult<PersonCallInterceptSetting>> UpdatePersonCallInterceptSettingAsync(string personId, PersonCallInterceptSetting personCallInterceptSetting, string orgId = null)
        {
            return await UpdatePersonSettingAsync(CallInterceptBaseUrl, personId, personCallInterceptSetting, orgId);
        }
    }
}