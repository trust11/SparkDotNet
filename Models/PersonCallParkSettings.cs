using System.Collections.Generic;

namespace SparkDotNet.Models;

public class PersonCallParkSettings : WebexObject
{
    /// <summary>
    /// Enable or disable call park notification.
    /// </summary>
    public bool EnableCallParkNotification { get; set; }

    /// <summary>
    /// Identifiers of monitored elements whose monitoring settings will be modified.
    /// </summary>
    public List<string> MonitoredElements { get; set; }
}

public class PersonMonitoringSettings : WebexObject
{
    /// <summary>
    /// Call park notification is enabled or disabled.
    /// </summary>
    public bool CallParkNotificationEnabled { get; set; }

    /// <summary>
    /// Settings of monitored elements which can be person, place, virtual line or call park extension.
    /// </summary>
    public List<GetMonitoredElementsObject> MonitoredElements { get; set; }
}

public class GetMonitoredElementsObject
{
    public MonitoredElementObjectExt Member { get; set; }
}

public class MonitoredElementsObject
{
    public MonitoredElementObjectExt Member { get; set; }

    public CallParkExtension MonitoredElements { get; set; }
}

public class MonitoredElementObjectExt : MonitoredElementObject
{
    /// <summary>
    /// The location name where the call park extension is.
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// The ID for the location.
    /// </summary>
    public string LocationId { get; set; }
}

public class CallParkExtension : WebexObject
{
    /// <summary>
    /// The extension number for the call park extension.
    /// </summary>
    public string Extension { get; set; }

    /// <summary>
    /// The identifier of the call park extension.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// The location name where the call park extension is.
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// The ID for the location.
    /// </summary>
    public string LocationId { get; set; }

    /// <summary>
    /// The name used to describe the call park extension.
    /// </summary>
    public string Name { get; set; }
}