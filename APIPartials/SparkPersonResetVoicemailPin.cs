using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SparkDotNet;
public partial class Spark
{
    private readonly string resetPinUrlBase = "/v1/people/{0}/features/voicemail/actions/resetPin/invoke";

    public async Task<SparkApiConnectorApiOperationResult> ResetVoicemailPIN(string personId, string orgId = null)
    {
        var queryParams = new Dictionary<string, string>();
        if (orgId != null) queryParams.Add("orgId", orgId);
        var path = GetURL(string.Format(resetPinUrlBase, personId), queryParams);
        var body = new Dictionary<string, object>();
        return await PostItemAsync<SparkApiConnectorApiOperationResult>(path, body).ConfigureAwait(false);
    }
}
