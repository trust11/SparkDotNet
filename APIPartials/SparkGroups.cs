using SparkDotNet.Models;
using System.Collections.Generic;
using System.Linq;
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
        /// Shows details for a group, by ID.
        /// Optionally, the members may be retrieved with this request.The maximum number of members returned is 500
        /// </summary>
        /// <param name="groupId">A unique identifier for the group.</param>
        /// <param name="includeMembers"></param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult<Group>> GetGroupAsync(string groupId, bool? includeMembers = null) //todo: GetGroupByIdAsync. IF Has value or !=null is enough
        {
            var queryParams = new Dictionary<string, string>();
            if (includeMembers.HasValue)
                if (includeMembers != null) queryParams.Add("includeMembers", includeMembers.ToString().ToLower());
            var path = GetURL($"{groupsBase}/{groupId}", queryParams);

            return await GetItemAsync<Group>(path).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the members of a group. The default maximum members returned is 500.
        /// Control parameters is available to page through the members and to control the size of the results.
        /// </summary>
        /// <param name="groupId">A unique identifier for the group.</param>
        /// <param name="startIndex">The index to start for group pagination</param>
        /// <param name="pageSize">Non-negative Integer. Specifies the desired number of search results per page;
        ///                     e.g., 100. Maximum value for the count is 500</param>
        /// <param name="maxUserCount">amount of user who have to be delivered</param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult<Group>> GetGroupMembersAsync(string groupId, int? startIndex = null, int? pageSize = null, int? maxUserCount = null)
        {
            SparkApiConnectorApiOperationResult<Group> result = new SparkApiConnectorApiOperationResult<Group>()
            {
                Result = new Group()
                {
                    Id = groupId,
                    Members = new List<Member>()
                }
                ,
                ResultCode = ExceptionHandling.SparkApiOperationResultCode.OK
            };
            if (pageSize != 0)
            {
                var path = CreateUrlForGetGroupMembersAsync(groupId, startIndex, pageSize);
                var getRes = await GetItemAsync<Group>(path).ConfigureAwait(false);
                if (!getRes.IsSuccess)
                    return getRes;
                var group = getRes.Result;
                result = await CompleteGroupMembersAsync(group, getRes.NextLink, pageSize, maxUserCount).ConfigureAwait(false);
            }
            return result;
        }

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
            var result = await GetItemAsync<GroupsOverview>(path).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// extracts a group with all their members
        /// </summary>
        /// <param name="groupId">A unique identifier for the group.</param>
        /// <param name="maxUserCount">maximal number of group members to extract</param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult<Group>> GetGroupWithAllMembersAsync(string groupId, int? maxUserCount = null)
        {
            var groupRes = await GetGroupAsync(groupId, true).ConfigureAwait(false);
            if (!groupRes.IsSuccess)
                return groupRes;
            maxUserCount ??= int.MaxValue;
            if (maxUserCount < 0) maxUserCount = 0;
            int deltaUserCount = (int)maxUserCount - groupRes.Result.Members.Count;
            if (deltaUserCount > 0)
            {
                deltaUserCount = deltaUserCount < 0 ? 0 : deltaUserCount; // in case that less then 500 where asked but Get group is delivering min 500
                var pageSize = deltaUserCount > 500 ? 500 : deltaUserCount;
                if (deltaUserCount != 0)
                {
                    var result = await GetGroupMembersAsync(groupId, groupRes.Result.Members.Count + 1, pageSize, deltaUserCount).ConfigureAwait(false);
                    if (!result.IsSuccess)
                        return result;
                    groupRes.Result.Members.AddRange(result.Result.Members);
                }
            }
            if (maxUserCount != null && maxUserCount < groupRes.Result.Members.Count)
                groupRes.Result.Members = groupRes.Result.Members.Take((int)maxUserCount).ToList();
            return groupRes;
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
            var putBody = new Dictionary<string, object>();
            if (displayName != null) putBody.Add("displayName", displayName);
            if (description != null) putBody.Add("description", description);
            if (members != null) putBody.Add("members", members.Take(members.Count > 500 ? 500 : members.Count).ToList());

            var path = GetURL($"{groupsBase}/{groupId}");
            var result = await PatchItemAsync<Group>(path, putBody).ConfigureAwait(false);
            result = await UpdateGroupsOverviewNextLinkAsync(result, putBody).ConfigureAwait(false);
            return result;
        }

        private static Dictionary<string, string> CreateQueryParametersforGroupMembersAsync(int? startIndex, int? pageSize)
        {
            var queryParams = new Dictionary<string, string>();
            //workaround startIndex>0 is needed as the API would not return all members when 0 is send
            if (startIndex.HasValue && startIndex > 0) queryParams.Add("startIndex", startIndex.ToString());
            if (pageSize.HasValue) queryParams.Add("count", pageSize.ToString());
            return queryParams;
        }

        /// <summary>
        /// completes the group member collection if the group has more than 500 members
        /// </summary>
        /// <param name="group">the existing group (with the first 'page' of members already loaded)</param>
        /// <param name="maxUserCount">amount of user who have to be delivered</param>
        /// <returns></returns>
        private async Task<SparkApiConnectorApiOperationResult<Group>> CompleteGroupMembersAsync(Group group, string nextLink, int? pageSize = null, int? maxUserCount = null)
        {
            if (group.Members.Count < maxUserCount)
                if (nextLink != null && group.Members.Count < (maxUserCount ?? int.MaxValue))
                {
                    if (pageSize != null && group.Members.Count + pageSize > maxUserCount)
                    {
                        int deltaUserCount = (int)maxUserCount - group.Members.Count;
                        pageSize = deltaUserCount > group.MemberSize ? group.MemberSize : deltaUserCount;
                        nextLink = CreateUrlForGetGroupMembersAsync(group.Id, group.Members.Count + 1, pageSize);
                    }

                    var getRes = await GetItemAsync<Group>(nextLink).ConfigureAwait(false);
                    if (!getRes.IsSuccess)
                        return getRes;
                    if (getRes.Result.Members != null)
                        group.Members.AddRange(getRes.Result.Members);
                    var result = await CompleteGroupMembersAsync(group, getRes.NextLink, pageSize, maxUserCount).ConfigureAwait(false);
                }
            return SparkApiConnectorApiOperationResult<Group>.SuccessResult(group);
        }

        private string CreateUrlForGetGroupMembersAsync(string groupId, int? startIndex, int? pageSize)
        {
            var queryParams = CreateQueryParametersforGroupMembersAsync(startIndex, pageSize);
            var path = GetURL($"{groupsBase}/{groupId}/members", queryParams);
            return path;
        }

        private async Task<SparkApiConnectorApiOperationResult<Group>> UpdateGroupsOverviewNextLinkAsync(SparkApiConnectorApiOperationResult<Group> presult, Dictionary<string, object> putBody)
        {
            if (presult.NextLink != null)
            {
                var result = await PatchItemAsync<Group>(presult.NextLink, putBody).ConfigureAwait(false);
                presult.Result.Members.AddRange(result.Result.Members);
                presult = await UpdateGroupsOverviewNextLinkAsync(presult, putBody).ConfigureAwait(false);
            }

            return presult;
        }
    }
}