using System;

namespace SparkDotNet.Models
{
    /// <summary>
    /// Teams are groups of people with a set of rooms that are visible to all members of that team.
    /// This API is used to manage the teams themselves. Teams are created and deleted with this API.
    /// You can also update a team to change its name, for example.
    /// To manage people in a team see the Team Memberships API.
    /// To manage team rooms see the Rooms API.
    /// </summary>
    public class Team : WebexObject
    {
        /// <summary>
        /// A unique identifier for the team.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A user-friendly name for the team.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id of the person who created the Team.
        /// </summary>
        public string CreatorId { get; set; }

        /// <summary>
        /// The date and time the team was created.
        /// </summary>
        public DateTime Created { get; set; }
    }
}