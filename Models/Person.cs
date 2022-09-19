using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SparkDotNet.Models
{
    /// <summary>
    /// People are registered users of Webex Teams.
    /// Searching and viewing People requires an auth token with a scope of spark:people_read.
    /// Viewing the list of all People in your Organization requires an administrator auth token with spark-admin:people_read scope.
    /// Adding, updating, and removing People requires an administrator auth token with the spark-admin:people_write scope.
    ///
    /// To learn more about managing people in a room see the Memberships API.
    /// For information about how to allocate Hybrid Services licenses to people, see the Managing Hybrid Services guide.
    /// https://developer.webex.com/docs/api/v1/people/list-people
    /// </summary>
    public class Person : WebexObject
    {
        /// <summary>
        /// The URL to the person's avatar in PNG format.
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// The date and time the person was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// The full name of the person.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The email addresses of the person.
        /// </summary>
        public HashSet<string> Emails { get; set; } = new HashSet<string>();

        /// <summary>
        /// The extension of the person retrieved from BroadCloud.
        /// </summary>
        [JsonProperty("extension")]
        public string Extension { get; set; }

        /// <summary>
        /// The first name of the person.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// A unique identifier for the person.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Whether or not an invite is pending for the user to complete account activation.
        /// This property is only returned if the authenticated user is an admin user for the person's organization.
        /// true: the person has been invited to Webex Teams but has not created an account
        /// false: an invite is not pending for this person
        /// </summary>
        [JsonProperty("invitePending")]
        public bool InvitePending { get; set; }

        /// <summary>
        /// The date and time of the person's last activity within Webex Teams.
        /// </summary>
        public DateTime LastActivity { get; set; }

        /// <summary>
        /// The date and time the person was last changed.
        /// </summary>
        [JsonProperty("lastModified")]
        public DateTime LastModified { get; set; }

        /// <summary>
        /// The last name of the person.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// An array of license strings allocated to this person.
        /// </summary>
        public HashSet<string> Licenses { get; set; } = new HashSet<string>();

        /// <summary>
        /// The business department the user belongs to.
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// A manager identifier
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// Person Id of the manager
        /// </summary>
        public string ManagerId { get; set; }

        /// <summary>
        /// The person's title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Person's address
        /// </summary>
        public HashSet<PersonAddress> Addresses { get; set; }

        /// <summary>
        /// The ID of the location for this person retrieved from BroadCloud.
        /// </summary>
        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        /// <summary>
        /// Whether or not the user is allowed to use Webex Teams.
        /// This property is only returned if the authenticated user is an admin user for the person's organization.
        /// true: the person can log into Webex Teams
        /// false: the person cannot log into Webex Teams
        /// </summary>
        [JsonProperty("loginEndbaled")]
        public bool LoginEnabled { get; set; }

        /// <summary>
        /// The nickname of the person if configured. If no nickname is configured for the person, this field will not be present.
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// The ID of the organization to which this person belongs.
        /// </summary>
        public string OrgId { get; set; }

        /// <summary>
        /// Phone numbers for the person.
        /// </summary>
        [JsonProperty("phoneNumbers")]
        public HashSet<PhoneNumber> PhoneNumbers { get; set; } = new HashSet<PhoneNumber>();

        /// <summary>
        /// An array of role strings representing the roles to which this person belongs.
        /// </summary>
        public HashSet<string> Roles { get; set; } = new HashSet<string>();

        /// <summary>
        /// Sip addresses for the person.
        /// </summary>
        [JsonProperty("sipAddresses")]
        public HashSet<SipAddress> SipAddresses { get; set; } = new HashSet<SipAddress>();

        /// <summary>
        /// One or several site names where this user has attendee role.
        /// Append #attendee to the sitename (eg: mysite.webex.com#attendee)
        /// Possible values: mysite.webex.com#attendee
        /// </summary>
        public HashSet<string> SiteUrls { get; set; }

        /// <summary>
        /// The current presence status of the person.
        /// active: active within the last 10 minutes
        /// call: the user is in a call
        /// DoNotDisturb: the user has manually set their status to "Do Not Disturb"
        /// inactive: last activity occurred more than 10 minutes ago
        /// meeting: the user is in a meeting
        /// OutOfOffice: the user or a Hybrid Calendar service has indicated that they are "Out of Office"
        /// pending: the user has never logged in; a status cannot be determined
        /// presenting: the user is sharing content
        /// unknown: the user’s status could not be determined
        /// </summary>
        public StatusType Status { get; set; }

        /// <summary>
        /// The time zone of the person if configured. If no timezone is configured on the account, this field will not be present
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// The type of person account, such as person or bot.
        /// person: account belongs to a person
        /// bot: account is a bot user
        /// appuser: account is a guest user
        /// </summary>
        public TypeType Type { get; set; }
    }

    public class PersonAddress
    {
        /// <summary>
        /// Possible values: US
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Possible values: Charlotte
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// Possible values: North Carolina
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Possible values: 1099 Bird Ave.
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Possible values: work
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Possible values: 99212
        /// </summary>
        public string PostalCode { get; set; }
    }

    /// <summary>
    /// The type of person account, such as person or bot.
    /// </summary>
    public enum TypeType
    {
        //account belongs to a person
        [EnumMember(Value = "person")]
        Person,

        //account is a bot user
        [EnumMember(Value = "bot")]
        Bot,

        //account is a guest user -> https://developer.webex.com/docs/guest-issuer
        [EnumMember(Value = "appuser")]
        Appuser
    }

    /// <summary>
    /// The current presence status of the person.This will only be returned for people within your organization or an organization you manage.Presence information will not be shown if the authenticated user has disabled status sharing.
    /// </summary>
    public enum StatusType
    {
        //Active within the last 10 minutes
        Call,

        //the user is in a call
        Active,

        //the user has manually set their status to "Do Not Disturb"
        DoNotDisturb,

        //last activity occurred more than 10 minutes ago
        Inactive,

        //the user is in a meeting
        Meeting,

        //the user or a Hybrid Calendar service has indicated that they are "Out of Office"
        OutOfOffice,

        //the user has never logged in; a status cannot be determined
        Pending,

        //the user is sharing content
        Presenting,

        //the user’s status could not be determined
        Unknown,
    }
}