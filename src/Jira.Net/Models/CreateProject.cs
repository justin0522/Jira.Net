using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class CreateProject
    {
        public CreateProject()
        {
            AssigneeType = "PROJECT_LEAD";
        }

        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "issueSecurityScheme")]
        public int? IssueSecurityScheme { get; set; }
        [DataMember(Name = "permissionScheme")]
        public int? PermissionScheme { get; set; }
        [DataMember(Name = "notificationScheme")]
        public int? NotificationScheme { get; set; }
        [DataMember(Name = "categoryId")]
        public int? CategoryId { get; set; }
        [DataMember(Name = "assigneeType")]
        public string AssigneeType { get; set; }
        [DataMember(Name = "lead")]
        public string Lead { get; set; }
        [DataMember(Name = "projectTypeKey")]
        public string ProjectTypeKey { get; set; }
        [DataMember(Name = "projectTemplateKey")]
        public string ProjectTemplateKey { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "avatarId")]
        public string AvatarId { get; set; }

    }
}