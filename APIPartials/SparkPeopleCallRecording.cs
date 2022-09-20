using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public class Notification:WebexObject
    {
        public bool Enabled { get; set; } = false;
    }

    public class Repeat:WebexObject
    {
        public IntervalTimeInSeconds Interval { get; set; } = IntervalTimeInSeconds.Default;

        public bool Enabled { get; set; } = false;
    }

    public enum IntervalTimeInSeconds 
    {
        [EnumMember(Value = "15")]
        Default = 15,
        [EnumMember(Value = "10")]
        Then = 10,
        [EnumMember(Value = "20")]
        Twenty = 20,
        [EnumMember(Value = "30")]
        Thirty = 30,
        [EnumMember(Value = "40")]
        Forty = 40,
        [EnumMember(Value = "50")]
        Fifty = 50,
        [EnumMember(Value = "60")]
        Sixty = 60,
        [EnumMember(Value = "70")]
        Seventy = 70,
        [EnumMember(Value = "80")]
        Eighty = 80,
        [EnumMember(Value = "90")]
        Ninety = 90
    }

    /// <summary>
    /// Specified under which scenarios calls will be recorded.
    /// </summary>
    public enum Record
    {
        /// <summary>
        /// Incoming and outgoing calls will be recorded with no control to start, stop, pause, or resume.
        /// </summary>
        [EnumMember(Value = "Always")]
        Always,

        /// <summary>
        /// Calls will not be recorded.
        /// </summary>
        [EnumMember(Value = "Never")]
        Never,

        /// <summary>
        /// Calls are always recorded, but user can pause or resume the recording. Stop recording is not supported.
        /// </summary>
        [EnumMember(Value = "Always with Pause/Resume")]
        AlwaysWithPauseResume,

        /// <summary>
        /// Records only the portion of the call after the recording start (*44) has been entered. Pause, resume, and stop controls are supported.
        /// </summary>
        [EnumMember(Value = "On Demand with User Initiated Start")]
        OnDemandWithUserInitiatedStart
    }

    public partial class Spark
    {
        private readonly string peopleCallRecordingBase = "/v1/people/{0}/features/callRecording";

        /// <summary>
        /// Retrieve a Person's Call Recording Settings
        /// The Call Recording feature provides a hosted mechanism to record the calls placed and received on
        /// the Carrier platform for replay and archival. This feature is helpful for quality assurance, security, training, and more.
        /// This API requires a full or user administrator auth token with the spark-admin:people_write scope.
        /// </summary>
        /// <param name="orgId">List people in this organization. Only admin users of another organization (such as partners) may use this parameter.</param>
        /// <returns>List of People Call Recording objects.</returns>
        public async Task<SparkApiConnectorApiOperationResult<PersonCallRecording>> GetPeopleCallRecordingSettingsAsync(string personId = null, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = GetURL(string.Format(peopleCallRecordingBase, personId), queryParams);
            return await GetItemAsync<PersonCallRecording>(path);
        }

        /// <summary>
        /// Update Call Recording Settings for a person, by ID.
        /// The Call Recording feature provides a hosted mechanism to record the calls placed and received
        /// on the Carrier platform for replay and archival. This feature is helpful for quality assurance, security, training, and more.
        /// This API requires a full or user administrator auth token with the spark-admin:people_write scope.
        /// </summary>
        /// <param name="personId">A unique identifier for the person.</param>
        /// <param name="orgId">The ID of the organization to which this person belongs.</param>
        /// <param name="enabled">true if call recording is enabled.</param>
        /// <param name="record"></param>
        /// <param name="recordVoicemailEnabled"></param>
        /// <param name="startStopAnnouncementEnabled"></param>
        /// <param name="notification"></param>
        /// <param name="repeat"></param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult<PersonCallRecording>> UpdatePersonCallRecordingSettingsAsync(
            string personId,//URI param
            string orgId = null,//Query param
            bool? enabled = null, Record? record = null, bool? recordVoicemailEnabled = null, bool? startStopAnnouncementEnabled = null, Notification notification = null, Repeat repeat = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = GetURL(string.Format(peopleCallRecordingBase, personId), queryParams);

            var putBody = new Dictionary<string, object>();
            if (enabled != null) putBody.Add("enabled", enabled);
            if (record != null) putBody.Add("record", record);
            if (recordVoicemailEnabled != null) putBody.Add("recordVoicemailEnabled", recordVoicemailEnabled);
            if (startStopAnnouncementEnabled != null) putBody.Add("startStopAnnouncementEnabled", startStopAnnouncementEnabled);
            if (notification != null) putBody.Add("notification", notification);
            if (repeat != null) putBody.Add("repeat", repeat);
            return await UpdateItemAsync<PersonCallRecording>(path, putBody);
        }

        /// <summary>
        /// Update Call Recording Settings for a person, by ID.
        /// The Call Recording feature provides a hosted mechanism to record the calls placed and received
        /// on the Carrier platform for replay and archival. This feature is helpful for quality assurance, security, training, and more.
        /// This API requires a full or user administrator auth token with the spark-admin:people_write scope.
        /// </summary>
        /// <param name="personCallRecording">The person call recording object to update</param>
        /// <returns>Person object.</returns>
        public async Task<SparkApiConnectorApiOperationResult<PersonCallRecording>> UpdatePersonCallRecordingSettingsAsync(string personId, PersonCallRecording personCallRecording, string orgId = null)
        {
            return await UpdatePersonCallRecordingSettingsAsync(personId, orgId,
                personCallRecording.Enabled, personCallRecording.Record, personCallRecording.RecordVoicemailEnabled,
                personCallRecording.StartStopAnnouncementEnabled, personCallRecording.Notification, personCallRecording.Repeat);
        }
    }
}