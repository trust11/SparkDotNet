using SparkDotNet.Models;

using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string IncomingPermissionBaseUrl { get; } = "/v1/people/{0}/features/incomingPermission";

        /// <summary>
        /// Retrieve a Person's Incoming Permission Settings
        /// You can change the incoming calling permissions for a person if you want them to be different from your organization's default.
        /// This API requires a full, user, or read-only administrator auth token with a scope of spark-admin:people_read.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="orgId"></param>
        /// <returns><c>PersonBargeInSetting</c> object</returns>
        public async Task<SparkApiConnectorApiOperationResult<PersonIncomingPermissionSettings>> GetPeopleIncomingCallingPermissionsSettingsAsync(string personId, string orgId = null)
        {
            return await GetPersonSettingAsync<PersonIncomingPermissionSettings>(IncomingPermissionBaseUrl, personId, orgId).ConfigureAwait(false);
        }

        /// <summary>
        /// Configure Incoming Permission Settings for a Person
        /// Configure a person's Incoming Permission settings.
        /// You can change the incoming calling permissions for a person if you want them to be different from your organization's default.
        /// This API requires a full or user administrator auth token with the spark-admin:people_write scope.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="payload"></param>
        /// <param name="orgId"></param>
        public async Task<SparkApiConnectorApiOperationResult<PersonIncomingPermissionSettings>> UpdatePeopleIncomingCallingPermissionsSettingsAsync(string personId, PersonIncomingPermissionSettings setting, string orgId = null)
        {
            return await UpdatePersonSettingAsync(IncomingPermissionBaseUrl, personId, setting, orgId).ConfigureAwait(false);
        }
    }
}