using System;
using System.Collections.Generic;

namespace SparkDotNet.Models
{
    public class GroupsOverview : WebexObject
    {
        public int TotalResults { get; set; }
        public int StartIndex { get; set; }
        public int ItemsPerPage { get; set; }
        public List<Group> Groups { get; set; }
    }
    public class Group : WebexObject
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string OrgId { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastModified { get; set; }
        public int MemberSize { get; set; }
        public List<Member> Members { get; set; }
    }

    public class Member : WebexObject
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
    }

}
