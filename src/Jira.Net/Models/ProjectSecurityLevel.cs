using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    class ProjectSecurityLevel
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }

    [Serializable]
    [DataContract]
    class ProjectSecurityLevelCollection
    {
        [DataMember(Name = "levels")]
        public List<ProjectSecurityLevel> Levels { get; set; }
    }
}
