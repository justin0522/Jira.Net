﻿using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Resolution
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
    }
}
