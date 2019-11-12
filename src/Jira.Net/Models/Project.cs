using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Project
    {
        private Jira _jira { get; set; }
        public Jira GetJira()
        {
            return _jira;
        }

        public void SetJira(Jira jira)
        {
            _jira = jira;
        }

        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "id")]
        public int? ID { get; set; }
        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "lead")]
        public User Lead { get; set; }
        [DataMember(Name = "components")]
        public List<Component> Components { get; set; }
        [DataMember(Name = "issueTypes")]
        public List<IssueType> IssueTypes { get; set; }
        [DataMember(Name = "url")]
        public string Url { get; set; }
        [DataMember(Name = "email")]
        public string Email { get; set; }
        [DataMember(Name = "assigneeType")]
        public string AssigneeType { get; set; }
        [DataMember(Name = "versions")]
        public List<ProjectVersion> Versions { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "roles")]
        public ProjectRole Roles { get; set; }
        //avatarUrls        
        [DataMember(Name = "projectCategory")]
        public ProjectCategory ProjectCategory { get; set; }
        [DataMember(Name = "simplified")]
        public bool? Simplified { get; set; }
        [DataMember(Name = "style")]
        public string Style { get; set; }
        //properties

        public static bool ValidProjectKey(string key)
        {
            throw new NotImplementedException();
        }

        public static bool ValidProjectName(string name)
        {
            throw new NotImplementedException();
        }
              
        private List<User> _assignableUsers;
        public List<User> AssignableUsers
        {
            get
            {
                return _assignableUsers ?? (_assignableUsers = _jira.Client.GetAssignableUsers(this.Key));
            }
        }

        //private List<ProjectVersion> _projectVersions;
        //public List<ProjectVersion> ProjectVersions
        //{
        //    get
        //    {
        //        if (_projectVersions == null)
        //        {
        //            _projectVersions = _jira.Client.GetProjectVersions(this.Key);

        //            _projectVersions.ForEach(vers => vers.Project = this);
        //        }
        //        return _projectVersions;
        //    }
        //}

        public ProjectVersion PreviousVersion
        {
            get
            {
                return Versions.Where(vers => vers.ReleaseDate.Value.CompareTo(DateTime.Now) <= 0)
                        .OrderByDescending(vers => vers.ReleaseDate)
                        .FirstOrDefault();
            }
        }

        public ProjectVersion CurrentVersion
        {
            get
            {
                return Versions.FirstOrDefault(vers => vers.StartDate.CompareTo(DateTime.Now) <= 0 && vers.ReleaseDate.Value.CompareTo(DateTime.Now) > 0 && !vers.Archived.Value);
            }
        }

        public ProjectVersion NextVersion
        {
            get
            {
                return Versions.Where(vers => vers.StartDate.CompareTo(DateTime.Now) > 0 && vers.ReleaseDate.Value.CompareTo(DateTime.Now) > 0).OrderBy(vers => vers.StartDate).FirstOrDefault();
            }
        }

        public List<Epic> GetEpics()
        {
            List<Issue> epicIssues = _jira.Client.GetEpicIssuesFromProject(this.Name).Issues;
            epicIssues.ForEach(epic => epic.SetJira(this.GetJira()));

            List<Epic> epics = epicIssues.Select(epic => Epic.FromIssue(epic)).ToList();

            return epics.OrderBy(epic => epic.Rank).ToList();
        }

        public Epic GetEpic(string epicName)
        {
            Issue epicIssue = _jira.Client.GetEpicIssueFromProject(this.Name, epicName);
            epicIssue.SetJira(this.GetJira());
            return Epic.FromIssue(epicIssue);
        }

        public Epic GetEpicByKey(string issueKey)
        {
            Issue epicIssue = _jira.Client.GetIssue(issueKey);
            epicIssue.SetJira(this.GetJira());
            return Epic.FromIssue(epicIssue);
        }

        public Issue CreateIssue(Issue issue)
        {
            issue.Fields.Project = new Project()
            {
                ID = this.ID
            };
            //Issue issue = GetJira().Client.AddIssue(fields);
            Issue newIssue = GetJira().Client.CreateItem(JiraObjectEnum.Issues, issue);
            newIssue.SetJira(this.GetJira());
            newIssue.Reload();
            return newIssue;
        }

        public override int GetHashCode()
        {
            return this.Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return (obj is Project) && this.Key.Equals(((Project)obj).Key);
        }
    }
}