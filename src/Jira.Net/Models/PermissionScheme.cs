using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class PermissionGrant
    {
        [DataMember(Name = "id")]
        public int? ID { get; set; }
        [DataMember(Name = "self")]
        public string Self { get; set; }
        //holder
        [DataMember(Name = "permission")]
        public string Permission { get; set; }
    }

    [Serializable]
    [DataContract]
    public class PermissionScheme
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public int? ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "permissions")]
        public List<PermissionGrant> Permissions { get; set; }
    }

    [Serializable]
    [DataContract]
    public class PermissionSchemeContainer
    {
        [DataMember(Name = "permissionSchemes")]
        public List<PermissionScheme> PermissionSchemes { get; set; }
    }
}
