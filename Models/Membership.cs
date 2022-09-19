using System;

namespace SparkDotNet.Models
{
    /// <summary>
    /// Memberships represent a person's relationship to a room. Use this API to list members of any room that you're in or
    /// create memberships to invite someone to a room. Memberships can also be updated to make someone a moderator or
    /// deleted to remove them from the room.
    /// Just like in the Webex Teams app, you must be a member of the room in order to list its memberships or invite people.
    /// </summary>
    public class Membership : WebexObject
    {
        /// <summary>
        /// A unique identifier for the membership.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The room ID.
        /// </summary>
        public string RoomId { get; set; }

        /// <summary>
        /// The type of room the membership is associated with.
        /// direct: 1:1 room
        /// group: group room
        /// </summary>
        public string RoomType { get; set; }

        /// <summary>
        /// The person ID.
        /// </summary>
        public string PersonId { get; set; }

        /// <summary>
        /// The email address of the person.
        /// </summary>
        public string PersonEmail { get; set; }

        /// <summary>
        /// The display name of the person.
        /// </summary>
        public string PersonDisplayName { get; set; }

        /// <summary>
        /// The organization ID of the person.
        /// </summary>
        public string PersonOrgId { get; set; }

        /// <summary>
        /// Whether or not the participant is a room moderator.
        /// </summary>
        public bool IsModerator { get; set; }

        /// <summary>
        /// Whether or not the room is hidden in the Webex Teams clients.
        /// </summary>
        public bool IsRoomHidden { get; set; }

        /// <summary>
        /// The date and time when the membership was created.
        /// </summary>
        public DateTime Created { get; set; }
    }
}