namespace SparkDotNet
{
    public class PersonCallVoicemailSetting : WebexObject
    {
        /// <summary>
        /// true if the Do Not Disturb feature is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Enables a Ring Reminder to play a brief tone on your desktop phone when you receive incoming calls.
        /// </summary>
        public bool RingSplashEnabled { get; set; }
    }
}