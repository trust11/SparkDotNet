using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string OutgoingPermissionBaseUrl { get; } = "/v1/people/{0}/features/outgoingPermission";

        /// <summary>
        /// Retrieve a Person's Outgoing Calling Permissions Settings
        /// You can change the outgoing calling permissions for a person if you want them to be different from your organization's default.
        /// This API requires a full, user, or read-only administrator auth token with a scope of spark-admin:people_read.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="orgId"></param>
        /// <returns><c>PersonBargeInSetting</c> object</returns>
        public async Task<SparkApiConnectorApiOperationResult<PersonOutgoingPermissionSettings>> GetPeopleOutgoingCallingPermissionsSettingsAsync(string personId, string orgId = null)
        {
            return await GetPersonSettingAsync<PersonOutgoingPermissionSettings>(OutgoingPermissionBaseUrl, personId, orgId);
        }

        public async Task<SparkApiConnectorApiOperationResult<PersonOutgoingPermissionSettings>> UpdatePeopleOutgoingCallingPermissionsSettingsAsync(string personId, PersonOutgoingPermissionSettings setting, string orgId = null)
        {
            return await UpdatePersonSettingAsync(OutgoingPermissionBaseUrl, personId, setting, orgId);
        }
    }
}