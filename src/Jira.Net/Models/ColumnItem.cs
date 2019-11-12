using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class ColumnItem
    {
        [DataMember(Name = "label")]
        public string Lable { get; set; }
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
