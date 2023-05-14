using System;

namespace SparkDotNet
{
    public class SparkConfiguration
    {
        public string AccessToken { get; set; }

        public string ApplicationId { get; set; }

        public string Id { get; set; }

        public Action<string, int> LogAction { get; set; }

        public bool NoProxy { get; set; }

        public string RefreshToken { get; set; }

        public string SecretKey { get; set; }
    }
}