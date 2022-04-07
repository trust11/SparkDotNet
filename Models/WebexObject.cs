using Newtonsoft.Json;
using System.Diagnostics;

namespace SparkDotNet
{
    /// <summary>
    /// This is a common superclass for all Webex API objects.
    /// It will bundle their common behavior
    /// </summary>
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
        public string ToString(JsonFormatting format = JsonFormatting.None)
        {
            return JsonConvert.SerializeObject(this, (Formatting)format);
        }

        public override string ToString() { return this.ToString(); }

        private string GetDebuggerDisplay()
        {
            return ToString(JsonFormatting.Indented);
        }
    }
}
