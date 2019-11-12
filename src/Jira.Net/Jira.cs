using Jira.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jira.Net
{
    public class Jira
    {
        internal IJiraClient Client { get; private set; }

        public List<Field> Fields { get; private set; }

        public void Connect(IJiraClient client)
        {
            Client = client;
            Fields = Client.GetFields();
        }

        public void Connect(string url)
        {
            Connect(new JiraClient(url));
        }

        public void Connect(string url, string username, string password)
        {
            Connect(new JiraClient(url, username, password));
        }

        public User GetUser(string username)
        {            
            return Client.GetItem<User>(JiraObjectEnum.User, new Dictionary<string, string>() { { "username", username } });
        }

        public bool CreateProject(CreateProject newProject)
        {
            return Client.CreateProject(newProject);
        }

        public bool UpdateProject(CreateProject existingProject)
        {
            return Client.UpdateProject(existingProject);
        }

        public ProjectCategory CreateProjectCategory(string Name, string Description)
        {
            return Client.CreateProjectCategory(Name, Description);
        }

        public Group GetGroup(string groupName)
        {            
            return Client.GetItem<Group>(JiraObjectEnum.Group, new Dictionary<string, string> { { "groupname", groupName } });
        }

        public List<Project> GetProjects()
        {
            List<Project> projects = Client.GetProjects();
            projects.ForEach(project => project.SetJira(this));
            return projects;
        }

        public Project GetProject(string key)
        {
            Project project = Client.GetProject(key);
            if (project != null)
            {
                project.SetJira(this);
            }
            return project;
        }

        public List<ProjectCategory> GetProjectCategories()
        {
            var categories = Client.GetProjectCategories();            
            return categories;
        }

        public List<ProjectRole> GetProjectRoles(string key)
        {
            var roles = Client.GetProjectRoles(key);           
            return roles;
        }

        public ProjectRole AddGroupActor(string projectKey, int id, string group)
        {
            var projectRole = Client.AddGroupActor(projectKey, id, group);           
            return projectRole;
        }

        public bool DeleteGroupActor(string projectKey, int id, string group)
        {
            return Client.DeleteGroupActor(projectKey, id, group);
        }

        public List<ProjectType> GetProjectTypes()
        {
            var types = Client.GetProjectTypes();
            return types;
        }

        public Issue GetIssue(string key)
        {
            Issue issue = Client.GetItem<Issue>(JiraObjectEnum.Issue, null, new Dictionary<string, string>() { { "issueKey", key } });

            if (issue != null)
            {
                issue.SetJira(this);
            }

            return issue;
        }

        public List<Issue> SearchIssues(string jql, int maxResults = 500)
        {
            List<Issue> issues = Client.SearchIssues(jql, maxResults).Issues;
            issues.ForEach(issue => issue.SetJira(this));
            return issues;
        }

        public List<IssueType> GetIssueTypes()
        {
            var types = Client.GetList<IssueType>(JiraObjectEnum.IssueType);
            return types;
        }

        private List<Filter> _favoriteFilters;
        public List<Filter> GetFavoriteFilters()
        {
            _favoriteFilters = Client.GetFavoriteFilters();            
            return _favoriteFilters;
        }

        public Filter GetFilter(string filtername)
        {
            if (null == _favoriteFilters)
            {
                GetFavoriteFilters();
            }
            Filter filter = _favoriteFilters.Where(f => f.Name.Equals(filtername)).FirstOrDefault();
            if (null == filter)
            {
                throw new Exception(string.Format("The filter with name {0} does not exist", filtername));
            }
            return filter;
        }

        public List<IssueSecurityScheme> GetIssueSecuritySchemes()
        {
            var schemes = Client.GetItem<IssueSecuritySchemeContainer>(JiraObjectEnum.IssueSecuritySchemes).IssueSecuritySchemes;
            return schemes;
        }

        public List<PermissionScheme> GetPermissionSchemes()
        {
            var schemes = Client.GetItem<PermissionSchemeContainer>(JiraObjectEnum.PermissionScheme).PermissionSchemes;
            return schemes;
        }

        public PageBean<NotificationScheme> GetNotificationSchemes()
        {
            var schemes = Client.GetItem<PageBean<NotificationScheme>>(JiraObjectEnum.NotificationScheme);
            return schemes;
        }

        public AttachmentSettings GetAttachmentSettings()
        {
            var settings = Client.GetItem<AttachmentSettings>(JiraObjectEnum.AttachmentsSetting);
            return settings;
        }

        public List<ColumnItem> GetNavigatorColumns()
        {
            var Items = Client.GetList<ColumnItem>(JiraObjectEnum.NavigatorColumns);
            return Items;
        }

        public ServerInfo GetServerInfo()
        {
            var info = Client.GetItem<ServerInfo>(JiraObjectEnum.ServerInfo);
            return info;
        }
    }
}
