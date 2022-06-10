using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private static string WorkspaceCallForwardingBase { get; } = "/v1/workspaces/{0}/features/callForwarding";

        /// <summary>
        /// Retrieve Call Forwarding Settings for a Workspace. Two types of call forwarding are supported:
        /// 1 When busy – forwards all incoming calls to the destination you chose whilethe phone is in use or the person is busy.
        /// 2 When no answer – forwarding only occurs when you are away or not answering your phone.
        ///
        /// This API requires a full or read-only administrator auth token with a scope of
        /// spark-admin:workspaces_read or a user auth token with spark:workspaces_read scope can be
        /// used to read workspace settings.
        /// </summary>
        /// <param name="personId"></param>
        /// <param name="orgId"></param>
        /// <returns><c>PersonBargeInSetting</c> object</returns>
        public async Task<SparkApiConnectorApiOperationResult<WorkspaceCallForwardingSetting>> GetWorkspaceCallForwardingSettingAsync(string personId, string orgId = null)
        {
            return await GetPersonSettingAsync<WorkspaceCallForwardingSetting>(WorkspaceCallForwardingBase, personId, orgId);
        }

        public async Task UpdateWorkspaceCallForwardingSettingAsync(string personId, WorkspaceCallForwardingSetting setting, string orgId = null)
        {
            await UpdatePersonSettingAsync(WorkspaceCallForwardingBase, personId, setting, orgId);
        }
    }
}