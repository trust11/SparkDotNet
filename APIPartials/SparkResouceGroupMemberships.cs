
using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{

    public partial class Spark
    {

        private readonly string resourceGroupMembershipsBase = "/v1/resourceGroups/memberships";

        /// <summary>
        /// Lists all resource group memberships for an organization.
        /// Use query parameters to filter the response.
        /// </summary>
        /// <param name="licenseId">List resource group memberships for a license, by ID.</param>
        /// <param name="personId">List resource group memberships for a person, by ID.</param>
        /// <param name="personOrgId">List resource group memberships for an organization, by ID.</param>
        /// <param name="status">Limit resource group memberships to a specific status.</param>
        /// <param name="max">Limit the maximum number of resource group memberships in the response.</param>
        /// <returns>A list of ResourceGroupMembership objects</returns>
        public async Task<SparkApiConnectorApiOperationResult<List<ResourceGroupMembership>>> GetResourceGroupMembershipsAsync(string licenseId = null, string personId = null,
                                                                                          string personOrgId = null, string status = null,
                                                                                          int max = 0)
        {
            var queryParams = new Dictionary<string, string>();
            if (licenseId != null) queryParams.Add("licenseId", licenseId);
            if (personId != null) queryParams.Add("personId", personId);
            if (personOrgId != null) queryParams.Add("personOrgId", personOrgId);
            if (max > 0) queryParams.Add("max", max.ToString());

            return await GetResourceGroupMembershipsAsync(queryParams).ConfigureAwait(false);
        }

        /// <summary>
        /// Lists all resource group memberships for an organization.
        /// Use query parameters to filter the response.
        /// </summary>
        /// <param name="queryParameters">Dictionary with query parameters.</param>
        /// <returns>A list of ResourceGroupMembership objects</returns>
        public async Task<SparkApiConnectorApiOperationResult<List<ResourceGroupMembership>>> GetResourceGroupMembershipsAsync(Dictionary<string, string> queryParameters)
        {
            var path = GetURL(resourceGroupsBase, queryParameters);
            return await GetItemsAsync<ResourceGroupMembership>(path).ConfigureAwait(false);
        }

        /// <summary>
        /// Shows details for a resource group membership, by ID.
        /// Specify the resource group membership ID in the resourceGroupMembershipId URI parameter.
        /// </summary>
        /// <param name="resourceGroupMembershipId">The unique identifier for the resource group membership.</param>
        /// <returns>A ResourceGroupMembership object</returns>
        public async Task<SparkApiConnectorApiOperationResult<ResourceGroupMembership>> GetResourceGroupMembershipAsync(string resourceGroupMembershipId)
        {
            var queryParams = new Dictionary<string, string>();
            var path = GetURL($"{resourceGroupMembershipsBase}/{resourceGroupMembershipId}", queryParams);
            return await GetItemAsync<ResourceGroupMembership>(path).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates a resource group membership, by ID.
        /// Specify the resource group membership ID in the resourceGroupMembershipId URI parameter.
        /// Only the resourceGroupId can be changed with this action. Resource group memberships with a
        /// status of "pending" cannot be updated.For more information about resource group memberships,
        /// see the Managing Hybrid Services guide.
        /// </summary>
        /// <param name="resourceGroupMembershipId">The unique identifier for the resource group membership.</param>
        /// <param name="resourceGroupId">The resource group ID.</param>
        /// <param name="licenseId">The license ID.</param>
        /// <param name="personId">The person ID.</param>
        /// <param name="personOrgId">The organization ID of the person.</param>
        /// <param name="status">The activation status of the resource group membership.</param>
        /// <returns>true if the update succeeded, false otherwise</returns>
        public async Task<SparkApiConnectorApiOperationResult<bool>> UpdateReourceGroupMembershipAsync(string resourceGroupMembershipId, string resourceGroupId, string licenseId, string personId, string personOrgId, string status)
        {
            var bodyParameter = new Dictionary<string, object>();
            bodyParameter.Add("resourceGroupId", resourceGroupId);
            bodyParameter.Add("licenseId", licenseId);
            bodyParameter.Add("personId", personId);
            bodyParameter.Add("personOrgId", personOrgId);
            bodyParameter.Add("status", status);

            return await UpdateItemAsync<bool>($"{resourceGroupMembershipsBase}/{resourceGroupMembershipId}", bodyParameter).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates a resource group membership, by Group Membership Object.
        /// Only the resourceGroupId can be changed with this action. Resource group memberships with a
        /// status of "pending" cannot be updated.For more information about resource group memberships,
        /// see the Managing Hybrid Services guide.
        /// </summary>
        /// <param name="resourceGroupMembership">The ResourceGroupMembership that should be updated.</param>
        /// <returns>true if the update succeeded, false otherwise</returns>
        public async Task<SparkApiConnectorApiOperationResult<bool>> UpdateReourceGroupMembershipAsync(ResourceGroupMembership resourceGroupMembership)
        {
            return await UpdateReourceGroupMembershipAsync(resourceGroupMembership.Id, resourceGroupMembership.ResourceGroupId, resourceGroupMembership.LicenseId, resourceGroupMembership.PersonId, resourceGroupMembership.PersonOrgId, resourceGroupMembership.Status).ConfigureAwait(false);
        }

    }

}