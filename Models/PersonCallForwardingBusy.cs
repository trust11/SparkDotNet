namespace SparkDotNet.Models
{
    /// <summary>
    /// Settings for forwarding all incoming calls to the destination you chose while the phone is
    /// in use or the person is busy.
    /// </summary>
    public class PersonCallForwardingBusy : WebexObject, IPersonCallForwarding
    {
        /// <summary>
        /// "Busy" call forwarding is enabled or disabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Destination for "Busy" call forwarding.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Enables and disables sending incoming to the destination number's voicemail if the
        /// destination is an internal phone number and that number has the voicemail service
        /// enabled.
        /// </summary>
        public bool DestinationVoicemailEnabled { get; set; }
    }
}