using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class IssueType
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "iconUrl")]
        public string IconUrl { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "subtask")]
        public bool? Subtask { get; set; }
        [DataMember(Name = "avatarId")]
        public int? AvatarId { get; set; }
        //scope
        [DataMember(Name = "statuses")]
        public List<Status> Statuses { get; set; }
    }
}
