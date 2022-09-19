using Newtonsoft.Json;

namespace Extensions
{
    public static class Extensions
    {
        public static T DeepClone<T>(this T p)
        {
            var json = JsonConvert.SerializeObject(p);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}