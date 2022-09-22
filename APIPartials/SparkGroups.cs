using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public enum OperationType
    {
        Delete,
        Add
    }

    public class PathMemberWithOperation : WebexObject
    {
        public string Id { get; set; }

        public OperationType Operation { get; set; }
    }

    public partial class Spark
    {
        private readonly string groupsBase = "/v1/groups";

        /// <summary>
        /// List all licenses for a given organization. If no orgId is specified, the default is the organization of the authenticated user.
        /// </summary>
        /// <param name="orgId">List licenses for this organization.</param>
        /// <param name="max"></param>
        /// <returns>List of License objects.</returns>
        public async Task<SparkApiConnectorApiOperationResult<GroupsOverview>> GetGroupsOverviewAsync(
        string orgId = null,
            string filter = null,
            string attributes = null,
            string sortBy = null,
            string sortOrder = null,
            bool? includeMembers = null,
            int? startIndex = null,
            int? count = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            if (filter != null) queryParams.Add("filter", filter);
            if (attributes != null) queryParams.Add("attributes", attributes);
            if (sortBy != null) queryParams.Add("sortBy", sortBy);
            if (sortOrder != null) queryParams.Add("orgId", sortOrder);
            if (includeMembers != null) queryParams.Add("includeMembers", includeMembers.ToString().ToLower());
            if (startIndex != null) queryParams.Add("startIndex", startIndex.ToString());
            if (count != null) queryParams.Add("count", count.ToString());

            var path = GetURL(groupsBase, queryParams);
            return await GetItemAsync<GroupsOverview>(path);
        }

        /// <summary>
        /// Shows details for a group, by ID.
        /// Optionally, the members may be retreived with this request.The maximum number of members returned is 500
        /// </summary>
        /// <param name="groupId">A unique identifier for the group.</param>
        /// <param name="includeMembers"></param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult<Group>> GetGroup(string groupId, bool? includeMembers = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (includeMembers.HasValue)
                if (includeMembers != null) queryParams.Add("includeMembers", includeMembers.ToString().ToLower());
            var path = GetURL($"{groupsBase}/{groupId}", queryParams);

            return await GetItemAsync<Group>(path).ConfigureAwait(false);
        }

        /// <summary>
        /// extracts a group with all their members
        /// </summary>
        /// <param name="groupId">A unique identifier for the group.</param>
        /// <param name="count">maximal number of group members to extract (if the group has more than 500)</param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult<Group>> GetGroupWithAllMembers(string groupId, int count = 500)
        {
            var groupRes = await GetGroup(groupId, true).ConfigureAwait(false);
            if (!groupRes.IsSuccess)
                return groupRes;
            return await CompleteGroupMembers(groupRes.Result, count).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the members of a group. The default maximum members returned is 500. Control parameters is available to page through the members and to control the size of the results.
        /// </summary>
        /// <param name="groupId">A unique identifier for the group.</param>
        /// <param name="startIndex">The index to start for group pagination</param>
        /// <param name="count">Non-negative Integer. Specifies the desired number of search results per page; e.g., 100. Maximum value for the count is 500</param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult<Group>> GetGroupMembers(string groupId, int? startIndex = null, int? count = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (startIndex.HasValue)
                queryParams.Add("startIndex", startIndex.ToString());
            if (count.HasValue)
                queryParams.Add("count", count.ToString());
            var path = GetURL($"{groupsBase}/{groupId}", queryParams);

            return await GetItemAsync<Group>(path).ConfigureAwait(false);
        }

        /// <summary>
        /// gets all members of a given group regardless of how many are in the group
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult<Group>> GetAllGroupMembers(string groupId, int count = 500)
        {
            var path = $"{groupsBase}/{groupId}";
            var initialGetRes = await GetItemAsync<Group>(path).ConfigureAwait(false);
            if (!initialGetRes.IsSuccess)
                return initialGetRes;
            return await CompleteGroupMembers(initialGetRes.Result, count).ConfigureAwait(false);
        }

        /// <summary>
        /// completes the group member collection if the group has more than 500 members
        /// </summary>
        /// <param name="group">the existing group (with the first 'page' of members already loaded)</param>
        /// <param name="count">Non-negative Integer. Specifies the desired number of search results per page; e.g., 100. Maximum value for the count is 500</param>
        /// <returns></returns>
        private async Task<SparkApiConnectorApiOperationResult<Group>> CompleteGroupMembers(Group group, int count = 500)
        {
            if (count > 500)
                count = 500;
            else if (count < 0)
                count = 1;
            var queryParams = new Dictionary<string, string>();
            int nbItems = group.Members.Count;
            while (group.Members.Count < nbItems)
            {
                queryParams.Add("startIndex", group.Members.Count.ToString());
                queryParams.Add("count", count.ToString());
                var path = GetURL($"{groupsBase}/{group.Id}", queryParams);
                var getRes = await GetItemAsync<Group>(path).ConfigureAwait(false);
                if (!getRes.IsSuccess)
                    return getRes;
                if (getRes.Result.Members != null)
                    group.Members.AddRange(getRes.Result.Members);
            }
            return SparkApiConnectorApiOperationResult<Group>.SuccessResult(group);
        }

        /// <summary>
        /// Update the group details, by ID.
        /// Specify the group ID in the groupId parameter in the URI.
        /// </summary>
        /// <param name="groupId">A unique identifier for the group.</param>
        /// <param name="displayName"></param>
        /// <param name="description"></param>
        /// <param name="members"></param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult<Group>> UpdateGroupsOverviewAsync(string groupId, string displayName = null, string description = null, List<PathMemberWithOperation> members = null)
        {
            var queryParams = new Dictionary<string, string>();
            var putBody = new Dictionary<string, object>();
            if (displayName != null) putBody.Add("displayName", displayName);
            if (description != null) putBody.Add("description", description);
            if (members != null) putBody.Add("members", members);

            var path = GetURL($"{groupsBase}/{groupId}", queryParams);
            return await PatchItemAsync<Group>(path, putBody).ConfigureAwait(false);
        }
    }
}