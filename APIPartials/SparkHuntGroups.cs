
using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SparkDotNet
{
    public partial class Spark
    {
        private readonly string huntGroupBase = "/v1/telephony/config/huntGroups";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgId"></param>
        /// <param name="location"></param>
        /// <param name="max"></param>
        /// <param name="start"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="available"></param>
        /// <param name="order"></param>
        /// <param name="ownerName"></param>
        /// <param name="ownerId"></param>
        /// <param name="ownerType"></param>
        /// <param name="extension"></param>
        /// <param name="numberType"></param>
        /// <param name="state"></param>
        /// <param name="details"></param>
        /// <param name="tollFreeNumbers"></param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult<HuntGroupList>> GetHuntGroupsAsync(
            string orgId = null,
            string locationId = null,
            int? max = null,
            int? start = null,
            string name = null,
            string phoneNumber = null)
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            if (locationId != null) queryParams.Add("locationId", locationId);
            if (max > 0) queryParams.Add("max", max.ToString());
            if (start != null) queryParams.Add("start", start.ToString());
            if (phoneNumber != null) queryParams.Add("phoneNumber", phoneNumber);
            if (name != null) queryParams.Add("name", name);

            var path = GetURL(huntGroupBase, queryParams);
            return await GetItemAsync<HuntGroupList>(path).ConfigureAwait(false);
        }
    }
}