
using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{

    public partial class Spark
    {

        private readonly string meetingsPreferencesBase = "/v1/meetingPreferences";

        /// <summary>
        /// Retrieves the list of Webex sites that the authenticated user is set up to use.
        /// </summary>
        /// <param name="userEmail">Email address for the user. This parameter is only used if the user or application calling the API has the admin-level scopes. If set, the admin may specify the email of a user and the API will return the list of Webex sites for that user.</param>
        /// <returns>Array of sites for the user. Users can have one site or multiple sites. This concept is specific to Webex Meetings. Any siteUrl in the site list can be assigned as user's default site by Update Default Site of Meeting Preference API.</returns>
        public async Task<SparkApiConnectorApiOperationResult<List<MeetingSite>>> GetMeetingSitesAsync(string userEmail = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (userEmail != null) queryParams.Add("userEmail", userEmail);

            var path = GetURL($"{meetingsPreferencesBase}/sites", queryParams);
            return await GetItemsAsync<MeetingSite>(path, "sites").ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the default site for the authenticated user.
        /// </summary>
        /// <param name="defaultSite">Whether or not to change user's default site. Note: defaultSite must be true.</param>
        /// <param name="siteUrl">Access URL for the site.</param>
        /// <returns>The updated MeetingSite object</returns>
        public async Task<SparkApiConnectorApiOperationResult<MeetingSite>> UpdateDefaultMeetingSite(bool defaultSite, string siteUrl)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "defaultSite", defaultSite.ToString().ToLower() }
            };
            var path = GetURL($"{meetingsPreferencesBase}/sites", queryParams);

            var postBody = new Dictionary<string, object>
            {
                { "siteUrl", siteUrl }
            };

            return await PostItemAsync<MeetingSite>(path, postBody).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves scheduling options for the authenticated user.
        /// </summary>
        /// <param name="userEmail">Email address for the user. This parameter is only used if the user or application calling the API has the admin-level scopes. If set, the admin may specify the email of a user in a site they manage and the API will return details of the scheduling options for that user.</param>
        /// <param name="siteUrl">URL of the Webex site to query. For individual use, if siteUrl is not specified, the query will use the default site of the user. For admin use, if siteUrl is not specified, the query will use the default site for the admin's authorization token used to make the call. In the case where the user belongs to a site different than the admin�s default site, the admin can set the site to query using the siteUrl parameter. All available Webex sites and default site of a user can be retrieved from /meetingPreferences/sites.</param>
        /// <returns>Meeting Scheduling Options object for the user.</returns>
        public async Task<SparkApiConnectorApiOperationResult<MeetingSchedulingOptions>> GetMeetingSchedulingOptionsAsync(string userEmail = null, string siteUrl = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (userEmail != null) queryParams.Add("userEmail", userEmail);
            if (siteUrl != null) queryParams.Add("siteUrl", siteUrl);

            var path = GetURL($"{meetingsPreferencesBase}/schedulingOptions", queryParams);

            return await GetItemAsync<MeetingSchedulingOptions>(path).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates scheduling options for the authenticated user.
        /// </summary>
        /// <param name="enabledJoinBeforeHost">Flag to enable/disable Join Before Host. The period during which invitees can join before the start time is defined by autoLockMinutes. This attribute can be modified by Update Scheduling Options API. Note: This feature is only effective if the site supports the Join Before Host feature. This attribute can be modified by Update Scheduling Options API.</param>
        /// <param name="joinBeforeHostMinutes">Number of minutes before the start time that an invitee can join a meeting if enabledJoinBeforeHost is true. Valid options are 0, 5, 10 and 15. This attribute can be modified by Update Scheduling Options API.</param>
        /// <param name="enabledAutoShareRecording">Flag to enable/disable the automatic sharing of the meeting recording with invitees when it is available. This attribute can be modified by Update Scheduling Options API.</param>
        /// <returns>The updated Meeting Scheduling Options</returns>
        public async Task<SparkApiConnectorApiOperationResult<MeetingSchedulingOptions>> UpdateMeetingSchedulingOptionsAsync(bool enabledJoinBeforeHost, int joinBeforeHostMinutes, bool enabledAutoShareRecording)
        {
            var postBody = new Dictionary<string, object>
            {
                { "enabledJoinBeforeHost", enabledJoinBeforeHost },
                { "joinBeforeHostMinutes", joinBeforeHostMinutes },
                { "enabledAutoShareRecording", enabledAutoShareRecording }
            };

            return await UpdateItemAsync<MeetingSchedulingOptions>($"{meetingsPreferencesBase}/schedulingOptions", postBody).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates scheduling options for the authenticated user.
        /// </summary>
        /// <param name="meetingSchedulingOptions">The new scheduling options</param>
        /// <returns>The updated Meeting Scheduling Options</returns>
        public async Task<SparkApiConnectorApiOperationResult<MeetingSchedulingOptions>> UpdateMeetingSchedulingOptionsAsync(MeetingSchedulingOptions meetingSchedulingOptions)
        => await UpdateMeetingSchedulingOptionsAsync(meetingSchedulingOptions.EnabledJoinBeforeHost, meetingSchedulingOptions.JoinBeforeHostMinutes, meetingSchedulingOptions.EnabledAutoShareRecording).ConfigureAwait(false);

        /// <summary>
        /// Retrieves video options for the authenticated user.
        /// </summary>
        /// <param name="userEmail">Email address for the user. This parameter is only used if the user or application calling the API has the admin-level scopes. If set, the admin may specify the email of a user in a site they manage and the API will return details of the scheduling options for that user.</param>
        /// <param name="siteUrl">URL of the Webex site to query. For individual use, if siteUrl is not specified, the query will use the default site of the user. For admin use, if siteUrl is not specified, the query will use the default site for the admin's authorization token used to make the call. In the case where the user belongs to a site different than the admin�s default site, the admin can set the site to query using the siteUrl parameter. All available Webex sites and default site of a user can be retrieved from /meetingPreferences/sites.</param>
        /// <returns>List of possible Video Device Objects</returns>
        public async Task<SparkApiConnectorApiOperationResult<List<MeetingVideoDevice>>> GetMeetingVideoDevicesAsync(string userEmail = null, string siteUrl = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (userEmail != null) queryParams.Add("userEmail", userEmail);
            if (siteUrl != null) queryParams.Add("siteUrl", siteUrl);

            var path = GetURL($"{meetingsPreferencesBase}/video", queryParams);
            return await GetItemsAsync<MeetingVideoDevice>(path, "videoDevices").ConfigureAwait(false);
        }

        /// <summary>
        /// Updates video options for the authenticated user.
        /// </summary>
        /// <param name="meetingVideoDevices">Array of video devices. If the array is not empty, one device and no more than one devices must be set as default device.</param>
        /// <returns>List of Video Devices</returns>
        public async Task<SparkApiConnectorApiOperationResult<List<MeetingVideoDevice>>> UpdateMeetingVideoDevicesAsync(List<MeetingVideoDevice> meetingVideoDevices)
        {
            var postBody = new Dictionary<string, object>
            {
                { "meetingVideoDevices", meetingVideoDevices }
            };

            // @flag @todo
            // This will not work as it returns an array instead of a single element
            return await UpdateItemAsync<List<MeetingVideoDevice>>($"{meetingsPreferencesBase}/video", postBody).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the Personal Meeting Room options for the authenticated user.
        /// </summary>
        /// <returns>A meeting room option object</returns>
        public async Task<SparkApiConnectorApiOperationResult<MeetingPreferencesPMR>> GetPersonalMeetingRoomOptionsAsync(string userEmail = null, string siteUrl = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (userEmail != null) queryParams.Add("userEmail", userEmail);
            if (siteUrl != null) queryParams.Add("siteUrl", siteUrl);

            var path = GetURL($"{meetingsPreferencesBase}/personalMeetingRoom", queryParams);
            return await GetItemAsync<MeetingPreferencesPMR>(path).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the personal room settings
        /// </summary>
        /// <param name="topic">Personal Meeting Room topic to be updated.</param>
        /// <param name="hostPin">Updated PIN for joining the room as host. The host PIN must be digits of a predefined length, e.g. 4 digits. It cannot contain sequential digits, such as 1234 or 4321, or repeated digits of the predefined length, such as 1111. The predefined length for host PIN can be viewed in user's My Personal Room page and it can only be changed by site administrator.</param>
        /// <param name="enableAutoLock">Update for option to automatically lock the Personal Room a number of minutes after a meeting starts. When a room is locked, invitees cannot enter until the owner admits them. The period after which the meeting is locked is defined by autoLockMinutes.</param>
        /// <param name="autoLockMinutes">Updated number of minutes after which the Personal Room is locked if enabledAutoLock is enabled. Valid options are 0, 5, 10, 15 and 20.</param>
        /// <param name="enabledNotifyHost">Update for flag to enable notifying the owner of a Personal Room when someone enters the Personal Room lobby while the owner is not in the room.</param>
        /// <param name="supportCoHost">Update for flag allowing other invitees to host a meetingCoHost in the Personal Room without the owner.</param>
        /// <param name="supportAnyoneAsCoHost">Whether or not to allow any attendee with a host account on the target site to become a cohost when joining the Personal Room. The target site is user's preferred site.</param>
        /// <param name="coHosts">Updated array defining cohosts for the room if both supportAnyoneAsCoHost and allowFirstUserToBeCoHost are false</param>
        /// <param name="allowFirstUserToBeCoHost">Whether or not to allow the first attendee with a host account on the target site to become a cohost when joining the Personal Room. The target site is user's preferred site.</param>
        /// <param name="allowAuthenticatedDevices">Whether or not to allow authenticated video devices in the user's organization to start or join the meeting without a prompt.</param>
        /// <param name="userEmail">Email address for the user. This parameter is only used if the user or application calling the API has the admin-level scopes. If set, the admin may specify the email of a user in a site they manage and the API will update Personal Meeting Room options for that user.</param>
        /// <param name="siteUrl">URL of the Webex site to query. For individual use, if siteUrl is not specified, the query will use the default site of the user. For admin use, if siteUrl is not specified, the query will use the default site for the admin's authorization token used to make the call. In the case where the user belongs to a site different than the admin�s default site, the admin can set the site to query using the siteUrl parameter. All available Webex sites and default site of a user can be retrieved from /meetingPreferences/sites.</param>
        /// <returns>The updated personal room options</returns>
        public async Task<SparkApiConnectorApiOperationResult<MeetingPreferencesPMR>> UpdatePersonalMeetingRoomOptionsAsync(string topic, string hostPin, bool enableAutoLock, int autoLockMinutes, bool enabledNotifyHost,
                                                                                       bool supportCoHost, bool supportAnyoneAsCoHost, List<MeetingPreferencesCoHost> coHosts,
                                                                                       bool? allowFirstUserToBeCoHost = null, bool? allowAuthenticatedDevices = null,
                                                                                       string userEmail = null, string siteUrl = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (userEmail != null) queryParams.Add("userEmail", userEmail);
            if (siteUrl != null) queryParams.Add("siteUrl", siteUrl);

            var postBody = new Dictionary<string, object>
            {
                { "topic", topic },
                { "hostPin", hostPin },
                { "enableAutoLock", enableAutoLock },
                { "autoLockMinutes", autoLockMinutes },
                { "enabledNotifyHost", enabledNotifyHost },
                { "supportCoHost", supportCoHost },
                { "supportAnyoneAsCoHost", supportAnyoneAsCoHost },
                { "coHosts", coHosts }
            };
            if (allowFirstUserToBeCoHost != null) postBody.Add("allowFirstUserToBeCoHost", allowFirstUserToBeCoHost);
            if (allowAuthenticatedDevices != null) postBody.Add("allowAuthenticatedDevices", allowAuthenticatedDevices);
            if (userEmail != null) postBody.Add("userEmail", userEmail);
            if (siteUrl != null) postBody.Add("siteUrl", siteUrl);

            var path = GetURL($"{meetingsPreferencesBase}/personalMeetingRoom", queryParams);
            return await UpdateItemAsync<MeetingPreferencesPMR>(path, postBody).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the personal room settings
        /// </summary>
        /// <param name="pmr">The personal room options object</param>
        /// <param name="userEmail">Email address for the user. This parameter is only used if the user or application calling the API has the admin-level scopes. If set, the admin may specify the email of a user in a site they manage and the API will update Personal Meeting Room options for that user.</param>
        /// <param name="siteUrl">URL of the Webex site to query. For individual use, if siteUrl is not specified, the query will use the default site of the user. For admin use, if siteUrl is not specified, the query will use the default site for the admin's authorization token used to make the call. In the case where the user belongs to a site different than the admin�s default site, the admin can set the site to query using the siteUrl parameter. All available Webex sites and default site of a user can be retrieved from /meetingPreferences/sites.</param>
        /// <returns>The updated personal room options</returns>
        public async Task<SparkApiConnectorApiOperationResult<MeetingPreferencesPMR>> UpdatePersonalMeetingRoomOptionsAsync(MeetingPreferencesPMR pmr, string userEmail = null, string siteUrl = null)
        => await UpdatePersonalMeetingRoomOptionsAsync(pmr.Topic, pmr.HostPin, pmr.EnabledAutoLock, pmr.AutoLockMinutes, pmr.EnabledNotifyHost, pmr.SupportCoHost, pmr.SupportAnyoneAsCoHost, pmr.ChoHosts, pmr.AllowFirstUserToBeCoHost, pmr.AllowAuthenticatedDevices, userEmail, siteUrl).ConfigureAwait(false);
    }
}