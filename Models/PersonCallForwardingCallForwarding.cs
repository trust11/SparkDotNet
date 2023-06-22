namespace SparkDotNet.Models
{
    /// <summary>
    /// Settings related to "Always", "Busy", and "No Answer" call forwarding.
    /// </summary>
    public class PersonCallForwardingCallForwarding : WebexObject
    {
        public PersonCallForwardingAlways Always { get; set; } = new PersonCallForwardingAlways();

        public PersonCallForwardingBusy Busy { get; set; } = new PersonCallForwardingBusy();

        public PersonCallForwardingNoAnswer NoAnswer { get; set; } = new PersonCallForwardingNoAnswer();
    }

    public interface IPersonCallForwarding
    {
        bool Enabled { get; set; }

        bool DestinationVoicemailEnabled { get; set; }

        string Destination { get; set; }
    }
}