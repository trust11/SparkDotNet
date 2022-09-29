
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{

    public partial class Spark
    {

        private readonly string resourceGroupsBase = "/v1/resourceGroups";

        /// <summary>
        /// ist resource groups.
        /// Use query parameters to filter the response.
        /// </summary>
        /// <param name="orgId">List resource groups in this organization. Only admin users of another organization (such as partners) may use this parameter.</param>
        /// <returns>A list of Resource Group objects</returns>
        public async Task<SparkApiConnectorApiOperationResult<List<ResourceGroup>>> GetResourceGroupsAsync(string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = GetURL(resourceGroupsBase, queryParams);
            return await GetItemsAsync<ResourceGroup>(path).ConfigureAwait(false);
        }

        /// <summary>
        /// Shows details for a resource group, by ID.
        /// Specify the resource group ID in the resourceGroupId parameter in the URI.
        /// </summary>
        /// <param name="resourceGroupId">The unique identifier for the resource group.</param>
        /// <returns>A Resource Group object</returns>
        public async Task<SparkApiConnectorApiOperationResult<ResourceGroup>> GetResourceGroupAsync(string resourceGroupId)
        {
            var queryParams = new Dictionary<string, string>();
            var path = GetURL($"{resourceGroupsBase}/{resourceGroupId}", queryParams);
            return await GetItemAsync<ResourceGroup>(path).ConfigureAwait(false);
        }


    }

}