using SparkDotNet.Models;

using System.Threading.Tasks;

namespace SparkDotNet;

public partial class Spark
{
    private static string personMonitoringUrl { get; } = "/v1/people/{0}/features/monitoring";

    /// <summary>
    /// Retrieves the monitoring settings of the person, which shows specified people, places, virtual lines or call park extenions that are being monitored.
    /// Monitors the line status which indicates if a person, place or virtual line is on a call and if a call has been parked on that extension.
    /// This API requires a full, user, or read-only administrator auth token with a scope of spark-admin:people_read.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="orgId"></param>
    public async Task<SparkApiConnectorApiOperationResult<PersonMonitoringSettings>> GetPersonsMonitoringSettingsAsync(string personId, string orgId = null)
    {
        return await GetPersonSettingAsync<PersonMonitoringSettings>(personMonitoringUrl, personId, orgId).ConfigureAwait(false);
    }

    /// <summary>
    /// Configure a person's privacy settings for the specified person ID.
    /// The privacy feature enables the person's line to be monitored by others and determine if they can be reached by Auto Attendant services.
    /// This API requires a full or user administrator auth token with the spark-admin:people_write scope.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="payload"></param>
    /// <param name="orgId"></param>
    public async Task<SparkApiConnectorApiOperationResult> UpdatePersonsMonitoringSettingsAsync(string personId, PersonCallParkSettings payload, string orgId = null)
    {
        return await UpdatePersonSettingAsync(personMonitoringUrl, personId, payload, orgId);
    }
}