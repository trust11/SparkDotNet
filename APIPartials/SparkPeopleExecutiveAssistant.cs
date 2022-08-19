using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{

    public partial class Spark
    {
        private static string peopleExecutiveAssistantBase { get; } = "/v1/people/{0}/features/executiveAssistant";


        /// <summary>
        /// Retrieve the executive assistant settings for the specified personId.

        /// People with the executive service enabled, can select from a pool of assistants who have been assigned
        /// the executive assistant service and who can answer or place calls on their behalf.Executive assistants
        /// can set the call forward destination and join or leave an executive's pool.
        /// 
        /// This API requires a full, user, or read-only administrator auth token with a scope of spark-admin:people_read.
        /// </summary>
        /// <param name="orgId">List people in this organization. Only admin users of another organization (such as partners) may use this parameter.</param>
        public async Task<SparkApiConnectorApiOperationResult<PersonExecutive>> GetPersonExecutiveAssistantAsync(string personId, string orgId = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            var path = GetURL(string.Format(peopleExecutiveAssistantBase, personId), queryParams);

            return await GetItemAsync<PersonExecutive>(path);
        }
    }
}
