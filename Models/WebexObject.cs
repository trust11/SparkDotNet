using Newtonsoft.Json;
using System.Diagnostics;

namespace SparkDotNet.Models
{
    /// <summary>
    /// This is a common superclass for all Webex API objects.
    /// It will bundle their common behavior
    /// </summary>
    [DebuggerDisplay("{" + nameof(ToStringFormatted) + "(),nq}")]
    public abstract class WebexObject
    {
        public enum JsonFormatting
        {
            None = Formatting.None,
            Indented = Formatting.Indented
        }

        /// <summary>
        /// Returns the JSON representation of the object
        /// </summary>
        /// <returns></returns>
        private string ToString(JsonFormatting format = JsonFormatting.None)
        {
            return JsonConvert.SerializeObject(this, (Formatting)format);
        }

        public override string ToString()
        {
            return ToString();
        }

        public string ToStringFormatted()
        {
            return ToString(JsonFormatting.Indented);
        }
    }
}