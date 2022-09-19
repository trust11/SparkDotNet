namespace SparkDotNet.Models
{
    /// <summary>
    /// Settings related to "Always", "Busy", and "No Answer" call forwarding.
    /// </summary>
    public class WorkspaceCallForwarding : WebexObject
    {
        public WorkspaceCallForwardingBusy Busy { get; set; }

        public WorkspaceCallForwardingNoAnswer NoAnswer { get; set; }
    }
}