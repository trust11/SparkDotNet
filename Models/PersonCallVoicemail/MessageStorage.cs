using System.Runtime.Serialization;

namespace SparkDotNet.Models.PersonCallVoicemail
{
    public enum StorageTypes
    {
        [EnumMember(Value = "INTERNAL")]
        INTERNAL,
        [EnumMember(Value = "EXTERNAL")]
        EXTERNAL
    }

    public class MessageStorage : WebexObject
    {
        public bool MwiEnabled { get; set; }

        public StorageTypes StorageType { get; set; }
        public string ExternalEmail { get; set; }
    }
}