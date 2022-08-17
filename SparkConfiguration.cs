using System;

namespace SparkDotNet
{
    public class SparkConfiguration
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string ApplicationId { get; set; }

        public string SecretKey { get; set; }

        public bool NoProxy { get; set; }

        public string Token { get; set; }

        public string Id { get; set; }

        public Action<string, int> LogAction { get; set; }
    }
}
