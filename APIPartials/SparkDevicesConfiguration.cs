using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public enum DevicesConfigurationOp
    {
        Remove, Replace
    }

    public static class ContentJsonTypes
    {
        public const string ApplicationJson = "application/json";
        public const string ApplicationJsonPatch = "application/json-patch+json";
    }

    public partial class Spark
    {
        private readonly string deviceConfigurationsBase = "/v1/deviceConfigurations";

        /// <summary>
        /// Lists all active Webex devices associated with the authenticated user, such as devices activated in personal mode.
        /// Administrators can list all devices within an organization.
        /// </summary>
        /// <param name="deviceId">List devices by person ID.</param>
        /// <param name="key">List devices by workspace ID.</param>
        /// <param name="orgId">List devices in this organization. Only admin users of another organization (such as partners) may use this parameter.</param>
        /// <param name="displayName">List devices with this display name.</param>
        /// <param name="product">List devices with this product name. Possible values: DX-80, RoomKit, SX-80</param>
        /// <param name="tag">List devices which have a tag. Accepts multiple values separated by commas.</param>
        /// <param name="connectionStatus">List devices with this connection status.</param>
        /// <param name="serial">List devices with this serial number.</param>
        /// <param name="software">List devices with this software version.</param>
        /// <param name="upgradeChannel">List devices with this upgrade channel.</param>
        /// <param name="errorCode">List devices with this error code.</param>
        /// <param name="capability">List devices with this capability. Possible values: xapi</param>
        /// <param name="permission">List devices with this permission.</param>
        /// <param name="start">Offset. Default is 0.</param>
        /// <param name="max">Limit the maximum number of devices in the response.</param>
        /// <returns>A list of Device objects</returns>
        public async Task<SparkApiConnectorApiOperationResult<DeviceConfiguration>> GetDeviceConfiguration(string deviceId, string key = null)
        {
            var queryParams = new Dictionary<string, string>()
            {
                {"deviceId", deviceId}
            };
            if (key != null) queryParams.Add("key", key);

            return await GetItemAsync<DeviceConfiguration>(GetURL(deviceConfigurationsBase, queryParams)).ConfigureAwait(false);
        }

        /// <summary>
        /// Update requests
        /// Use JSON Patch syntax:
        /// When using JSON Patch you are required to specify a Content-Type header with value application/json-patch+json.
        /// </summary>
        /// <param name="deviceId">Update device configurations by device ID</param>
        /// <param name="op">Remove or Replace of the enum <c>DevicesConfigurationOp</c></param>
        /// <param name="path">Only paths ending in /sources/configured/value are supported.</param>
        /// <param name="value">One of string OR number OR boolean</param>
        /// <returns></returns>
        public async Task<SparkApiConnectorApiOperationResult<DeviceConfiguration>> UpdateDevicConfigurationsAsync(string deviceId, DevicesConfigurationOp op, string path, object value)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "deviceId", deviceId }
            };
            var bodyParams = new Dictionary<string, object>
            {
                { "op", op }, { "path", $"{path}/sources/configured/value"}, { "value",  value}
            };

            return await PatchItemAsync<DeviceConfiguration>(GetURL(deviceConfigurationsBase, queryParams), bodyParams, ContentJsonTypes.ApplicationJsonPatch).ConfigureAwait(false);
        }
    }
}