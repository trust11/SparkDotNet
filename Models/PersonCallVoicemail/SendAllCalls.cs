namespace SparkDotNet.Models.PersonCallVoicemail
{
    public class SendAllCalls : WebexObject, IVoicemailForwarding
    {
        public bool Enabled { get; set; }
    }
}