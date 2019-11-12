using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class IssueLink
    {
        [DataMember(Name = "id")]
        public int? ID { get; set; }
        [DataMember(Name = "type")]
        public IssueLinkType Type { get; set; }
        [DataMember(Name = "inwardIssue")]
        public LinkedIssue InwardIssue { get; set; }
        [DataMember(Name = "outwardIssue")]
        public LinkedIssue OutwardIssue { get; set; }

        //only for create
        [DataMember(Name = "comment")]
        public Comment Comment { get; set; }
    }

    [Serializable]
    [DataContract]
    public class LinkedIssue
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }

        [DataMember(Name = "key")]
        public string Key { get; set; }


    }
}
