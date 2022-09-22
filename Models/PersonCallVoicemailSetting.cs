namespace SparkDotNet.Models
{
    using SparkDotNet.Models.PersonCallVoicemail;

    public class PersonCallVoicemailSetting : WebexObject
    {
        public EmailCopyOfMessage EmailCopyOfMessage { get; set; } = new EmailCopyOfMessage();

        public bool? Enabled { get; set; } = true;

        public FaxMessago FaxMessage { get; set; } = new FaxMessago();

        public MessageStorage MessageStorage { get; set; } = new MessageStorage();

        public Notifications Notifications { get; set; } = new Notifications();

        public SendAllCalls SendAllCalls { get; set; } = new SendAllCalls();

        public SendBusyCalls SendBusyCalls { get; set; } = new SendBusyCalls();

        public SendUnansweredCalls SendUnansweredCalls { get; set; } = new SendUnansweredCalls();

        public TransferToNumber TransferToNumber { get; set; } = new TransferToNumber();

        public bool? VoiceMessageForwardingEnabled { get; set; }
    }
}