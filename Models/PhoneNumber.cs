using Newtonsoft.Json;

namespace SparkDotNet.Models
{
    /// <summary>
    /// Phone numbers for the person. Only settable for Webex Calling. Needs a Webex Calling license.
    /// </summary>
    public class PhoneNumber : WebexObject
    {
        /// <summary>
        /// The type of phone number.
        /// </summary>
        [JsonProperty("type")]
        public TypeOfPhoneNumber PhoneNumberType { get; set; }

        /// <summary>
        /// The phone number.
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// The type of phone number.
    /// Work, Mobile, Fax
    /// </summary>
    public enum TypeOfPhoneNumber
    {
        Work,
        Mobile,
        Fax,
        Work_Extension
    }
}