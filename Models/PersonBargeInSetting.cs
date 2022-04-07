namespace SparkDotNet
{
    public class PersonBargeInSetting : WebexObject
    {
        /// <summary>
        /// indicates if the Barge In feature is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Indicates that a stutter dial tone will be played when a person is barging in on the
        /// active call.
        /// </summary>
        public bool ToneEnabled { get; set; }
    }
}