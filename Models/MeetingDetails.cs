namespace SparkDotNet.Models
{
    public class MeetingDetails : WebexObject
    {
        /// <summary>
        /// A unique identifier for the room.
        /// </summary>
        public string RoomId { get; set; }

        /// <summary>
        /// The Webex meeting URL for the room.
        /// </summary>
        public string MeetingLink { get; set; }

        /// <summary>
        /// The SIP address for the room.
        /// </summary>
        public string SipAddress { get; set; }

        /// <summary>
        /// The Webex meeting number for the room.
        /// </summary>
        public string MeetingNumber { get; set; }

        /// <summary>
        /// The toll-free PSTN number for the room.
        /// </summary>
        public string CallInTollFreeNumber { get; set; }

        /// <summary>
        /// The toll (local) PSTN number for the room.
        /// </summary>
        public string CallInTollNumber { get; set; }
    }
}