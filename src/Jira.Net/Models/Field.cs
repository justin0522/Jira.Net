using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Field
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "custom")]
        public bool? Custom { get; set; }
        [DataMember(Name = "orderable")]
        public bool? Orderable { get; set; }
        [DataMember(Name = "navigable")]
        public bool? Navigable { get; set; }
        [DataMember(Name = "searchable")]
        public bool? Searchable { get; set; }
        [DataMember(Name = "clauseNames")]
        public List<string> ClauseNames { get; set; }
        [DataMember(Name = "schema")]
        public FieldSchema Schema { get; set; }
    }

    [Serializable]
    [DataContract]
    public class FieldSchema
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "custom")]
        public string Custom { get; set; }
        [DataMember(Name = "customId")]
        public int CustomID { get; set; }
    }
}
