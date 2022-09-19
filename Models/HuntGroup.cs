using System.Collections.Generic;

namespace SparkDotNet.Models
{
    public class HuntGroup
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string LocationName { get; set; }

        public string LocationId { get; set; }

        public bool Enabled { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class HuntGroupList
    {
        public List<HuntGroup> HuntGroups { get; set; }
    }
}