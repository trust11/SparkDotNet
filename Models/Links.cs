using Newtonsoft.Json;

namespace SparkDotNet.Models
{
    public class Links
    {
        public string Next { get; set; }

        public string Prev { get; set; }

        public string First { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}