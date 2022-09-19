using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private readonly string membershipsBase = "/v1/memberships";

        /// <summary>
        /// Lists all room memberships. By default, lists memberships for rooms to which the authenticated user belongs.
        /// - Use query parameters to filter the response.
        /// - Use roomId to list memberships for a room, by ID.
        /// - Use either personId or personEmail to filter the results. The roomId parameter is required when using these parameters.
        /// </summary>
        /// <remarks>
        /// Note: For moderated team spaces, the list of memberships will include only the space moderators if the user
        /// is a team member but not a direct participant of the space.
        /// </remarks>
        /// <param name="roomId">List memberships associated with a room, by ID.</param>
        /// <param name="personId">List memberships associated with a person, by ID. The roomId parameter is required when using this parameter.</param>
        /// <param name="personEmail">List memberships associated with a person, by email address. The roomId parameter is required when using this parameter.</param>
        /// <param name="max">Limit the maximum number of memberships in the response. Default: 100</param>
        /// <returns>A List of Membership objects.</returns>
        public async Task<SparkApiConnectorApiOperationResult<List<Membership>>> GetMembershipsAsync(string roomId = null, string personId = null, string personEmail = null, int max = 0)
        {
            // Check if room Id is not empty when personId or person email is not empty
            if (personId != null || personEmail != null && roomId == null)
                return new SparkApiConnectorApiOperationResult<List<Membership>>() { ErrorMessage = $"If it is used either personId or personEmail to filter the results, then the roomId parameter is required.", ResultCode = SparkApiOperationResultCode.OtherError};
            var queryParams = new Dictionary<string, string>();
            if (roomId != null) queryParams.Add("roomId", roomId);
            if (personId != null) queryParams.Add("personId", personId);
            if (personEmail != null) queryParams.Add("personEmail", personEmail);
            if (max > 0) queryParams.Add("max", max.ToString());

            var path = GetURL(membershipsBase, queryParams);
            return await GetItemsAsync<Membership>(path);
        }

        /// <summary>
        /// Add someone to a room by Person ID or email address; optionally making them a moderator.
        /// </summary>
        /// <param name="roomId">The room ID.</param>
        /// <param name="personId">The person ID.</param>
        /// <param name="personEmail">The email address of the person.</param>
        /// <param name="isModerator">Whether or not the participant is a room moderator.</param>
        /// <returns>Membership object.</returns>
        public async Task<SparkApiConnectorApiOperationResult<Membership>> CreateMembershipAsync(string roomId, string personId = null, string personEmail = null, bool isModerator = false)
        {
            var postBody = new Dictionary<string, object>
            {
                { "roomId", roomId }
            };
            if (personId != null) { postBody.Add("personId", personId); }
            if (personEmail != null) { postBody.Add("personEmail", personEmail); }
            postBody.Add("isModerator", isModerator);
            return await PostItemAsync<Membership>(membershipsBase, postBody);
        }

        /// <summary>
        /// Deletes a membership by ID.
        /// Specify the membership ID in the membershipId URI parameter.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the membership.</param>
        /// <returns>Boolean representing the success of the operation.</returns>
        public async Task<SparkApiConnectorApiOperationResult<bool>> DeleteMembershipAsync(string membershipId) => await DeleteItemAsync($"{membershipsBase}/{membershipId}");

        /// <summary>
        /// Deletes a membership by Membership Object.
        /// Specify the membership object in the membership parameter.
        /// </summary>
        /// <param name="membership">The Membership object for the membership.</param>
        /// <returns>Boolean representing the success of the operation.</returns>
        public async Task<SparkApiConnectorApiOperationResult<bool>> DeleteMembershipAsync(Membership membership) => await DeleteTeamMembershipAsync(membership.Id);

        /// <summary>
        /// Updates properties for a membership by ID.
        /// Specify the membership ID in the membershipId URI parameter.
        /// </summary>
        /// <param name="membershipId">The unique identifier for the membership.</param>
        /// <param name="isModerator">Whether or not the participant is a room moderator.</param>
        /// <param name="isRoomHidden">Whether or not the room is hidden in the Webex Teams clients.</param>
        /// <returns>Membership object.</returns>
        public async Task<SparkApiConnectorApiOperationResult<Membership>> UpdateMembershipAsync(string membershipId, bool isModerator, bool isRoomHidden)
        {
            var putBody = new Dictionary<string, object>
            {
                { "isModerator", isModerator },
                { "isRoomHidden", isRoomHidden }
            };
            var path = $"{membershipsBase}/{membershipId}";
            return await UpdateItemAsync<Membership>(path, putBody);
        }

        /// <summary>
        /// Updates properties for a membership by object.
        /// </summary>
        /// <param name="membership">The membership object to be updatad.</param>
        /// <returns>Membership object.</returns>
        public async Task<SparkApiConnectorApiOperationResult<Membership>> UpdateMembershipAsync(Membership membership) => await UpdateMembershipAsync(membership.Id, membership.IsModerator, membership.IsRoomHidden);
    }
}