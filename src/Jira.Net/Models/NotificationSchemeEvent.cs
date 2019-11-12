using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class NotificationSchemeEventEvent
    {
        [DataMember(Name = "id")]
        public int? ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
    }

    [Serializable]
    [DataContract]
    public class NotificationSchemeEventNotification
    {
        [DataMember(Name = "id")]
        public int? ID { get; set; }
        [DataMember(Name = "notificationType")]
        public string NotificationType { get; set; }
        //parameter
        //group
        //expand
    }

    [Serializable]
    [DataContract]
    public class NotificationSchemeEvent
    {
        [DataMember(Name = "event")]
        public NotificationSchemeEventEvent Event { get; set; }
        [DataMember(Name = "notifications")]
        public List<NotificationSchemeEventNotification> Notifications { get; set; }
    }
}
