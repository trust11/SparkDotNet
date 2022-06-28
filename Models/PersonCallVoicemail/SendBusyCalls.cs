namespace SparkDotNet.Models.PersonCallVoicemail
{
    public class SendBusyCalls : WebexObject
    {
        public bool Enabled { get; set; }

        public string Greeting { get; set; }

        public bool GreetingUploaded { get; set; }
    }
}