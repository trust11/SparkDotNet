using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SparkDotNet
{
    public class RefreshTokenPayload
    {
        public string client_id { get; set; }

        public string client_secret { get; set; }

        public string grant_type { get; } = "refresh_token";

        public bool IsInitialized { get; private set; }

        public string refresh_token { get; set; }

        public Dictionary<string, object> PostBody()
        {
            var body = new Dictionary<string, object>();
            body["grant_type"] = grant_type;
            body["client_id"] = client_id;
            body["client_secret"] = client_secret;
            body["refresh_token"] = refresh_token;
            return body;
        }

        internal void Initialize(SparkConfiguration config)
        {
            client_id = config.ApplicationId;
            client_secret = config.SecretKey;
            refresh_token = config.RefreshToken;
            IsInitialized = true;
        }
    }

    public partial class Spark
    {
        private readonly string refreshTokenBase = "/v1/access_token";
        private readonly RefreshTokenPayload refreshTokenPayload = new RefreshTokenPayload();
        private SparkToken sparkToken;

        public async Task<SparkApiConnectorApiOperationResult> RefreshTokenAsync()
        {
            var path = GetURL(refreshTokenBase);
            var postBody = refreshTokenPayload.PostBody();
            var reqRes = await PostItemAsync<SparkToken>(path, postBody).ConfigureAwait(false);
            if (!reqRes.IsSuccess)
                Log($"Refreshing token failed.\r\n{reqRes.Error}", 2);
            sparkToken = reqRes.Result;
            config.AccessToken = reqRes.Result.AccessToken;
            SetBearerToken();
            return SparkApiConnectorApiOperationResult.Success;
        }

        public async Task<SparkApiConnectorApiOperationResult> RefreshTokenIfRequired([CallerMemberName] string methodName = null)
        {
            if (IsTokenRefreshRequired())
                return await RefreshTokenAsync().ConfigureAwait(false);
            return SparkApiConnectorApiOperationResult.Success;
        }

        public async Task<SparkApiConnectorApiOperationResult> Reinitialize(SparkConfiguration newConfig = null)
        {
            if (sparkToken != null)
            {
                var isCredentialsChanged = config == null
                    || (!string.IsNullOrEmpty(newConfig?.ApplicationId) && config.ApplicationId != newConfig.ApplicationId)
                    || (!string.IsNullOrEmpty(newConfig?.SecretKey) && config.SecretKey != newConfig.SecretKey)
                    || (!string.IsNullOrEmpty(newConfig?.AccessToken) && config.AccessToken != newConfig.AccessToken);
                UpdateConfiguration(newConfig);
                if (isCredentialsChanged)
                {
                    var loginRes = await RefreshTokenAsync().ConfigureAwait(false);
                    if (loginRes.IsSuccess)
                        UpdateCredentials(newConfig);
                    return loginRes;
                }
                else if (IsTokenRefreshRequired())
                {
                    return await RefreshTokenAsync().ConfigureAwait(false);
                }
                return SparkApiConnectorApiOperationResult.Success;
            }
            else
            {
                var loginRes = await RefreshTokenAsync().ConfigureAwait(false);
                if (loginRes.IsSuccess)
                    UpdateCredentials(newConfig);
                return loginRes;
            }
        }

        private bool IsTokenRefreshRequired()
        {
            if (sparkToken is not null && sparkToken.AccessToken is not null)
            {
                int minutesBeforeExpired = 60; //refresh will be done 1h before it is invalid. token is 14 Days valid by default.
                var refreshAt = sparkToken.AccessTokenValidTill.AddMinutes(minutesBeforeExpired * -1);
                var isRequired = refreshAt < DateTime.Now;
                return isRequired;
            }
            return true;
        }

        private void SetBearerToken()
        {
            Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", config.AccessToken);
            if (config.AccessToken is not null)
            {
                Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", config.AccessToken);
            }
            else
            {
                Client.DefaultRequestHeaders.Authorization = null;
                sparkToken = null;
            }
            //bool removed = httpClient.DefaultRequestHeaders.Remove("Bearer");
            //httpClient.DefaultRequestHeaders.Add("Bearer", token);
        }

        private void UpdateCredentials(SparkConfiguration newConfig)
        {
            if (newConfig != null)
            {
                config.ApplicationId = newConfig.ApplicationId;
                config.SecretKey = newConfig.SecretKey;
                config.RefreshToken = newConfig.RefreshToken;
                if (config.AccessToken != newConfig.AccessToken && !string.IsNullOrEmpty(newConfig.AccessToken))
                {
                    config.AccessToken = newConfig.AccessToken;
                    SetBearerToken();
                }
            }
        }
    }

    public class SparkToken
    {
        private string accessToken;

        private DateTime accessTokenValidTill;

        [JsonProperty("access_token")]
        public string AccessToken
        {
            get { return accessToken; }
            set
            {
                accessToken = value;
                AccessTokenValidTill = DateTime.UtcNow;
            }
        }

        [JsonIgnore]
        public DateTime AccessTokenValidTill
        {
            get { return accessTokenValidTill.AddSeconds(ExpiresIn); }
            set { accessTokenValidTill = value; }
        }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("refresh_token_expires_in")]
        public int RefreshTokenExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }
    }
}