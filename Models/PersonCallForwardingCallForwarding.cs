namespace SparkDotNet.Models
{
    /// <summary>
    /// Settings related to "Always", "Busy", and "No Answer" call forwarding.
    /// </summary>
    public class PersonCallForwardingCallForwarding : WebexObject
    {
        public PersonCallForwardingAlways Always { get; set; }
        public PersonCallForwardingBusy Busy { get; set; }
        public PersonCallForwardingNoAnswer NoAnswer { get; set; }
    }
}