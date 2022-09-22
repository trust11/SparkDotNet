using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SparkDotNet.Models
{
    public class PersonCallerIdUpdateSetting : WebexObject
    {
        //Which type of outgoing Caller ID will be used.
        public CallerIdSelectedType? Selected { get; set; }

        //This value must be an assigned number from the person's location.
        public string CustomNumber { get; set; }

        //Person's Caller ID first name. Characters of %, +, ``, " and Unicode characters are not allowed
        public string FirstName { get; set; }

        //Person's Caller ID last name. Characters of %, +, ``, " and Unicode characters are not allowed.
        public string LastName { get; set; }

        //Designates which type of External Caller Id Name policy is used. Default is DIRECT_LINE.
        public ExternalCallerIdNamePolicy ExternalCallerIdNamePolicy { get; set; }

        //Custom External Caller Name, which will be shown if External Caller Id Name is OTHER.
        public string CustomExternalCallerIdName { get; set; }

        //Block this user's identity when receiving a transferred or forwarded call.
        public bool? BlockInForwardCallsEnabled { get; set; }
    }

    public class PersonCallerIdSetting : PersonCallerIdUpdateSetting
    {
        //Allowed types for the selected field.
        public List<CallerIdSelectedType> Types { get; set; }

        //Direct number which will be shown if DIRECT_LINE is selected.
        public string DirectNumber { get; set; }

        //Extension number which will be shown if DIRECT_LINE is selected.
        public string ExtensionNumber { get; set; }

        //Location number which will be shown if LOCATION_NUMBER is selected.
        public string LocationNumber { get; set; }

        //Mobile number which will be shown if MOBILE_NUMBER is selected
        public string MobileNumber { get; set; }

        public bool? TollFreeLocationNumber { get; set; }

        //Information about the custom caller ID number.
        public CustomerInfo CustomerInfo { get; set; }

        //Block this user's identity when receiving a transferred or forwarded call.
        // public bool BlockInForwardCallsEnabled { get; set; }

        public string LocationExternalCallerIdName { get; set; }
    }

    public class CustomerInfo : WebexObject
    {
        //First name of custom caller ID number.
        public string FirstName { get; set; }

        //Last name of custom caller ID number.
        public string LastName { get; set; }

        //EXTERNAL if the custom caller ID number is external, otherwise INTERNAL.
        public TypeOfCallerIdNumber? Type { get; set; }
    }

    public enum TypeOfCallerIdNumber
    {
        [EnumMember(Value = "INTERNAL")]
        INTERNAL,

        [EnumMember(Value = "EXTERNAL")]
        EXTERNAL
    }

    public enum CallerIdSelectedType
    {
        //Outgoing caller ID will show the caller's direct line number and/or extension.
        [EnumMember(Value = "DIRECT_LINE")]
        DIRECT_LINE,

        //Outgoing caller ID will show the main number for the location.
        [EnumMember(Value = "LOCATION_NUMBER")]
        LOCATION_NUMBER,

        //Outgoing caller ID will show the mobile number for this person.
        [EnumMember(Value = "MOBILE_NUMBER")]
        MOBILE_NUMBER,

        //Outgoing caller ID will show the value from the customNumber field.
        [EnumMember(Value = "CUSTOM")]
        CUSTOM
    }

    //Designates which type of External Caller Id Name policy is used.Default is DIRECT_LINE.
    public enum ExternalCallerIdNamePolicy
    {
        //Outgoing caller ID will show the caller's direct line name.
        [EnumMember(Value = "DIRECT_LINE")]
        DIRECT_LINE,

        //Outgoing caller ID will show the Site Name for the location.
        [EnumMember(Value = "LOCATION")]
        LOCATION,

        //Outgoing caller ID will show the value from the customExternalCallerIdName field.
        [EnumMember(Value = "OTHER")]
        OTHER
    }
}