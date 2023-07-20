using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SparkDotNet.Models;

[JsonConverter(typeof(StringEnumConverter))]
public enum LineType
{
    ///<summary>
    // Primary line for the member.
    ///</summary>
    PRIMARY,

    ///<summary>
    // Shared line for the member. A shared line allows users to receive and place calls to and from another user's extension, using their own device.
    ///</summary>
    SHARED_CALL_APPEARANCE
}

[JsonConverter(typeof(StringEnumConverter))]
public enum UserType
{
    ///<summary>
    // The associated member is a person.
    ///</summary>
    PEOPLE,

    ///<summary>
    // The associated member is a workspace.
    ///</summary>
    PLACE
}

public class SearchLocationMemeber : WebexObject
{
    /// <summary>
    /// Location identifier associated with the members.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Location name associated with the member.
    /// </summary>
    public string Name { get; set; }
}

public class SearchPersonLineMembersPayload : WebexObject
{
    /// <summary>
    /// Search for users with extensions that match the query.
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// Location ID for the user.
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// Number of records per page.
    /// </summary>
    public int Max { get; set; }

    /// <summary>
    /// Search for users with names that match the query.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Search for users with numbers that match the query.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Sort by first name (fname) or last name (lname).
    /// </summary>
    public string Order { get; set; }

    /// <summary>
    /// Page number.
    /// </summary>
    public int Start { get; set; }
}

//public class SearchPersonLineMembersResult : WebexObject
//{
//    /// <summary>
//    /// Phone extension of member.
//    /// </summary>
//    public string Extension { get; set; }

//    /// <summary>
//    /// First name of member.
//    /// </summary>
//    public string FirstName { get; set; }

//    /// <summary>
//    /// A unique member identifier.
//    /// </summary>
//    public string Id { get; set; }

//    /// <summary>
//    /// Last name of member.
//    /// </summary>
//    public string LastName { get; set; }

//    /// <summary>
//    /// Indicates if the line is acting as a primary line or a shared line for this device.
//    /// </summary>
//    public LineType LineType { get; set; }

//    /// <summary>
//    /// Location object having a unique identifier for the location and its name.
//    /// </summary>
//    public SearchLocationMemeber Location { get; set; }

//    /// <summary>
//    /// Phone number of member.Currently, E.164 format is not supported.
//    /// </summary>
//    public string PhoneNumber { get; set; }
//}

public class AvailableSharedLineMemberItem : PutSharedLineMemberItem
{
    ///<summary>
    // Set how a device behaves when a call is declined. When set to true, a call decline request is extended to all the endpoints on the device. When set to false, a call decline request is only declined at the current endpoint.
    ///</summary>
    //public bool AllowCallDeclineEnabled { get; set; }

    /// <summary>
    /// Phone extension of a person or workspace.
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// First name of person or workspace.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Registration home IP for the line port.
    /// </summary>
    public string HostIP { get; set; }

    ///<summary>
    // Preconfigured number for the hotline. Required only if hotlineEnabled is set to true.
    ///</summary>
    //public string HotlineDestination { get; set; }

    ///<summary>
    /// Configure this line to automatically call a predefined number whenever taken off-hook. Once enabled, the line can only make calls to the predefined number set in hotlineDestination.
    ///</summary>
    //public bool HotlineEnabled { get; set; }

    /// <summary>
    /// Location identifier associated with the members.
    /// </summary>
    //public string Id { get; set; }

    /// <summary>
    /// Last name of person or workspace.
    /// </summary>
    public string LastName { get; set; }

    ///<summary>
    // Device line label.
    ///</summary>
    //public string LineLabel { get; set; }

    /// <summary>
    /// Indicates if the line is acting as a primary line or a shared line for this device.
    /// </summary>
    //public LineType LineType { get; set; }

    /// <summary>
    /// Number of lines that have been configured for the person on the device.
    /// </summary>
    //public int LineWeight { get; set; }

    ///<summary>
    // Location object having a unique identifier for the location and its name.
    ///</summary>
    public SearchLocationMemeber Location { get; set; }

    ///<summary>
    // Indicates if the member is of type PEOPLE or PLACE.
    ///</summary>
    public UserType MemberType { get; set; }

    /// <summary>
    /// Phone number of a person or workspace. Currently, E.164 format is not supported. This will be supported in the future update.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Device port number assigned to a person or workspace.
    /// </summary>
    //public int Port { get; set; }

