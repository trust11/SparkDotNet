namespace SparkDotNet.Models
{
    public class PersonCallInterceptAnnouncements : WebexObject
    {
        /// <summary>
        /// Filename of custom greeting, will be an empty string if no custom greeting has been
        /// uploaded.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// DEFAULT indicates that a system default message will be placed when incoming calls are
        /// intercepted.
        /// </summary>
        public PersonCallInterceptInterceptGreeting Greeting { get; set; }

        /// <summary>
        /// Information about the new number announcement.
        /// </summary>
        public PersonCallInterceptNewNumber NewNumber { get; set; }

        /// <summary>
        /// Information about how the call will be handled if zero (0) is pressed.
        /// </summary>
        public PersonCallInterceptZeroTransfer ZeroTransfer { get; set; }
    }
}