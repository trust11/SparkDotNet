namespace SparkDotNet
{
    /// <summary>
    /// Information about the new number announcement.
    /// </summary>
    public class PersonCallInterceptNewNumber : WebexObject
    {
        /// <summary>
        /// New number caller will hear announced.
        /// </summary>
        public string Destination { get; set; }

        /// <summary>
        /// If true, the caller will hear this new number when a call is intercepted.
        /// </summary>
        public bool Enabled { get; set; }
    }
}