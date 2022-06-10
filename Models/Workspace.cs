using Newtonsoft.Json;

namespace SparkDotNet.Models
{
    /// <summary>
    /// Workspaces represent where people work, such as conference rooms, meeting spaces, lobbies,
    /// and lunch rooms. Devices may be associated with workspaces.
    /// Viewing the list of workspaces in an organization requires an administrator
    /// auth token with the spark-admin:workspaces_read scope.Adding, updating, or
    /// deleting workspaces in an organization requires an administrator auth token
    /// with the spark-admin:workspaces_write scope.
    /// The Workspaces API can also be used by partner administrators acting as administrators
    /// of a different organization than their own. In those cases an orgId value must be supplied,
    /// as indicated in the reference documentation for the relevant endpoints.
    /// </summary>
    public class Workspace : WebexObject
    {
        /// <summary>
        /// Unique identifier for the Workspace.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// A friendly name for the workspace.
        /// </summary>
        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        /// <summary>
        /// How many people the workspace is suitable for.
        /// </summary>
        [JsonProperty("capacity")]
        public int Capacity { get; set; }

        /// <summary>
        /// The workspace type. One of
        /// notSet: No workspace type set.
        /// focus: High concentration.
        /// huddle: Brainstorm/collaboration.
        /// meetingRoom: Dedicated meeting space.
        /// open: Unstructured agile.
        /// desk: Individual.
        /// other: Unspecified.
        /// </summary>
        [JsonProperty("type")]
        public WorkspaceType Type { get; set; }

        /// <summary>
        /// OrgId associate with the workspace
        /// </summary>
        [JsonProperty("orgId")]
        public string OrgId { get; set; }

        /// <summary>
        /// SipUrl to call all the devices associated with the workspace.
        /// </summary>
        [JsonProperty("sipAddress")]
        public string SipAddress { get; set; }

        /// <summary>
        /// The date and time that the workspace was registered, in ISO8601 format.
        /// </summary>
        [JsonProperty("created")]
        public System.DateTime Created { get; set; }

        /// <summary>
        /// Calling type. Possible values: freeCalling, hybridCalling, webexCalling, webexEdgeForDevices
        /// </summary>
        [JsonProperty("calling")]
        public WorkspaceCallingType Calling { get; set; }

        /// <summary>
        /// Calendar type. Possible values: none, google, microsoft.
        /// </summary>
        [JsonProperty("calendar")]
        public WorkspaceCalendar Calendar { get; set; }

        /// <summary>
        /// Notes associated to the workspace.
        /// </summary>
        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>
        /// Location associated with the workspace.
        /// </summary>
        public string WorkspaceLocationId { get; set; }

        /// <summary>
        /// Floor associated with the workspace.
        /// </summary>
        public string FloorId { get; set; }
    }

    public enum WorkspaceType
    {
        /// <summary>
        /// No workspace type set.
        /// </summary>
        NotSet,

        /// <summary>
        /// High concentration.
        /// </summary>
        Focus,

        /// <summary>
        /// Brainstorm/collaboration.
        /// </summary>
        HudDle,

        /// <summary>
        /// Dedicated meeting space.
        /// </summary>
        MeetingRoom,

        /// <summary>
        /// Unstructured agile.
        /// </summary>
        Open,

        /// <summary>
        /// Individual.
        /// </summary>
        Desk,

        /// <summary>
        ///     Unspecified.
        ///     </summary>
        Other
    }
}