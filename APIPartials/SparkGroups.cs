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
        public string Group { get; set; }

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