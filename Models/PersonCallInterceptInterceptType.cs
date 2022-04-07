namespace SparkDotNet
{
    /// <summary>
    /// INTERCEPT_TYPE how in- and outgoing calls are intercepted.
    /// </summary>
    public enum PersonCallInterceptInterceptType
    {
        /// <summary>
        /// Incoming calls are routed as destination and voicemail specify.
        /// </summary>
        INTERCEPT_ALL,

        /// <summary>
        /// Incoming calls are not intercepted.
        /// </summary>
        ALLOW_ALL
    }
}