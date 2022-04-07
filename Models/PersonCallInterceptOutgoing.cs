namespace SparkDotNet
{
    /// <summary>
    /// Settings related to how outgoing calls are handled when the intercept feature is enabled.
    /// </summary>
    public class PersonCallInterceptOutgoing : WebexObject
    {
        /// <summary>
        /// Number to which the outbound call be transferred.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// If true, when the person attempts to make an outbound call, a system default message is
        /// played and the call is made to the destination phone number
        /// </summary>
        public bool TransferEnabled { get; set; }

        /// <summary>
        /// INTERCEPT_TYPE how in- and outgoing calls are intercepted.
        /// </summary>
        public PersonCallInterceptInterceptType Type { get; set; }
    }
}