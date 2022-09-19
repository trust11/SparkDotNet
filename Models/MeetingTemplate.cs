namespace SparkDotNet.Models
{
    public class MeetingTemplate : WebexObject
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Locale { get; set; }

        public string SiteUrl { get; set; }

        public string TemplateType { get; set; }

        public bool IsDefault { get; set; }

        public bool IsStandard { get; set; }
    }
}