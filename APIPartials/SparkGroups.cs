
using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SparkDotNet
{

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
    }
}