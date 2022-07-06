using Newtonsoft.Json;
using SparkDotNet.Models;

namespace Extensions
{
    public static class Extensions
    {
        public static T Clone<T>(this T p)
        {
            var json = JsonConvert.SerializeObject(p);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
