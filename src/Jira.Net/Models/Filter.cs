using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Filter
    {
        [DataMember(Name = "self")]
        public string Selft { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "owner")]
        public User Owner { get; set; }
        [DataMember(Name = "jql")]
        public string JQL { get; set; }
        [DataMember(Name = "viewUrl")]
        public string ViewUrl { get; set; }
        [DataMember(Name = "searchUrl")]
        public string SearchUrl { get; set; }
        [DataMember(Name = "favourite")]
        public bool Favourite { get; set; }
        [DataMember(Name = "favouritedCount")]
        public int FavouritedCount { get; set; }
        //sharePermissions
        //subscriptions
    }

    //[Serializable]
    //[DataContract]
    //public class FilterCollection
    //{
    //    [DataMember(Name = "self")]
    //    public string Self { get; set; }
    //    [DataMember(Name = "maxResults")]
    //    public int? MaxResults { get; set; }
    //    [DataMember(Name = "startAt")]
    //    public int? StartAt { get; set; }
    //    [DataMember(Name = "total")]
    //    public int? Total { get; set; }
    //    [DataMember(Name = "isLast")]
    //    public bool? IsLast { get; set; }
    //    [DataMember(Name = "values")]
    //    public List<Filter> Values { get; set; }
    //}

}
