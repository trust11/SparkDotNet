using System.Runtime.Serialization;

namespace SparkDotNet.Models
{
    public class PersonExecutive
    {
        public ExecutiveType? Type { get; set; }
    }

    public enum ExecutiveType
    {
        [EnumMember(Value = "UNASSIGNED")]
        UNASSIGNED,

        [EnumMember(Value = "EXECUTIVE")]
        EXECUTIVE,

        [EnumMember(Value = "EXECUTIVE_ASSISTANT")]
        EXECUTIVE_ASSISTANT
    }
}