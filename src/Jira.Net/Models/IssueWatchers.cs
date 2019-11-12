using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class IssueWatchers
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "isWatching")]
        public bool? IsWatching { get; set; }
        [DataMember(Name = "watchCount")]
        public int? WatchCount { get; set; }
        [DataMember(Name = "watchers")]
        public List<User> Watchers { get; set; }
    }
}
