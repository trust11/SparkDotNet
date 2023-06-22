using Newtonsoft.Json;

using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SparkDotNet;
public partial class Spark
{
    private static string personPrivacySettingsBaseUrl { get; } = "/v1/people/{0}/features/privacy";

    /// <summary>
    /// Get a person's privacy settings for the specified person ID.
    /// The privacy feature enables the person's line to be monitored by others and determine if they can be reached by Auto Attendant services.zation's default.
    /// This API requires a full, user, or read-only administrator auth token with a scope of spark-admin:people_read.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="orgId"></param>
    public async Task<SparkApiConnectorApiOperationResult<PersonPrivacySettings>> GetPersonsPrivacySettingsAsync(string personId, string orgId = null)
    {
        return await GetPersonSettingAsync<PersonPrivacySettings>(personPrivacySettingsBaseUrl, personId, orgId).ConfigureAwait(false);
    }

    /// <summary>
    /// Configure a person's privacy settings for the specified person ID.
    /// The privacy feature enables the person's line to be monitored by others and determine if they can be reached by Auto Attendant services.
    /// This API requires a full or user administrator auth token with the spark-admin:people_write scope.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="payload"></param>
    /// <param name="orgId"></param>
    public async Task<SparkApiConnectorApiOperationResult<PersonPrivacySettings>> UpdatePersonsPrivacySettingsAsync(string personId, PersonPrivacySettings payload, string orgId = null)
    {
        return await UpdatePersonSettingAsync<PersonPrivacySettings>(personPrivacySettingsBaseUrl, personId, payload, orgId);
    }
}