using System.Runtime.Serialization;

namespace SparkDotNet.Models
{
    public enum ExternalTransfer
    {
        [EnumMember(Value = "ALLOW_ALL_EXTERNAL")] ALLOW_ALL_EXTERNAL,
        [EnumMember(Value = "ALLOW_ONLY_TRANSFERRED_EXTERNAL")] ALLOW_ONLY_TRANSFERRED_EXTERNAL,
        [EnumMember(Value = "BLOCK_ALL_EXTERNAL")] BLOCK_ALL_EXTERNAL
    }

    public class PersonIncomingPermissionSettings : WebexObject
    {
        public bool CollectCallsEnabled { get; set; }

        public ExternalTransfer ExternalTransfer { get; set; }

        public bool InternalCallsEnabled { get; set; }

        public bool UseCustomEnabled { get; set; }
    }
}