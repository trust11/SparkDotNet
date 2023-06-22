using Newtonsoft.Json;

using System.Collections.Generic;

namespace SparkDotNet.Models;

public class MonitoredElementObject
{
    /// <summary>
    /// Display name of the person.
    /// </summary>
    public string DisplayName { get; set; }

    /// <summary>
    /// First name of the person.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Unique private identifier of the person.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// List of phone numbers of the person.
    /// </summary>
    public List<PushToTalkNumberObject> Numbers { get; set; }

    /// <summary>
    /// Type usually indicates PEOPLE, PLACE or VIRTUAL_LINE.Push-to-Talk and Privacy features only supports PEOPLE.
    /// </summary>
    public PeopleOrPlaceOrVirtualLineType Type { get; set; }

    /// <summary>
    /// Email private address of the person.
    /// </summary>
    private string Email { get; set; }

    /// <summary>
    /// Last name of the person.
    /// </summary>
    private string LastName { get; set; }
}

public enum PeopleOrPlaceOrVirtualLineType
{
    PEOPLE, PLACE, VIRTUAL_LINE
}
//    /// <summary>
//    /// Indicates a person or list of people.
//    /// </summary>
//    [JsonProperty("PEOPLE")]
//    public string People { get; set; }

//    /// <summary>
//    /// Indicates a workspace that is not assigned to a specific person such as for a shared device in a common area.
//    /// </summary>
//    [JsonProperty("PLACE")]
//    public string Place { get; set; }

//    /// <summary>
//    /// Indicates a private virtual line or private list of virtual lines.
//    /// </summary>
//    [JsonProperty("VIRTUAL_LINE")]
//    public string VirtualLine { get; set; }
//}

public class PersonPrivacySettings
{
    /// <summary>
    /// When true auto attendant extension dialing will be enabled.
    /// </summary>
    public bool AAExtensionDialingEnabled { get; set; }

    /// <summary>
    /// When true auto attendant dailing by first or last name will be enabled.
    /// </summary>
    public bool AANamingDialingEnabled { get; set; }

    /// <summary>
    /// When true phone status directory privacy will be enabled.
    /// </summary>
    public bool EnablePhoneStatusDirectoryPrivacy { get; set; }

    /// <summary>
    /// List of people that are being monitored
    /// </summary>
    public List<MonitoredElementObject> MonitoringAgents { get; set; }
}

public class PushToTalkNumberObject
{
    /// <summary>
    /// Extension number of the person.
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// External phone number of the person.
    /// </summary>
    public string External { get; set; }

    /// <summary>
    /// Indicates whether phone number is primary number.
    /// </summary>
    public bool Primary { get; set; }
}