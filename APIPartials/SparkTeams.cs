
using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{

    public partial class Spark
    {

        private readonly string teamsBase = "/v1/teams";

        /// <summary>
        /// Lists teams to which the authenticated user belongs.
        /// </summary>
        /// <param name="max">Limit the maximum number of teams in the response. Default: 100</param>
        /// <returns>List of Team objects.</returns>
        public async Task<SparkApiConnectorApiOperationResult<List<Team>>> GetTeamsAsync(int max = 0)
        {
            var queryParams = new Dictionary<string, string>();
            if (max > 0) queryParams.Add("max",max.ToString());
            var path = GetURL(teamsBase, queryParams);
            return await GetItemsAsync<Team>(path).ConfigureAwait(false);
        }

        /// <summary>
        /// Shows details for a team, by ID.
        /// Specify the room ID in the teamId parameter in the URI.
        /// </summary>
        /// <param name="teamId">The unique identifier for the team.</param>
        /// <returns>Team object.</returns>
        public async Task<SparkApiConnectorApiOperationResult<Team>> GetTeamAsync(string teamId)
        {
            var queryParams = new Dictionary<string, string>();
            var path = GetURL($"{teamsBase}/{teamId}", queryParams);
            return await GetItemAsync<Team>(path).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a team. The authenticated user is automatically added as a member of the team.
        /// See the Team Memberships API to learn how to add more people to the team.
        /// </summary>
        /// <param name="name">A user-friendly name for the team.</param>
        /// <returns>Team object.</returns>
        public async Task<SparkApiConnectorApiOperationResult<Team>> CreateTeamAsync(string name)
        {
            var postBody = new Dictionary<string, object>();
            postBody.Add("name", name);
            return await PostItemAsync<Team>(teamsBase, postBody).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a team, by ID.
        /// Specify the team ID in the teamId parameter in the URI.
        /// </summary>
        /// <param name="teamId">The unique identifier for the team.</param>
        /// <returns>Boolean indicating success of operation.</returns>
        public async Task<SparkApiConnectorApiOperationResult<bool>> DeleteTeamAsync(string teamId)
        {
            return await DeleteItemAsync($"{teamsBase}/{teamId}").ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a team, by object.
        /// </summary>
        /// <param name="team">The team object.</param>
        /// <returns>Boolean indicating success of operation.</returns>
        public async Task<SparkApiConnectorApiOperationResult<bool>> DeleteTeamAsync(Team team)
        {
            return await DeleteTeamAsync(team.Id).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates details for a team, by ID.
        /// Specify the team ID in the teamId parameter in the URI.
        /// </summary>
        /// <param name="teamId">The unique identifier for the team.</param>
        /// <param name="name">A user-friendly name for the team.</param>
        /// <returns>Team object.</returns>
        public async Task<SparkApiConnectorApiOperationResult<Team>> UpdateTeamAsync(string teamId, string name)
        {
            var putBody = new Dictionary<string, object>();
            putBody.Add("name",name);
            var path = $"{teamsBase}/{teamId}";
            return await UpdateItemAsync<Team>(path, putBody).ConfigureAwait(false);
        }
    }
}