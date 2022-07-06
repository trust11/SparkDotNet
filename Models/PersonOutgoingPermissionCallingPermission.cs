using System.Runtime.Serialization;

namespace SparkDotNet.Models
{
    public class PersonOutgoingPermissionCallingPermission
    {
        public CallType CallType { get; set; }
        public Action Action { get; set; }
        public bool TransferEnabled { get; set; }
    }

    public enum CallType
    {
        [EnumMember(Value = "INTERNAL_CALL")] INTERNAL_CALL,
        [EnumMember(Value = "TOLL_FREE")] TOLL_FREE,
        [EnumMember(Value = "NATIONAL")] NATIONAL,
        [EnumMember(Value = "INTERNATIONAL")] INTERNATIONAL,
        [EnumMember(Value = "LOCAL")] LOCAL,
        [EnumMember(Value = "OPERATOR_ASSISTED")] OPERATOR_ASSISTED,
        [EnumMember(Value = "CHARGEABLE_DIRECTORY_ASSISTED")] CHARGEABLE_DIRECTORY_ASSISTED,
        [EnumMember(Value = "SPECIAL_SERVICES_I")] SPECIAL_SERVICES_I,
        [EnumMember(Value = "SPECIAL_SERVICES_II")] SPECIAL_SERVICES_II,
        [EnumMember(Value = "PREMIUM_SERVICES_I")] PREMIUM_SERVICES_I,
        [EnumMember(Value = "PREMIUM_SERVICES_II")] PREMIUM_SERVICES_II,

        [EnumMember(Value = "CASUAL")] CASUAL,
        [EnumMember(Value = "URL_DIALING")] URL_DIALING,
        [EnumMember(Value = "TOLL")] TOLL,
        [EnumMember(Value = "UNKNOWN")] UNKNOWN
    }
    public enum Action
    {
        [EnumMember(Value = "ALLOW")] ALLOW,
        [EnumMember(Value = "BLOCK")] BLOCK,
        [EnumMember(Value = "AUTH_CODE")] AUTH_CODE,
        [EnumMember(Value = "TRANSFER_NUMBER_1")] TRANSFER_NUMBER_1,
        [EnumMember(Value = "TRANSFER_NUMBER_2")] TRANSFER_NUMBER_2,
        [EnumMember(Value = "TRANSFER_NUMBER_3")] TRANSFER_NUMBER_3
    }
}