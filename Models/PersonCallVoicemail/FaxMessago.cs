namespace SparkDotNet.Models.PersonCallVoicemail
{
    public class FaxMessago : WebexObject
    {
        public bool Enabled { get; set; }

        public string PhoneNumber { get; set; }

        public string Extension { get; set; }
    }
}