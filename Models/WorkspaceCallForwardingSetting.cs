namespace SparkDotNet.Models
{
    /// <summary>
    /// Configure a Workspace's Call Forwarding Settings
    ///
    /// Three types of call forwarding are supported: Always – forwards all incoming calls to the
    /// destination you choose.
    ///
    /// - When busy – forwards all incoming calls to the destination you chose while the phone is in
    /// use or the Workspace is busy.
    /// - When no answer – forwarding only occurs when you are away or not answering your phone.
    /// - In addition, the Business Continuity feature will send calls to a destination of your
    /// choice if your phone is not connected to the network for any reason, such as power
    /// outage,¨failed Internet connection, or wiring problem
    ///
    /// This API requires a full or user administrator auth token with the spark-admin:people_write
    /// scope or a user auth token with
    /// - spark:people_write scope can be used by a Workspace to update their settings.
    /// </summary>
    public class WorkspaceCallForwardingSetting : WebexObject
    {
        private WorkspaceCallForwarding CallForwarding { get; set; }
    }
}