using System.Collections.Generic;

namespace SparkDotNet
{
    public class PersonCallIdSetting
    {
        public List<string> Types { get; set; }

        /// <summary>
        /// Which type of outgoing Caller ID will be used. Possible values: DIRECT_LINE
        /// </summary>
        public string Selected { get; set; }

        /// <summary>
        /// Direct number which will be shown if DIRECT_LINE is selected.
        /// </summary>
        public string DirectNumber { get; set; }

        /// <summary>
        /// Extension number which will be shown if DIRECT_LINE is selected
        /// </summary>
        public string ExtensionNumber { get; set; }

        /// <summary>
        /// Location number which will be shown if LOCATION_NUMBER is selected.
        /// </summary>
        public string LocationNumber { get; set; }

        /// <summary>
        /// No information form Webex Docs.
        /// https://developer.webex.com/docs/api/v1/webex-calling-person-settings/read-caller-id-settings-for-a-person
        /// </summary>
        public bool TollFreeLocationNumber { get; set; }

        /// <summary>
        /// Person's Caller ID first name. Characters of %, +, ``, " and Unicode characters are not
        /// allowed.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Person's Caller ID last name. Characters of %, +, ``, " and Unicode characters are not
        /// allowed.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// No information form Webex Docs.
        /// https://developer.webex.com/docs/api/v1/webex-calling-person-settings/read-caller-id-settings-for-a-person
        /// </summary>
        public bool BlockInForwardCallsEnabled { get; set; }

        /// <summary>
        /// Designates which type of External Caller Id Name policy is used. Default is DIRECT_LINE.
        /// </summary>
        public PersonCallIdExternalCallerIdNamePolicy ExternalCallerIdNamePolicy { get; set; }

        /// <summary>
        /// Person's custom External Caller ID last name. Characters of %, +, ``, " and Unicode
        /// characters are not allowed.
        /// </summary>
        public string LocationExternalCallerIdName { get; set; }

        /// <summary>
        /// Mobile number which will be shown if MOBILE_NUMBER is selected.
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// Person's custom External Caller ID last name. Characters of %, +, ``, " and Unicode
        /// characters are not allowed.
        /// </summary>
        public string CustomExternalCallerIdName { get; set; }

        /// <summary>
        /// This value must be an assigned number from the person's location.
        /// </summary>
        public string CustomNumber { get; set; }

        /// <summary>
        /// Information about the custom caller ID number.
        /// </summary>
        public string CustomNumberInfo { get; set; }
    }
}