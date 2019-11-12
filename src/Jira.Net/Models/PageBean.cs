using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class PageBean<T> where T : new()
    {
        [DataMember(Name = "maxResults")]
        public int? MaxResults { get; set; }
        [DataMember(Name = "startAt")]
        public int? StartAt { get; set; }
        [DataMember(Name = "total")]
        public int? Total { get; set; }
        [DataMember(Name = "isLast")]
        public bool? IsLast { get; set; }
        [DataMember(Name = "values")]
        public List<T> Values { get; set; }
    }
}
