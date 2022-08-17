
using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public enum OwnerType
    {
        PEOPLE,
        PLACE,
        AUTO_ATTENDANT,
        CALL_CENTER,
        GROUP_PAGING,
        HUNT_GROUP,
        VOICE_MESSAGING,
        BROADWORKS_ANYWHERE,
        CONTACT_CENTER_LINK,
        ROUTE_LIST,
        VOICEMAIL_GROUP,
    }

    public partial class Spark
    {
        private readonly string numbersBase = "/v1/telephony/config/numbers";

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
        public async Task<SparkApiConnectorApiOperationResult<List<PhoneNumberDetails>>> GetNumbersAsync(
            string orgId = null,
            string location = null,
            int? max = null,
            int? start = null,
            string phoneNumber = null,
            bool? available = null,
            string order = null,
            string ownerName = null,
            string ownerId = null,
            OwnerType? ownerType = null,
            string extension = null,
            string numberType = null,
            string state = null,
            bool? details = null,
            bool? tollFreeNumbers = null
            )
        {
            var queryParams = new Dictionary<string, string>();
            if (orgId != null) queryParams.Add("orgId", orgId);
            if (location != null) queryParams.Add("location", location);
            if (max > 0) queryParams.Add("max", max.ToString());
            if (start != null) queryParams.Add("start", start.ToString());
            if (phoneNumber != null) queryParams.Add("phoneNumber", phoneNumber);
            if (available != null) queryParams.Add("available", available.ToString());
            if (order != null) queryParams.Add("order", order);
            if (ownerName != null) queryParams.Add("ownerName", ownerName);
            if (ownerId != null) queryParams.Add("ownerId", ownerId);
            if (ownerType != null) queryParams.Add("ownerType", ownerType.ToString());
            if (extension != null) queryParams.Add("extension", extension);
            if (numberType != null) queryParams.Add("numberType", numberType);
            if (state != null) queryParams.Add("state", state);
            if (details != null) queryParams.Add("details", details.ToString());
            if (tollFreeNumbers != null) queryParams.Add("tollFreeNumbers", tollFreeNumbers.ToString());

            var path = GetURL(numbersBase, queryParams);
            return await GetItemsAsync<PhoneNumberDetails>(path, "phoneNumbers").ConfigureAwait(false);
        }

        /////////////// <summary>
        /////////////// Shows details for a license, by ID.
        /////////////// Specify the license ID in the licenseId parameter in the URI.
        /////////////// </summary>
        /////////////// <param name="licenseId">The unique identifier for the license.</param>
        /////////////// <returns>License object.</returns>
        ////////////public async Task<SparkApiConnectorApiOperationResult<License>> GetLicenseAsync(string licenseId)
        ////////////{
        ////////////    var queryParams = new Dictionary<string, string>();
        ////////////    var path = GetURL($"{numbersBase}", queryParams);
        ////////////    return await GetItemAsync<License>(path);
        ////////////}
    }
}