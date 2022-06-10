using System.Runtime.Serialization;

namespace SparkDotNet.Models
{
    /// <summary>
    /// Designates which type of External Caller Id Name policy is used. Default is DIRECT_LINE.
    /// </summary>
    public enum PersonCallIdExternalCallerIdNamePolicy
    {
        /// <summary>
        /// Outgoing caller ID will show the caller's direct line name.
        /// </summary>
        [EnumMember(Value = "DIRECT_LINE")]
        DIRECT_LINE,

        /// <summary>
        /// Outgoing caller ID will show the Site Name for the location.
        /// </summary>
        [EnumMember(Value = "LOCATION")]
        LOCATION,

        /// <summary>
        /// Outgoing caller ID will show the value from the customExternalCallerIdName field.
        /// </summary>
        [EnumMember(Value = "OTHER")]
        OTHER
    }
}