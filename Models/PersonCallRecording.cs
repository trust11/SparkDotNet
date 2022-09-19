namespace SparkDotNet.Models
{
    /// <summary>
    /// Configure a Person's Call Recording Settings
    /// The Call Recording feature provides a hosted mechanism to record the calls placed and received on
    /// the Carrier platform for replay and archival.This feature is helpful for quality assurance, security,
    /// training, and more.
    /// This API requires a full or user administrator auth token with the spark-admin:people_write scope.
    /// </summary>
    public class PersonCallRecording : WebexObject
    {
        public bool Enabled { get; set; } = false;

        public Record Record { get; set; } = SparkDotNet.Record.Never;

        public bool RecordVoicemailEnabled { get; set; } = false;

        public bool StartStopAnnouncementEnabled { get; set; } = false;

        public Notification Notification { get; set; } = new Notification();

        public Repeat Repeat { get; set; } = new Repeat();

        public string ServiceProvider { get; set; }

        public string ExternalGroup { get; set; }

        public string ExternalIdentifier { get; set; }
    }
}