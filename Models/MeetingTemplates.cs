using Newtonsoft.Json;
using System.Collections.Generic;

namespace SparkDotNet.Models
{
    public class MeetingTemplates:WebexObject
    {
        [JsonProperty("items")]
        public List<MeetingTemplate> MeetingTemplateList { get; set; }
    }
}