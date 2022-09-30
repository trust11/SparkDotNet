using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private readonly string devicesBase = "/v1/devices";

        /// <summary>
        /// Lists all active Webex devices associated with the authenticated user, such as devices activated in personal mode.
        /// Administrators can list all devices within an organization.
        /// </summary>
        /// <param name="personId">List devices by person ID.</param>
        /// <param name="workspaceId">List devices by workspace ID.</param>
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
        public async Task<SparkApiConnectorApiOperationResult<List<Device>>> GetDevicesAsync(string personId = null, string workspaceId = null, string orgId = null,
                                                        string displayName = null, string product = null, string tag = null,
                                                        string connectionStatus = null, string serial = null, string software = null,
                                                        string upgradeChannel = null, string errorCode = null, string capability = null,
                                                        string permission = null,
                                                        int start = 0, int max = 0)
        {
            var queryParams = new Dictionary<string, string>();

            if (personId != null) queryParams.Add("personId", personId);
            if (workspaceId != null) queryParams.Add("placeId", workspaceId);
            if (orgId != null) queryParams.Add("orgId", orgId);
            if (displayName != null) queryParams.Add("displayName", displayName);
            if (product != null) queryParams.Add("product", product);
            if (tag != null) queryParams.Add("tag", tag);
            if (connectionStatus != null) queryParams.Add("connectionStatus", connectionStatus);
            if (serial != null) queryParams.Add("serial", serial);
            if (software != null) queryParams.Add("software", software);
            if (upgradeChannel != null) queryParams.Add("upgradeChannel", upgradeChannel);
            if (errorCode != null) queryParams.Add("errorCode", errorCode);
            if (capability != null) queryParams.Add("capability", capability);
            if (permission != null) queryParams.Add("permission", permission);
            if (start > 0) queryParams.Add("start", max.ToString());
            if (max > 0) queryParams.Add("max", max.ToString());

            return await GetDevicesAsync<Device>(queryParams).ConfigureAwait(false);
        }

        /// <summary>
        /// Lists all active Webex devices associated with the authenticated user, such as devices activated in personal mode.
        /// Administrators can list all devices within an organization.
        /// </summary>
        /// <param name="queryParameters">Dictionary with request parameters</param>
        /// <returns>A list of Device objects</returns>
        public async Task<SparkApiConnectorApiOperationResult<List<Device>>> GetDevicesAsync<Device>(Dictionary<string, string> queryParameters) =>
            await GetItemsAsync<Device>(GetURL(devicesBase, queryParameters)).ConfigureAwait(false);

        /// <summary>
        /// Shows details for a device, by ID.
        /// Specify the device ID in the deviceId parameter in the URI.
        /// </summary>
        /// <param name="deviceId">A unique identifier for the device.</param>
        /// <returns>A device objects</returns>
        public async Task<SparkApiConnectorApiOperationResult<Device>> GetDeviceAsync(string deviceId)
        {
            var queryParams = new Dictionary<string, string>();
            var path = GetURL($"{devicesBase}/{deviceId}", queryParams);
            return await GetItemAsync<Device>(path).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a device, by ID.
        /// Specify the device ID in the deviceId parameter in the URI.
        /// </summary>
        /// <param name="deviceId">A unique identifier for the device.</param>
        /// <returns>true if the device was deleted, false otherwise</returns>
        public async Task<SparkApiConnectorApiOperationResult<bool>> DeleteDeviceAsync(string deviceId) => await DeleteItemAsync($"{devicesBase}/{deviceId}").ConfigureAwait(false);

        /// <summary>
        /// Deletes a device, by ID.
        /// Specify the device ID in the deviceId parameter in the URI.
        /// </summary>
        /// <param name="device">A Device object to delete.</param>
        /// <returns>true if the device was deleted, false otherwise</returns>
        public async Task<SparkApiConnectorApiOperationResult<bool>> DeleteDeviceAsync(Device device) => await DeleteDeviceAsync(device.Id).ConfigureAwait(false);

        /// <summary>
        /// Generate an activation code for a device in a specific place by placeId.
        /// Currently, activation codes may only be generated for shared places--personal mode is not supported.
        /// </summary>
        /// <param name="workspaceId">The placeId of the place where the device will be activated.</param>
        /// <returns>A Device Activation COde objects</returns>
        public async Task<SparkApiConnectorApiOperationResult<DeviceActivationCode>> CreateDeviceActivationCodeAsync(string workspaceId)
        {
            var bodyParams = new Dictionary<string, object>
            {
                { "workspaceId", workspaceId }
            };
            return await PostItemAsync<DeviceActivationCode>($"{devicesBase}/activationCode", bodyParams).ConfigureAwait(false);
        }

        /// <summary>
        /// Generate an activation code for a device in a specific place by placeId.
        /// Currently, activation codes may only be generated for shared places--personal mode is not supported.
        /// </summary>
        /// <param name="place">The place object of the place where the device will be activated.</param>
        /// <returns>A Device Activation COde objects</returns>
        public async Task<SparkApiConnectorApiOperationResult<DeviceActivationCode>> CreateDeviceActivationCodeAsync(Workspace workspace) =>
            await CreateDeviceActivationCodeAsync(workspace.Id).ConfigureAwait(false);
    }
}