    /// <summary>
    /// If true the person or the workspace is the owner of the device. Points to primary line/port of the device.
    /// </summary>
    //public bool PrimaryOwner { get; set; }

    /// <summary>
    /// Registration remote IP for the line port.
    ///</summary>
    public string RemoteIP { get; set; }

    /// <summary>
    /// T.38 Fax Compression setting. Valid only for ATA Devices. Overrides user level compression options.
    /// </summary>
    //public bool? T38FaxCompressionEnabled { get; set; }

    public PutSharedLineMemberItem GetPutSharedLineMemberItemInstance()
    {
        var ret = new PutSharedLineMemberItem();
        ret.HotlineEnabled = this.HotlineEnabled;
        ret.LineLabel = this.LineLabel;
        ret.PrimaryOwner = this.PrimaryOwner;
        ret.Port = this.Port;
        ret.AllowCallDeclineEnabled = this.AllowCallDeclineEnabled;
        ret.LineWeight = this.LineWeight;
        ret.Id = this.Id;
        ret.T38FaxCompressionEnabled = this.T38FaxCompressionEnabled;
        ret.HotlineDestination = this.HotlineDestination;
        ret.LineType = this.LineType;
        return ret;
    }
}

/// <summary>
/// Represents a search for users.
/// </summary>
public class AvailableSharedLineMemberItemsPayload
{
    /// <summary>
    /// Gets or sets the maximum number of records per page.
    /// </summary>
    public int Max { get; set; }

    /// <summary>
    /// Gets or sets the page number.
    /// </summary>
    public int Start { get; set; }

    /// <summary>
    /// Gets or sets the location ID for the user.
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// Gets or sets the name to search for users with names that match the query.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the number to search for users with numbers that match the query.
    /// </summary>
    public string Number { get; set; }

    /// <summary>
    /// Gets or sets the order to sort by first name (fname) or last name (lname).
    /// </summary>
    public string Order { get; set; }

    /// <summary>
    /// Gets or sets the extension to search for users with extensions that match the query.
    /// </summary>
    public string Extension { get; set; }
}

public class AvailableSharedLineMemberItems : WebexObject
{
    ///List of members.
    public List<AvailableSharedLineMemberItem> Members { get; set; }
}

public class SharedLineMembersGetResult : AvailableSharedLineMemberItems
{
    /// <summary>
    /// Model name of device.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Maximum number of device ports.
    /// </summary>

    private int MaxLineCount { get; set; }
}

public class PutSharedLineMemberItems : WebexObject
{
    ///List of members.
    public List<PutSharedLineMemberItem> Members { get; set; }
}

public class PutSharedLineMemberItem : WebexObject
{
    /// <summary>
    /// Gets or sets the unique identifier
    /// /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the device port number assigned to person or workspace.
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets the T.38 Fax Compression setting. Valid only for ATA Devices. Overrides user level compression options.
    /// </summary>
    public bool? T38FaxCompressionEnabled { get; set; }

    /// <summary>
    /// Gets or sets if the person or the workspace is the owner of the device. Points to primary line/port of the device.
    /// </summary>
    public bool PrimaryOwner { get; set; }

    /// <summary>
    /// Gets or sets if the line is acting as a primary line or a shared line for this device.
    /// </summary>
    public LineType LineType { get; set; }

    /// <summary>
    /// Gets or sets the number of lines, between 1 to 16, that have been configured for the person on the device.
    /// </summary>
    [Range(1, 16)]
    public int LineWeight { get; set; }

    /// <summary>
    /// Gets or sets if this line is configured to automatically call a predefined number whenever taken off-hook. Once enabled, the line can only make calls to the predefined number set in hotlineDestination.
    /// </summary>
    public bool HotlineEnabled { get; set; }

    /// <summary>
    /// Gets or sets the preconfigured number for the hotline. Required only if hotlineEnabled is set to true.
    /// </summary>
    public string HotlineDestination { get; set; }

    /// <summary>
    /// Gets or sets how a device behaves when a call is declined. When set to true, a call decline request is extended to all the endpoints on the device. When set to false, a call decline request is only declined at the current endpoint.
    /// </summary>
    public bool AllowCallDeclineEnabled { get; set; }

    /// <summary>
    /// Gets or sets the device line label.
    /// </summary>
    public string LineLabel { get; set; }
}