using System.Collections.Generic;

namespace SparkDotNet.Models
{
    public class PersonOutgoingPermissionSettings : WebexObject
    {
        public bool UseCustomEnabled { get; set; }

        public HashSet<PersonOutgoingPermissionCallingPermission> CallingPermissions { get; set; }
    }
}