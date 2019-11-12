using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class IssueSecurityScheme
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public int? Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "Description")]
        public string Description { get; set; }
        [DataMember(Name = "defaultSecurityLevelId")]
        public int? DefaultSecurityLevelId { get; set; }
    }

    [Serializable]
    [DataContract]
    public class IssueSecuritySchemeContainer
    {
        [DataMember(Name = "issueSecuritySchemes")]
        public List<IssueSecurityScheme> IssueSecuritySchemes { get; set; }
    }
}
