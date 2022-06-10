namespace SparkDotNet.Models
{
    public class PersonCallInterceptSetting : WebexObject
    {
        /// <summary>
        /// true if call intercept is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Settings related to how incoming calls are handled when the intercept feature is
        /// enabled.
        /// </summary>
        public PersonCallInterceptIncoming Incoming { get; set; }

        /// <summary>
        /// Settings related to how outgoing calls are handled when the intercept feature is
        /// enabled.
        /// </summary>
        public PersonCallInterceptOutgoing Outgoing { get; set; }
    }
}