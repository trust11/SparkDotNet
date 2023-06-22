namespace SparkDotNet.Models
{
    /// <summary>
    /// Settings for forwarding which only occurs when you are away or not answering your phone.
    /// </summary>
    public class PersonCallForwardingNoAnswer : WebexObject, IPersonCallForwarding
    {
        /// <summary>
        /// "No Answer" call forwarding is enabled or disabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Destination for "No Answer" call forwarding.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Number of rings before the call will be forwarded if unanswered.
        /// </summary>
        public int NumberOfRings { get; set; }

        /// <summary>
        /// No documentation available
        /// https://developer.webex.com/docs/api/v1/webex-calling-person-settings/configure-call-forwarding-settings-for-a-person
        /// </summary>
        public int SystemMaxNumberOfRings { get; set; }

        /// <summary>
        /// Enables and disables sending incoming to destination number's voicemail if the
        /// destination is an internal phone number and that number has the voicemail service
        /// enabled.
        /// </summary>
        public bool DestinationVoicemailEnabled { get; set; }
    }
}