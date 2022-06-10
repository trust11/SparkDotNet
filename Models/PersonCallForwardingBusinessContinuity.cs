namespace SparkDotNet.Models
{
    /// <summary>
    /// Settings for sending calls to a destination of your choice if your phone is not connected to
    /// the network for any reason, such as power outage, failed Internet connection, or wiring
    /// problem.
    /// </summary>
    public class PersonCallForwardingBusinessContinuity : WebexObject
    {
        /// <summary>
        /// Business Continuity is enabled or disabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Destination for Business Continuity.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// Enables and disables sending incoming to destination number's voicemail if the
        /// destination is an internal phone number and that number has the voicemail service
        /// enabled.
        /// </summary>
        public bool DestinationVoicemailEnabled { get; set; }
    }
}