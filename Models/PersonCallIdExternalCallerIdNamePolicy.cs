namespace SparkDotNet
{
    /// <summary>
    /// Designates which type of External Caller Id Name policy is used. Default is DIRECT_LINE.
    /// </summary>
    public enum PersonCallIdExternalCallerIdNamePolicy
    {
        /// <summary>
        /// Outgoing caller ID will show the caller's direct line name.
        /// </summary>
        DIRECT_LINE,

        /// <summary>
        /// Outgoing caller ID will show the Site Name for the location.
        /// </summary>
        LOCATION,

        /// <summary>
        /// Outgoing caller ID will show the value from the customExternalCallerIdName field.
        /// </summary>
        OTHER
    }
}