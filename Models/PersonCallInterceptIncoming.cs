namespace SparkDotNet.Models
{
    /// <summary>
    /// Settings related to how incoming calls are handled when the intercept feature is enabled.
    /// </summary>
    public class PersonCallInterceptIncoming : WebexObject
    {
        /// <summary>
        /// Settings related to how incoming calls are handled when the intercept feature is
        /// enabled.
        /// </summary>
        public PersonCallInterceptAnnouncements Announcements { get; set; }

        /// <summary>
        /// INTERCEPT_TYPE how in- and outgoing calls are intercepted.
        /// </summary>
        public PersonCallInterceptInterceptType Type { get; set; }

        /// <summary>
        /// If true, the destination will be the person's voicemail.
        /// </summary>
        public bool VoicemailEnabled { get; set; }
    }
}