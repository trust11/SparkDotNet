using System;
using System.Collections.Generic;

namespace SparkDotNet.Models
{
    public class Group : WebexObject
    {
        public DateTime Created { get; set; }

        public string DisplayName { get; set; }

        public string Id { get; set; }

        public DateTime LastModified { get; set; }

        public List<Member> Members { get; set; }

        public int MemberSize { get; set; }

        public string OrgId { get; set; }
    }

    public class GroupsOverview : WebexObject
    {
        public List<Group> Groups { get; set; }

        public int ItemsPerPage { get; set; }

        public int StartIndex { get; set; }

        public int TotalResults { get; set; }
    }
    public class Member : WebexObject
    {
        public string DisplayName { get; set; }

        public string Id { get; set; }

        public string Type { get; set; }
    }
}