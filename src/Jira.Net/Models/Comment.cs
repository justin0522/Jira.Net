using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Comment
    {
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "author")]
        public User Author { get; set; }
        [DataMember(Name = "body")]
        public string Body { get; set; }
        [DataMember(Name = "updateAuthor")]
        public User UpdateAuthor { get; set; }
        [DataMember(Name = "created")]
        public DateTime Created { get; set; }
        [DataMember(Name = "updated")]
        public DateTime Updated { get; set; }

        //visibility

    }

    public class CommentContainer
    {
        [DataMember(Name = "maxResults")]
        public int? MaxResults { get; set; }
        [DataMember(Name = "startAt")]
        public int? StartAt { get; set; }
        [DataMember(Name = "total")]
        public int? Total { get; set; }
        [DataMember(Name = "comments")]
        public List<Comment> Comments { get; set; }
    }
}
