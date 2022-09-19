using System;

namespace SparkDotNet.Models
{
    /// <summary>
    /// eam Memberships represent a person's relationship to a team. Use this API to list members of any team that you're in
    /// or create memberships to invite someone to a team. Team memberships can also be updated to make someone a moderator
    /// or deleted to remove them from the team.
    /// Just like in the Webex Teams app, you must be a member of the team in order to list its memberships or invite people.
    /// </summary>
    public class TeamMembership : WebexObject
    {
        /// <summary>
        /// A unique identifier for the team membership.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The team ID.
        /// </summary>
        public string TeamId { get; set; }

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
        /// Whether or not the participant is a team moderator.
        /// </summary>
        public bool IsModerator { get; set; }

        /// <summary>
        /// The date and time when the team membership was created.
        /// </summary>
        public DateTime Created { get; set; }
    }
}