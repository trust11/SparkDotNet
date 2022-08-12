using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Threading.Tasks;

namespace SparkDotNet
{
    /// <summary>
    /// Webex Calling Person Settings supports modifying Webex Calling settings for a specific
    /// person.
    ///
    /// Viewing People requires a full, user, or read-only administrator auth token with a scope of
    /// spark-admin:people_read or, for select APIs, a user auth token with spark:people_read scope
    /// can be used by a person to read their own settings.
    ///
    /// Configuring People settings requires a full or user administrator auth token with the
    /// spark-admin:people_write scope or, for select APIs, a user auth token with
    /// spark:people_write scope can be used by a person to update their own settings.
    /// </summary>
    public partial class Spark
    {
        private static string CallingBehaviorBaseUrl { get; } = "/v1/people/{0}/features/callingBehavior";

        /// <summary>
        /// Retrieves the UC Manager Profile setting for the person.
        /// Note: In the future, calling behavior will be included in this API. UC Manager Profiles
        /// are applicable if your organization uses Jabber in Team Messaging mode or Calling in
        /// Webex Teams(Unified CM). The UC Manager Profile also has an organization-wide default
        /// and may be overridden for individual persons. This API requires a full, user, or
        /// read-only administrator auth token with a scope of spark-admin:people_read.
        /// </summary>
        /// <param name="personId">A unique identifier for the person.</param>
        /// <param name="orgId">A unique identifier for the organisation</param>
        /// <param name="profileId">A unique identifier for the person profile settings</param>
        /// <returns>PersonUcProfileSetting object</returns>
        public async Task<SparkApiConnectorApiOperationResult<PersonUcProfileSetting>> GetPersonUcProfileSettingAsync(string personId, string orgId = null)
        {
            return await GetPersonSettingAsync<PersonUcProfileSetting>(CallingBehaviorBaseUrl, personId, orgId);
        }

        /// <summary>
        /// Modifies the UC Profile setting for the person.
        /// Note: In the future, calling behavior will be included in this API. UC Manager Profiles
        /// are applicable if your organization uses Jabber in Team Messaging mode or Calling in
        /// Webex Teams(Unified CM). The UC Manager Profile has an organization-wide default and may
        /// be overridden for individual persons. This API requires a full or user administrator
        /// auth token with the spark-admin:people_write scope.
        /// </summary>
        /// <param name="personId">A unique identifier for the person.</param>
        /// <param name="orgId">A unique identifier for the organisation</param>
        /// <param name="profileId">A unique identifier for the person profile settings</param>
        /// <returns>Nothing</returns>
        public async Task<SparkApiConnectorApiOperationResult> UpdatePersonUcProfileSettingAsync(string personId, PersonUcProfileSettingConfig personUcProfileSettingConfig, string orgId = null)
        {
            if (personUcProfileSettingConfig.ProfileId == null)// || personUcProfileSettingConfig.BehaviorType == null)
               return await UpdatePersonSettingAsync<PersonUcProfileSetting, object>(CallingBehaviorBaseUrl, personId, new { }, orgId).ConfigureAwait(false);
            return await UpdatePersonSettingAsync<PersonUcProfileSetting, PersonUcProfileSettingConfig>(CallingBehaviorBaseUrl, personId, personUcProfileSettingConfig, orgId).ConfigureAwait(false);
        }

        public async Task<SparkApiConnectorApiOperationResult> UpdatePersonUcProfileSettingAsyncOfficial(string personId, PersonUcProfileSettingConfig personUcProfileSettingConfig, string orgId = null)
        {
            return await UpdatePersonSettingAsync<PersonUcProfileSetting, PersonUcProfileSettingConfig>(CallingBehaviorBaseUrl, personId, personUcProfileSettingConfig, orgId).ConfigureAwait(false);
        }
    }
}