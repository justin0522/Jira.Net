using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class ProjectVersion
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "archived")]
        public bool? Archived { get; set; }
        [DataMember(Name = "released")]
        public bool? Released { get; set; }
        [DataMember(Name = "releaseDate")]
        public DateTime? ReleaseDate { get; set; }
        [DataMember(Name = "overDue")]
        public bool? OverDue { get; set; }
        [DataMember(Name = "userReleaseDate")]
        public DateTime? UserReleaseDate { get; set; }
        [DataMember(Name = "projectId")]
        public int? ProjectId { get; set; }

        //TODO
        public Project Project { get; set; }
        public DateTime StartDate { get; set; }
        
    }
}
