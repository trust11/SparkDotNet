using SparkDotNet.ExceptionHandling;
using SparkDotNet.Models;

using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public partial class Spark
    {
        private async Task<SparkApiConnectorApiOperationResult<T>> GetSharedLineMembersAsync<T>(string url, string personId, string applicationId = null)
        {
            var path = GetURL(string.Format(url, personId, applicationId));
            return await GetItemAsync<T>(path).ConfigureAwait(false);
        }

        private async Task<SparkApiConnectorApiOperationResult> UpdateSharedLineMembersAsync<T>(string url, string personId, string applicationId, T setting)
        {
            var path = GetURL(string.Format(url, personId, applicationId));
            return await UpdateItemAsync(path, setting).ConfigureAwait(false);
        }

        private async Task<SparkApiConnectorApiOperationResult<TRetunType>> UpdateSharedLineMembersAsync<TRetunType, TPayloadType>(string url, string personId, string applicationId, TPayloadType setting)
        {
            var path = GetURL(string.Format(url, personId, applicationId));
            return await UpdateItemAsync<TRetunType, TPayloadType>(path, setting).ConfigureAwait(false);
        }
    }
}