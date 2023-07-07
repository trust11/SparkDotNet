using SparkDotNet.Models;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace SparkDotNet;

/// <summary>
/// https://help.webex.com/en-us/article/d6gavk/Shared-line-appearance-for-Webex-App
/// </summary>

public partial class Spark
{
    private static string personSharedLineAppearanceMemberBaseUrl { get; } = "/v1/telephony/config/people/{0}/applications/{1}";

    private static string personSharedLineAppearanceMembersGetSetUrl { get; } = $"{personSharedLineAppearanceMemberBaseUrl}/members";

    private static string personSharedLineAppearanceMembersSearchUrl { get; } = $"{personSharedLineAppearanceMemberBaseUrl}/availableMembers";

    /// <summary>
    /// Get a person's privacy settings for the specified person ID.
    /// The privacy feature enables the person's line to be monitored by others and determine if they can be reached by Auto Attendant services.zation's default.
    /// This API requires a full, user, or read-only administrator auth token with a scope of spark-admin:people_read.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="orgId"></param>
    public async Task<SparkApiConnectorApiOperationResult<SharedLineMembersGetResult>> GetSharedLineAppearanceMembersAsync(string personId, string applicationId)
    {
        return await GetSharedLineMembersAsync<SharedLineMembersGetResult>(personSharedLineAppearanceMembersGetSetUrl, personId, applicationId).ConfigureAwait(false);
    }

    /// <summary>
    /// Configure a person's privacy settings for the specified person ID.
    /// The privacy feature enables the person's line to be monitored by others and determine if they can be reached by Auto Attendant services.
    /// This API requires a full or user administrator auth token with the spark-admin:people_write scope.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="payload"></param>
    /// <param name="orgId"></param>
    public async Task<SparkApiConnectorApiOperationResult> UpdateSharedLineAppearanceMembersAsync(string personId, string applicationId, PutSharedLineMemberItems payload)
    {
        var intPay = payload.Members.Select(o => (AvailableSharedLineMemberItem)o).Select(o=>o.GetPutSharedLineMemberItemInstance());
        return await UpdateSharedLineMembersAsync(personSharedLineAppearanceMembersGetSetUrl, personId, applicationId, intPay, true);
    }

    /// <summary>
    /// Get members available for shared-line assignment to a Webex Calling Apps Desktop device.
    /// This API requires a full or user administrator auth token with the spark-admin:people_read scope.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="payload"></param>
    /// <param name="orgId"></param>
    /// <returns></returns>
    public async Task<SparkApiConnectorApiOperationResult<AvailableSharedLineMemberItems>> SearchSharedLineAppearanceMembersAsync(string personId, string applicationId, AvailableSharedLineMemberItemsPayload payload = null)
    {
        return await GetSharedLineMembersAsync<AvailableSharedLineMemberItems>(personSharedLineAppearanceMembersSearchUrl, personId, applicationId).ConfigureAwait(false);
    }
}