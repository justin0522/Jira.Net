using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Priority
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "statusColor")]
        public string StatusColor { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "iconUrliconUrl")]
        public string IconUrl { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
    }
}
