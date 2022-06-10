using System.Runtime.Serialization;

namespace SparkDotNet.Models
{
    public enum PersonCallInterceptInterceptGreeting
    {
        /// <summary>
        /// A custom will be placed when incoming calls are intercepted.
        /// </summary>
        [EnumMember(Value = "CUSTOM")]
        CUSTOM,

        /// <summary>
        /// A System default message will be placed when incoming calls are intercepted.
        /// </summary>
        [EnumMember(Value = "DEFAULT")]
        DEFAULT
    }
}