#nullable enable
using SparkDotNet;

namespace SparkDotNet.Models
{
    public class PersonUcProfileSetting : WebexObject
    {
        /// <summary>
        /// A unique identifier for the person uc profile setting.
        /// </summary>
        public string? ProfileId { get; set; }
    }
}