namespace SparkDotNet.Models
{
    /// <summary>
    /// Settings for forwarding all incoming calls to the destination
    /// </summary>
    public class PersonCallForwardingAlways : WebexObject
    {
        /// <summary>
        /// "Always" call forwarding is enabled or disabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// If true, a brief tone will be played on the person's phone when a call has been
        /// forwarded.
        /// </summary>
        public bool RingReminderEnabled { get; set; }

        /// <summary>
        /// Enables and disables sending incoming calls to voicemail when the destination is an
        /// internal phone number and that number has the voicemail service enabled.
        /// </summary>
        public bool DestinationVoicemailEnabled { get; set; }

        /// <summary>
        /// Destination for "Always" call forwarding.
        /// </summary>
        public string Destination { get; set; }
    }
}