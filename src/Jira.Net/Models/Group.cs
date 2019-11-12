using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Group
    {        
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "users")]
        public GroupMembers Users { get; set; }                      
    }

    [Serializable]
    [DataContract]
    public class GroupMembers
    {
        [DataMember(Name = "size")]
        public int Size { get; set; }
        [DataMember(Name = "items")]
        public List<User> Items { get; set; }
        [DataMember(Name = "max-results")]
        public int MaxResults { get; set; }
        [DataMember(Name = "start-index")]
        public int StartIndex { get; set; }
        [DataMember(Name = "end-index")]
        public int EndIndex { get; set; }
    }
}
