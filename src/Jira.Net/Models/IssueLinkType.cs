using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class IssueLinkType
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "inward")]
        public string Inward { get; set; }
        [DataMember(Name = "outward")]
        public string Outward { get; set; }

        public IssueLinkTypeEnum ToEnum()
        {
            return (IssueLinkTypeEnum)Enum.Parse(typeof(IssueLinkTypeEnum), Name);
        }
        public enum IssueLinkTypeEnum
        {
            Cloners,
            Relates,
            Blocks,
            Duplicate,
        }
    }

    [Serializable]
    [DataContract]
    public class IssueLinkTypeCollection
    {
        [DataMember(Name = "issueLinkTypes")]
        public List<IssueType> IssueLinkTypes { get; set; }
    }
}
