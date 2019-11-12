using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Subtask
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }
    }
}
