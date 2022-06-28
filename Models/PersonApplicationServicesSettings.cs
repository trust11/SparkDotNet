namespace SparkDotNet.Models
{
    public class PersonApplicationServicesSettings : WebexObject
    {
        /// <summary>
        /// When true, indicates to ring devices for outbound Click to Dial calls.
        /// </summary>
        public bool RingDevicesForClickToDialCallsEnabled { get; set; }
        /// <summary>
        /// When true, indicates to ring devices for inbound Group Pages.
        /// </summary>
        public bool RingDevicesForGroupPageEnabled { get; set; }
        /// <summary>
        /// When true, indicates to ring devices for Call Park recalled.
        /// </summary>
        public bool RingDevicesForCallParkEnabled { get; set; }
        /// <summary>
        /// Indicates that the desktop Webex Calling application is enabled for use.
        /// </summary>
        public bool DesktopClientEnabled { get; set; }
        /// <summary>
        /// Indicates that the tablet Webex Calling application is enabled for use.
        /// </summary>
        public bool TabletClientEnabled { get; set; }
        /// <summary>
        /// Indicates that the mobile Webex Calling application is enabled for use.
        /// </summary>
        public bool MobileClientEnabled { get; set; }
        /// <summary>
        /// Number of available device licenses for assigning devices/apps.
        /// </summary>
        public int AvailableLineCount { get; set; }

    }
}