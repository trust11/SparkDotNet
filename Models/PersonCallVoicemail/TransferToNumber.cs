namespace SparkDotNet.Models.PersonCallVoicemail
{
    public class TransferToNumber : WebexObject
    {
        public bool Enabled { get; set; }

        public string Destination { get; set; }
    }
}