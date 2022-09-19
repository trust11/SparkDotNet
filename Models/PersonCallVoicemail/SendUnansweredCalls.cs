using System.ComponentModel.DataAnnotations;

namespace SparkDotNet.Models.PersonCallVoicemail
{
    public class SendUnansweredCalls : WebexObject
    {
        public bool Enabled { get; set; }

        public string Greeting { get; set; }

        public bool GreetingUploaded { get; set; }

        [Range(2, 20)]
        public int NumberOfRings { get; set; } = 2;

        public int SystemMaxNumberOfRings { get; set; }
    }
}