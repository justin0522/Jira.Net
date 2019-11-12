using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class ProjectType
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "formattedkey")]
        public string FormattedKey { get; set; }
        [DataMember(Name = "color")]
        public string Color { get; set; }
        [DataMember(Name = "description|i18nKey")]
        public string Description { get; set; }
        [DataMember(Name = "icon")]
        public string icon { get; set; }
    }
}
