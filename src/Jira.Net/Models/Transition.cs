using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Transition
    {
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "to")]
        public Status To { get; set; }
        [DataMember(Name = "hasScreen")]
        public bool? HasScreen { get; set; }
        [DataMember(Name = "isGlobal")]
        public bool? IsGlobal { get; set; }
        [DataMember(Name = "isInitial")]
        public bool? IsInitial { get; set; }
        [DataMember(Name = "isConditional")]
        public bool? IsConditional { get; set; }
        //fields

    }

    [Serializable]
    [DataContract]
    public class TransitionContainer
    {
        [DataMember(Name = "transitions")]
        public List<Transition> Transitions { get; set; }
    }
}
