using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    /// <summary>
    /// Webex Calling Person Settings supports modifying Webex Calling settings for a specific person.
    ///
    /// Viewing People requires a full, user, or read-only administrator auth token with a scope
    /// of spark-admin:people_read or, for select APIs, a user auth token with spark:people_read scope
    /// can be used by a person to read their own settings.
    ///
    /// Configuring People settings requires a full or user administrator auth token with the
    /// spark-admin:people_write scope or, for select APIs, a user auth token with spark:people_write
    /// scope can be used by a person to update their own settings.
    /// </summary>
    public partial class Spark
    {
        private const string PersonSettingBaseUrl = "/v1/people/{0}";
        private readonly string callingBehaviorBaseUrl = $"{PersonSettingBaseUrl}/features/callingBehavior";
        private readonly string bargeInBaseUrl = $"{PersonSettingBaseUrl}/features/bargeIn";
        private readonly string callForwardingBaseUrl = $"{PersonSettingBaseUrl}/features/callForwarding";
        private readonly string callInterceptBaseUrl = $"{PersonSettingBaseUrl}/features/intercept";

        /// <summary>
        /// Retrieves the UC Manager Profile setting for the person.
        /// Note: In the future, calling behavior will be included in this API.
        /// UC Manager Profiles are applicable if your organization uses Jabber in Team Messaging mode or Calling in Webex Teams(Unified CM).
        /// The UC Manager Profile also has an organization-wide default and may be overridden for individual persons.
        /// This API requires a full, user, or read-only administrator auth token with a scope of spark-admin:people_read.
        /// </summary>
        /// <param name="personId">A unique identifier for the person.</param>
        /// <param name="orgId">A unique identifier for the organisation</param>
        /// <param name="profileId">A unique identifier for the person profile settings</param>
        /// <returns>PersonUcProfileSetting object</returns>
        public async Task<PersonUcProfileSetting> GetPersonUcProfileSettingAsync(string personId, string orgId = null, string profileId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            if (profileId != null) queryParams.Add("profileId", profileId);
            var path = getURL(string.Format(callingBehaviorBaseUrl, personId), queryParams);
            return await GetItemAsync<PersonUcProfileSetting>(path);
        }

        /// <summary>
        /// Modifies the UC Profile setting for the person.
        /// Note: In the future, calling behavior will be included in this API.
        /// UC Manager Profiles are applicable if your organization uses Jabber in Team Messaging mode or Calling in Webex Teams(Unified CM).
        /// The UC Manager Profile has an organization-wide default and may be overridden for individual persons.
        /// This API requires a full or user administrator auth token with the spark-admin:people_write scope.
        /// </summary>
        /// <param name="personId">A unique identifier for the person.</param>
        /// <param name="orgId">A unique identifier for the organisation</param>
        /// <param name="profileId">A unique identifier for the person profile settings</param>
        /// <returns>Nothing</returns>
        public async Task UpdatePersonUcProfileSettingAsync(string personId, string orgId = null, string profileId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = getURL(string.Format(callingBehaviorBaseUrl, personId), queryParams);

            var putBody = new Dictionary<string, object>();
            putBody.Add("profileId", profileId);

            await UpdateItemAsync(path, putBody);
        }

        /// <summary>
        /// Retrieve a Person's Barge In Settings
        /// The Barge In feature enables you to use a Feature Access Code(FAC) to answer a call
        /// that was directed to another subscriber, or barge-in on the call if it was already
        /// answered.Barge In can be used across locations.
        ///
        /// This API requires a full, user, or read-only administrator auth token with a scope of
        /// spark-admin:people_read or a user auth token with spark:people_read scope can be
        /// used by a person to read their own settings.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="orgId"></param>
        /// <returns><c>PersonBargeInSetting</c> object</returns>
        public async Task<PersonBargeInSetting> GetPersonCallBargeSettingAsync(string personId, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = getURL(string.Format(bargeInBaseUrl, personId), queryParams);
            return await GetItemAsync<PersonBargeInSetting>(path);
        }

        public async Task UpdatePersonBargeSettingAsync(string personId, PersonBargeInSetting personBargeInSetting, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = getURL(string.Format(bargeInBaseUrl, personId), queryParams);

            //var putBody = new Dictionary<string, object>();
            //putBody.Add("enabled", personBargeInSetting.Enabled);
            //putBody.Add("toneEnabled", personBargeInSetting.ToneEnabled);

            await UpdateItemAsync(path, personBargeInSetting);
        }

        /// <summary>
        /// Read Forwarding Settings for a Person
        /// 
        /// Retrieve a Person's Call Forwarding Settings
        /// Three types of call forwarding are supported:
        /// - Always – forwards all incoming calls to the destination you choose.
        /// - When busy – forwards all incoming calls to the destination you chose while the phone is in use or the person is busy.
        /// - When no answer – forwarding only occurs when you are away or not answering your phone.
        ///
        /// In addition, the Business Continuity feature will send calls to a destination of
        /// your choice if your phone is not connected to the network for any reason,
        /// such as power outage, failed Internet connection, or wiring problem
        /// 
        /// This API requires a full, user, or read-only administrator auth token with a scope
        /// of spark-admin:people_read or a user auth token with spark:people_read scope can be
        /// used by a person to read their own settings.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="orgId"></param>
        /// <returns>PersonCallForwardingSetting object</returns>
        public async Task<PersonCallForwardingSetting> GetPersonCallForwardingSetting(string personId, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = getURL(string.Format(callForwardingBaseUrl, personId), queryParams);
            return await GetItemAsync<PersonCallForwardingSetting>(path);
        }

        public async Task UpdatePersonCallForwardingSettingAsync(string personId, PersonCallForwardingSetting personCallForwardingSetting, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = getURL(string.Format(callForwardingBaseUrl, personId), queryParams);

            await UpdateItemAsync(path, personCallForwardingSetting);
        }

        /// <summary>
        /// Read Call Intercept Settings for a Person
        /// Retrieves Person's Call Intercept Settings
        /// The intercept feature gracefully takes a person’s phone out of service, while providing callers with informative
        /// announcements and alternative routing options.Depending on the service configuration, none, some, or all incoming
        /// calls to the specified person are intercepted.Also depending on the service configuration, outgoing calls are
        /// intercepted or rerouted to another location.
        /// 
        /// This API requires a full, user, or read-only administrator auth token with a scope of spark-admin:people_read.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="orgId"></param>
        /// <returns></returns>
        public async Task<PersonCallInterceptSetting> GetPersonCallInterceptSetting(string personId, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = getURL(string.Format(callInterceptBaseUrl, personId), queryParams);
            return await GetItemAsync<PersonCallInterceptSetting>(path);
        }

        public async Task UpdatePersonCallInterceptSettingAsync(string personId, PersonCallInterceptSetting personCallInterceptSetting, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = getURL(string.Format(callInterceptBaseUrl, personId), queryParams);

            await UpdateItemAsync(path, personCallInterceptSetting);
        }

    }
}