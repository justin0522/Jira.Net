using Jira.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.Net
{
    public interface IJiraClient
    {
        string BaseUrl { get; }

        //Group GetGroup(string groupName);

        bool CreateProject(CreateProject newProject);
        bool UpdateProject(CreateProject existingProject);
        List<Project> GetProjects();
        Project GetProject(string projectKey);

        List<ProjectCategory> GetProjectCategories();
        List<ProjectType> GetProjectTypes();
        List<ProjectRole> GetProjectRoles(string key);
        ProjectRole AddGroupActor(string projectKey, int id, string group);
        bool DeleteGroupActor(string projectKey, int id, string group);

        List<Field> GetFields();

        List<ProjectVersion> GetProjectVersions(string projectKey);

        //List<Component> GetProjectComponents(string projectKey);

        //User GetUser(string username);
        List<User> GetAssignableUsers(string projectKey);

        //List<AgileBoard> GetAgileBoards();
        List<Sprint> GetSprintsFromAgileBoard(int agileBoardID);
        List<Sprint> GetBacklogSprintsFromAgileBoard(int agileBoardID);
        ProjectCategory CreateProjectCategory(string Name, string Description);
        Sprint GetSprint(int agileBoardID, int sprintID);
        IssueSearchResults GetIssuesFromSprint(int sprintID);

        Issue GetIssue(string key);
        IssueSearchResults SearchIssues(string jql, int maxResults);
        //List<IssueType> GetIssueTypes();

        Issue AddIssue(IssueFields fields);

        string UpdateIssueSummary(Issue issue, string summary);
        void TransitionIssue(Issue issue, Transition transition, Comment comment);

        IssueSearchResults GetIssuesFromProjectVersion(string projectKey, string projectVersionName);
        IssueSearchResults GetSubtasksFromIssue(string issueKey);
        WorklogSearchResult GetWorkLogs(string issueKey);
        List<Transition> GetTransitions(string issueKey);

        IssueSearchResults GetEpicIssuesFromProject(string projectName);
        Issue GetEpicIssueFromProject(string projectName, string epicName);

        IssueSearchResults GetIssuesWithEpicLink(string epicLink);

        List<Filter> GetFavoriteFilters();

        //List<IssueSecurityScheme> GetIssueSecuritySchemes();

             
        #region Attachment
        Attachment AddAttachment(string issueKey, string fileName, string filePath);
        Attachment GetAttachment(string id);
        byte[] DownloadAttachment(string id);
        #endregion

        #region Comment
        Comment AddComment(string issueKey, Comment comment);
        Comment GetComment(string issueKey, string commentId);
        CommentContainer GetAllComments(string issueKey);
        Comment UpdateComment(string issueKey, Comment comment);
        void DeleteComment(string issueKey, string commentId);
        #endregion

        #region Common Methods
        
        T GetItem<T>(JiraObjectEnum objectType, Dictionary<string, string> parameters = null, Dictionary<string, string> keys = null) where T : new();

        List<T> GetList<T>(JiraObjectEnum objectType, Dictionary<string, string> parameters = null, Dictionary<string, string> keys = null) where T : new();

        T CreateItem<T>(JiraObjectEnum objectType, T obj, Dictionary<string, string> parameters = null, Dictionary<string, string> keys = null) where T : new();

        T GetItem<T>(string url) where T : new();
        
        #endregion
    }
}
