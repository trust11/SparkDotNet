using System.Collections.Generic;

namespace SparkDotNet.Models
{
    public class Configured : Default
    {
    }

    public class Default
    {
        public Editability Editability { get; set; }

        public object Value { get; set; }
    }

    public class DeviceConfiguration
    {
        public string DeviceId { get; set; }

        public Dictionary<string, DeviceConfigurationData> Items { get; set; }
    }

    public class DeviceConfigurationData
    {
        public string Source { get; set; }

        public Sources Sources { get; set; }

        public string Value { get; set; }

        public ValueSpace ValueSpace { get; set; }
    }

    public class Editability
    {
        public bool IsEditable { get; set; }

        public string Reason { get; set; }
    }

    public class Sources
    {
        public Configured Configured { get; set; }

        public Default Default { get; set; }
    }

    public class ValueSpace
    {
        public List<string> Enum { get; set; }

        public int Maximum { get; set; }

        public int MaxLength { get; set; }

        public int Minimum { get; set; }

        public int MinLength { get; set; }

        public string Type { get; set; }
    }
}