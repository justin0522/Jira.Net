using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Status
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "iconUrl")]
        public string IconUrl { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "statusCategory")]
        public StatusCategory StatusCategory { get; set; }
    }

    [Serializable]
    [DataContract]
    public class StatusCategory
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public int? ID { get; set; }
        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "colorName")]
        public string ColorName { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
