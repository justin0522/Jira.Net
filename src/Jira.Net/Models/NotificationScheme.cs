using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class NotificationScheme
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public int? ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "expand")]
        public string Expand { get; set; }
        [DataMember(Name = "notificationSchemeEvents")]
        public List<NotificationSchemeEvent> NotificationSchemeEvents { get; set; }
    }
}
