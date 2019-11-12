using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class ProjectRole
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "id")]
        public int? ID { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "actors")]
        public List<ProjectRoleActor> Actors { get; set; }

        //TODO
        //scope
    }

    [Serializable]
    [DataContract]
    public class ProjectRoleActor
    {
        [DataMember(Name = "id")]
        public int? Id { get; set; }
        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "actorGroup")]
        public ProjectRoleActorGroup ActorGroup { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ProjectRoleActorGroup
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }
    }
}
