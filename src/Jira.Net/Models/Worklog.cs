using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Worklog
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "author")]
        public User Author { get; set; }
        [DataMember(Name = "updateAuthor")]
        public User UpdateAuthor { get; set; }
        [DataMember(Name = "comment")]
        public string Comment { get; set; }       
        [DataMember(Name = "updated")]
        public DateTime? Updated { get; set; }
        //visibility
        [DataMember(Name = "started")]
        public DateTime? Started { get; set; }
        [DataMember(Name = "timeSpent")]
        public string TimeSpent { get; set; }
        [DataMember(Name = "timeSpentSeconds")]
        public long TimeSpentSeconds { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "issueId")]
        public string IssueId { get; set; }        
    }

    [Serializable]
    [DataContract]
    public class WorklogSearchResult
    {
        [DataMember(Name = "startAt")]
        public int? StartAt { get; set; }
        [DataMember(Name = "maxResults")]
        public int? MaxResults { get; set; }
        [DataMember(Name = "total")]
        public int? Total { get; set; }
        [DataMember(Name = "worklogs")]
        public List<Worklog> Worklogs { get; set; }
    }
}
