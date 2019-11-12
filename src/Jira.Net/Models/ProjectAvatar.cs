using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    class ProjectAvatar
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "isSystemAvatar")]
        public bool? IsSystemAvatar { get; set; }
        [DataMember(Name = "isSelected")]
        public bool? IsSelected { get; set; }
        [DataMember(Name = "isDeletable")]
        public bool? IsDeletable { get; set; }
        [DataMember(Name = "urls")]
        public Dictionary<string, string> Urls { get; set; }
    }

    [Serializable]
    [DataContract]
    class ProjectAvatarCollection
    {
        [DataMember(Name = "system")]
        public List<ProjectAvatar> System { get; set; }
        [DataMember(Name = "custom")]
        public List<ProjectAvatar> Custom { get; set; }
    }
}
