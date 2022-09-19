namespace SparkDotNet.Models.PersonCallVoicemail
{
    public class Notifications : WebexObject
    {
        public bool Enabled { get; set; }

        public string Destination { get; set; }
    }
}