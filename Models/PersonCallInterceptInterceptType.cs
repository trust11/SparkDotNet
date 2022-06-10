using System.Runtime.Serialization;

namespace SparkDotNet.Models
{
    /// <summary>
    /// INTERCEPT_TYPE how in- and outgoing calls are intercepted.
    /// </summary>
    public enum PersonCallInterceptInterceptType
    {
        /// <summary>
        /// Incoming calls are routed as destination and voicemail specify.
        /// </summary>
        [EnumMember(Value = "INTERCEPT_ALL")]
        INTERCEPT_ALL,

        /// <summary>
        /// Incoming calls are not intercepted.
        /// </summary>
        [EnumMember(Value = "ALLOW_ALL")]
        ALLOW_ALL
    }
}