using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class IssueSearchResults
    {
        [DataMember(Name = "expand")]
        public string Expand { get; set; }
        [DataMember(Name = "startAt")]
        public int? StartAt { get; set; }
        [DataMember(Name = "maxResults")]
        public int? MaxResults { get; set; }
        [DataMember(Name = "total")]
        public int? Total { get; set; }
        [DataMember(Name = "issues")]
        public List<Issue> Issues { get; set; }
        [DataMember(Name = "warningMessages")]
        public List<string> WarningMessages { get; set; }
    }
}
