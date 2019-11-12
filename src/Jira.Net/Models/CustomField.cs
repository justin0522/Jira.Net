using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class CustomField
    {
        [DataMember(Name = "id")]
        public int? ID { get; set; }
        [DataMember(Name = "value")]
        public string Value { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
