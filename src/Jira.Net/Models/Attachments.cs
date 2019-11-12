using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Attachment
    {
        [NonSerialized]
        public Issue Parent; //{ get; set; }

        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "filename")]
        public string FileName { get; set; }
        [DataMember(Name = "author")]
        public User Author { get; set; }
        [DataMember(Name = "created")]
        public DateTime Created { get; set; }
        [DataMember(Name = "size")]
        public int Size { get; set; }
        [DataMember(Name = "mimeType")]
        public string MimeType { get; set; }
        [DataMember(Name = "content")]
        public string Content { get; set; }
        [DataMember(Name = "thumbnail")]
        public string Thumbnail { get; set; }

    }

    [Serializable]
    [DataContract]
    public class AttachmentSettings
    {
        [DataMember(Name = "enabled")]
        public bool? Enabled { get; set; }
        [DataMember(Name = "uploadLimit")]
        public int? UploadLimit { get; set; }
    }
}
