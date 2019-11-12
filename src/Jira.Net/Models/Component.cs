using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Component
    {
        [NonSerialized]
        public Project Parent; //{ get; set; }

        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "lead")]
        public User Lead { get; set; }
        [DataMember(Name = "assigneeType")]
        public string AssigneeType { get; set; }
        [DataMember(Name = "assignee")]
        public User Assignee { get; set; }
        [DataMember(Name = "realAssigneeType")]
        public string RealAssigneeType { get; set; }
        [DataMember(Name = "realAssignee")]
        public User RealAssignee { get; set; }
        [DataMember(Name = "isAssigneeTypeValid")]
        public bool? IsAssigneeTypeValidisAssigneeTypeValid { get; set; }
        [DataMember(Name = "project")]
        public string Project { get; set; }
        [DataMember(Name = "projectId")]
        public int? ProjectId { get; set; }
        
    }
}
