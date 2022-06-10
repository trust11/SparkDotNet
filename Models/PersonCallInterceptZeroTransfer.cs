namespace SparkDotNet.Models
{
    /// <summary>
    /// Information about how the call will be handled if zero (0) is pressed.
    /// </summary>
    public class PersonCallInterceptZeroTransfer : WebexObject
    {
        /// <summary>
        /// Destination to which caller will be transferred when zero is pressed.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// If true, the caller will be transferred to destination of when zero (0) is pressed.
        /// </summary>
        public bool Enabled { get; set; }
    }
}