using Jira.Net.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Jira.Net
{
    public enum JiraObjectEnum
    {
        Fields,
        Projects,
        Project,
        AssignableUser,
        ProjectVersions,
        Issue,
        Issues,
        IssueSearch,
        Worklog,
        User,
        Group,
        AgileBoards,
        Sprints,
        BacklogSprints,
        Sprint,
        SprintIssues,
        Filter,
        MyFilters,
        FavouriteFilters,
        Transitions,
        ProjectComponents,
        IssueSecuritySchemes,
        PermissionScheme,
        NotificationScheme,
        ProjectRoles,
        ProjectRole,
        ProjectCategories,
        ProjectTypes,
        Attachments,
        AttachmentsSetting,
        IssueType,
        Comment,
        CommentId,
        NavigatorColumns,
        ServerInfo
    }

    public class JiraClient : IJiraClient
    {

        private RestClient Client { get; set; }

        private const string JiraAPIServiceURI = "/rest/api/2";
        private const string JiraAgileServiceURI = "/rest/greenhopper/latest";

        private Dictionary<JiraObjectEnum, string> _methods = new Dictionary<JiraObjectEnum, string>()
        {
            {JiraObjectEnum.Fields, string.Format("{0}/field/", JiraAPIServiceURI)},
            {JiraObjectEnum.Projects, string.Format("{0}/project/", JiraAPIServiceURI)},
            {JiraObjectEnum.Project, string.Format("{0}/project/{{projectKey}}/", JiraAPIServiceURI)},
            {JiraObjectEnum.ProjectVersions, string.Format("{0}/project/{{projectKey}}/versions/", JiraAPIServiceURI)},
            {JiraObjectEnum.AssignableUser, string.Format("{0}/user/assignable/search/", JiraAPIServiceURI)},
            {JiraObjectEnum.Issue, string.Format("{0}/issue/{{issueKey}}/", JiraAPIServiceURI)},
            {JiraObjectEnum.Issues, string.Format("{0}/issue/", JiraAPIServiceURI)},
            {JiraObjectEnum.IssueSearch, string.Format("{0}/search/", JiraAPIServiceURI)},
            {JiraObjectEnum.Worklog, string.Format("{0}/issue/{{issueKey}}/worklog/", JiraAPIServiceURI)},
            {JiraObjectEnum.Transitions, string.Format("{0}/issue/{{issueKey}}/transitions/", JiraAPIServiceURI)},
            {JiraObjectEnum.User, string.Format("{0}/user/", JiraAPIServiceURI)},
            {JiraObjectEnum.Group, string.Format("{0}/group", JiraAPIServiceURI) },
            {JiraObjectEnum.Filter, string.Format("{0}/filter", JiraAPIServiceURI)},
            {JiraObjectEnum.MyFilters, string.Format("{0}/filter/my", JiraAPIServiceURI)},
            {JiraObjectEnum.FavouriteFilters, string.Format("{0}/filter/favourite", JiraAPIServiceURI)},
            {JiraObjectEnum.ProjectComponents, string.Format("{0}/project/{{projectKey}}/components/", JiraAPIServiceURI)},
            {JiraObjectEnum.IssueSecuritySchemes, string.Format("{0}/issuesecurityschemes/", JiraAPIServiceURI)},
            {JiraObjectEnum.PermissionScheme, string.Format("{0}/permissionscheme/", JiraAPIServiceURI)},
            {JiraObjectEnum.NotificationScheme, string.Format("{0}/notificationscheme/", JiraAPIServiceURI)},
            {JiraObjectEnum.ProjectRoles, string.Format("{0}/project/{{projectKey}}/role/", JiraAPIServiceURI)},
            {JiraObjectEnum.ProjectRole, string.Format("{0}/project/{{projectKey}}/role/{{id}}", JiraAPIServiceURI)},
            {JiraObjectEnum.ProjectCategories, string.Format("{0}/projectCategory", JiraAPIServiceURI)},
            {JiraObjectEnum.ProjectTypes, string.Format("{0}/project/type", JiraAPIServiceURI)},
            {JiraObjectEnum.Attachments, string.Format("{0}/attachment/{{id}}", JiraAPIServiceURI) },

            {JiraObjectEnum.IssueType, string.Format("{0}/issuetype/", JiraAPIServiceURI) },
            {JiraObjectEnum.Comment, string.Format("{0}/issue/{{issueKey}}/comment/", JiraAPIServiceURI) },
            {JiraObjectEnum.CommentId, string.Format("{0}/issue/{{issueKey}}/comment/{{id}}", JiraAPIServiceURI) },
            #region settings
            {JiraObjectEnum.AttachmentsSetting, string.Format("{0}/attachment/meta", JiraAPIServiceURI) },
            {JiraObjectEnum.NavigatorColumns, string.Format("{0}/settings/columns", JiraAPIServiceURI) },
            {JiraObjectEnum.ServerInfo, string.Format("{0}/serverInfo/", JiraAPIServiceURI) },
            #endregion
            #region Agile Service
            {JiraObjectEnum.AgileBoards, string.Format("{0}/rapidviews/list/", JiraAgileServiceURI)},
            {JiraObjectEnum.Sprints, string.Format("{0}/sprintquery/{{boardID}}/", JiraAgileServiceURI)},
            {JiraObjectEnum.BacklogSprints, string.Format("{0}/xboard/plan/backlog/data.json", JiraAgileServiceURI)},
            {JiraObjectEnum.Sprint, string.Format("{0}/rapid/charts/sprintreport/", JiraAgileServiceURI)},
            {JiraObjectEnum.SprintIssues, string.Format("{0}/sprintquery/", JiraAgileServiceURI)},
            #endregion 
        };

        public JiraClient(RestClient client)
        {
            Client = client;
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                return true;
            };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public JiraClient(string url)
        {
            Client = new RestClient(url);
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                return true;
            };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        }

        public JiraClient(string url, string username, string password)
        {
            Client = new RestClient(url)
            {
                Authenticator = new HttpBasicAuthenticator(username, password)
            };
            ServicePointManager.ServerCertificateValidationCallback = delegate (object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
            {
                return true;
            };
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            Client.AddHandler("application/json", () => new RestSharpJsonNetSerializer());
        }

        public string BaseUrl => Client.BaseUrl.ToString();

        public IssueSearchResults SearchIssues(string jql, int maxResults = 700)
        {
            //return GetIssues(_methods[JiraObjectEnum.Issues], new Dictionary<string, string>() { { "jql", jql }, { "maxResults", maxResults.ToString() }, { "fields", "*all" }, { "expand", "transitions" } });
            return GetItem<IssueSearchResults>(JiraObjectEnum.IssueSearch, new Dictionary<string, string>() { { "jql", jql }, { "maxResults", maxResults.ToString() }, { "fields", "*all" }, { "expand", "transitions" } });
        }

        #region Fields
        public List<Field> GetFields()
        {
            return GetList<Field>(JiraObjectEnum.Fields);
        }
        #endregion

        #region Projects
        public bool CreateProject(CreateProject newProject)
        {
            var request = GetRequest(_methods[JiraObjectEnum.Projects], Method.POST, new Dictionary<string, string>(), new Dictionary<string, string>());
            //request.Method = Method.POST;
            request.AddJsonBody(newProject);
            var response = this.Client.Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.Created;
        }

        /// <summary></summary>
        /// <param name="existingProject">Any field left as NULL will not be updated. Key must be set.</param>
        /// <returns>True on success</returns>
        public bool UpdateProject(CreateProject existingProject)
        {
            if (string.IsNullOrWhiteSpace(existingProject.Key))
                throw new ArgumentOutOfRangeException("Project key not set");
            var request = GetRequest(_methods[JiraObjectEnum.Project], Method.PUT, new Dictionary<string, string>(), new Dictionary<string, string>() { { "projectKey", existingProject.Key } });
            //request.Method = Method.PUT;
            request.AddJsonBody(existingProject);
            var response = this.Client.Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }

        public ProjectCategory CreateProjectCategory(string Name, string Description)
        {
            var request = GetRequest(_methods[JiraObjectEnum.ProjectCategories], Method.POST, new Dictionary<string, string>(), new Dictionary<string, string>());
            //request.Method = Method.POST;
            request.AddJsonBody(new { name = Name, description = Description });
            var response = this.Client.Execute<ProjectCategory>(request);
            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return response.Data;
            else
                return null;
        }

        public List<Project> GetProjects()
        {
            return GetList<Project>(JiraObjectEnum.Projects);
        }

        public Project GetProject(string projectKey)
        {
            return GetItem<Project>(JiraObjectEnum.Project, keys: new Dictionary<string, string>() { { "projectKey", projectKey } });
        }

        public List<ProjectCategory> GetProjectCategories()
        {
            return GetList<ProjectCategory>(JiraObjectEnum.ProjectCategories);
        }

        public List<ProjectType> GetProjectTypes()
        {
            return GetList<ProjectType>(JiraObjectEnum.ProjectTypes);
        }
        #endregion

        #region Security
        //private class hiddenIssueSecuritySchemes
        //{
        //    public List<IssueSecurityScheme> IssueSecuritySchemes { get; set; }
        //}
        //public List<IssueSecurityScheme> GetIssueSecuritySchemes()
        //{
        //    return GetItem<hiddenIssueSecuritySchemes>(JiraObjectEnum.IssueSecuritySchemes).IssueSecuritySchemes;
        //}

        public class hiddenPermissionSchemes
        {
            public List<PermissionScheme> PermissionSchemes { get; set; }
        }
        
        private class hiddenNotificationScheme
        {
            public int MaxResults { get; set; }
            public int StartAt { get; set; }
            public int Total { get; set; }
            public bool IsLast { get; set; }
            public List<NotificationScheme> values { get; set; }
        }
        
        #endregion

        #region Project versions
        public List<ProjectVersion> GetProjectVersions(string projectKey)
        {
            return GetList<ProjectVersion>(JiraObjectEnum.ProjectVersions,
                               keys: new Dictionary<string, string>() { { "projectKey", projectKey } });
        }
        #endregion

        #region Project components
        //public List<Component> GetProjectComponents(string projectKey)
        //{
        //    return GetList<Component>(JiraObjectEnum.ProjectComponents,
        //                       keys: new Dictionary<string, string>() { { "projectKey", projectKey } });
        //}
        #endregion

        #region Project roles
        public List<ProjectRole> GetProjectRoles(string projectKey)
        {
            var responses = this.Execute<Dictionary<string, string>>(JiraObjectEnum.ProjectRoles, Method.GET, null, keys: new Dictionary<string, string>() { { "projectKey", projectKey } });
            var roles = new List<ProjectRole>();
            foreach (var response in responses)
                roles.Add(new ProjectRole
                {
                    Name = response.Key,
                    Self = response.Value,
                    ID = int.Parse(response.Value.Split('/').Last())
                });
            return roles;
        }

        public ProjectRole AddGroupActor(string projectKey, int id, string group)
        {
            var request = this.GetRequest(_methods[JiraObjectEnum.ProjectRole], Method.POST,
                new Dictionary<string, string>(),
                new Dictionary<string, string>() { { "projectKey", projectKey }, { "id", id.ToString() } });
            //request.Method = Method.POST;
            request.AddJsonBody(new { group = new List<string> { group } });
            var response = Client.Execute<ProjectRole>(request);
            return response.Data;
        }

        public bool DeleteGroupActor(string projectKey, int id, string group)
        {
            var request = this.GetRequest(_methods[JiraObjectEnum.ProjectRole], Method.DELETE,
                new Dictionary<string, string>() { { "group", group } },
                new Dictionary<string, string>() { { "projectKey", projectKey }, { "id", id.ToString() } });
            //request.Method = Method.DELETE;
            var response = Client.Execute<ProjectRole>(request);
            return response.StatusCode == System.Net.HttpStatusCode.NoContent;
        }
        #endregion

        #region Agile boards
        public List<AgileBoard> GetAgileBoards()
        {
            return GetItem<AgileBoardView>(JiraObjectEnum.AgileBoards).Views;
        }

        public List<Sprint> GetSprintsFromAgileBoard(int agileBoardID)
        {
            return GetItem<SprintResult>(JiraObjectEnum.Sprints, keys: new Dictionary<string, string>() { { "boardID", agileBoardID.ToString() } }).Sprints;
        }

        public List<Sprint> GetBacklogSprintsFromAgileBoard(int agileBoardID)
        {
            return GetItem<SprintResult>(JiraObjectEnum.BacklogSprints, parameters: new Dictionary<string, string>() { { "rapidViewId", agileBoardID.ToString() } }).Sprints;
        }

        public Sprint GetSprint(int agileBoardID, int sprintID)
        {
            return GetItem<SprintResult>(JiraObjectEnum.Sprint, parameters: new Dictionary<string, string>() { { "rapidViewId", agileBoardID.ToString() }, { "sprintId", sprintID.ToString() } }).Sprint;
        }

        public IssueSearchResults GetIssuesFromSprint(int sprintID)
        {
            return SearchIssues(string.Format("Sprint = {0}", sprintID));
        }
        #endregion

        #region Issues
        public Issue GetIssue(string key)
        {
            //return GetIssue(_methods[JiraObjectEnum.Issue], keys: new Dictionary<string, string>() { { "issueKey", key } });
            return GetItem<Issue>(JiraObjectEnum.Issue, null, new Dictionary<string, string>() { { "issueKey", key } });
        }

        public IssueSearchResults GetSubtasksFromIssue(string issueKey)
        {
            return SearchIssues(string.Format("parent=\"{0}\"", issueKey));
        }

        public IssueSearchResults GetIssuesFromProjectVersion(string projectKey, string projectVersionName)
        {
            return SearchIssues(string.Format("project=\"{0}\"&fixversion=\"{1}\"",
                            projectKey, projectVersionName));
        }

        public List<Filter> GetFavoriteFilters()
        {
            return GetList<Filter>(JiraObjectEnum.FavouriteFilters);
        }

        public List<Filter> GetMyFilters()
        {
            return GetList<Filter>(JiraObjectEnum.MyFilters);
        }

        public IssueSearchResults GetIssuesWithEpicLink(string epicLink)
        {
            return SearchIssues(string.Format("'Epic Link' = {0}", epicLink));
        }

        public IssueSearchResults GetEpicIssuesFromProject(string projectName)
        {
            return SearchIssues(string.Format("project = '{0}' AND Type = Epic", projectName));
        }

        public Issue GetEpicIssueFromProject(string projectName, string epicName)
        {
            return SearchIssues(string.Format("project = '{0}' AND Type = Epic and 'Epic Name' = '{1}'", projectName, epicName)).Issues.FirstOrDefault();
        }

        public Issue AddIssue(IssueFields issueFields)
        {
            IRestRequest request = new RestRequest(string.Format("{0}/issue", JiraAPIServiceURI), Method.POST);
            request.RequestFormat = DataFormat.Json;

            Issue issue = new Issue() { Fields = issueFields };

            List<Field> fields = GetFields();

            //TODO
            //foreach (KeyValuePair<string, CustomField> customfield in issueFields.CustomFields)
            //{
            //    Field field = fields.Where(f => f.ID.Equals(customfield.Key)).FirstOrDefault();

            //    switch (field.Schema.Custom)
            //    {
            //        case "com.atlassian.jira.plugin.system.customfieldtypes:select":
            //            tempjson.Add(customfield.Key.ToLower(), JToken.FromObject(new { value = customfield.Value.Value }));
            //            break;
            //        default:
            //            tempjson.Add(customfield.Key.ToLower(), customfield.Value.Value);
            //            break;
            //    }
            //}

            request.RequestFormat = DataFormat.Json;
            request.AddBody(issue);

            IRestResponse<Issue> response = Client.Post<Issue>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }

            return response.Data;
        }

        public Attachment AddAttachment(string issueKey, string fileName, string filePath)
        {
            byte[] fileContent = System.IO.File.ReadAllBytes(filePath);
            IRestRequest request = new RestRequest(string.Format("{0}/issue/{1}/attachments", JiraAPIServiceURI, issueKey), Method.POST);
            request.AddHeader("X-Atlassian-Token", "nocheck");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddFileBytes("file", fileContent, fileName, "application/octet-stream");
            //IRestResponse<Attachment> response = Client.Post<List<Attachment>>(request);
            var response = Client.Execute<List<Attachment>>(request);
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }

            return response.Data.FirstOrDefault();
        }

        public Attachment GetAttachment(string id)
        {
            return GetItem<Attachment>(JiraObjectEnum.Attachments, keys: new Dictionary<string, string> { { "id", id } });
        }

        public byte[] DownloadAttachment(string id)
        {
            var attach = GetAttachment(id);
            IRestRequest request = new RestRequest(attach.Content);
            var data = Client.DownloadData(request);
            return data;
        }

        public Comment AddComment(string issueKey, Comment comment)
        {
            IRestRequest request = new RestRequest(string.Format("{0}/issue/{1}/comment", JiraAPIServiceURI, issueKey), Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { body = comment.Body });

            IRestResponse<Comment> response = Client.Post<Comment>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }

            return response.Data;
        }

        public Comment GetComment(string issueKey, string commentId)
        {
            return GetItem<Comment>(JiraObjectEnum.CommentId, keys: new Dictionary<string, string> { { "issueKey", issueKey }, { "id", commentId } });
        }

        public CommentContainer GetAllComments(string issueKey)
        {
            return GetItem<CommentContainer>(JiraObjectEnum.Comment, keys: new Dictionary<string, string> { { "issueKey", issueKey } });
        }

        public Comment UpdateComment(string issueKey, Comment comment)
        {
            IRestRequest request = new RestRequest(string.Format("{0}/issue/{1}/comment/{2}", JiraAPIServiceURI, issueKey, comment.ID), Method.PUT);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(new { body = comment.Body });

            IRestResponse<Comment> response = Client.Put<Comment>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }

            return response.Data;
        }

        public void DeleteComment(string issueKey, string commentId)
        {
            IRestRequest request = new RestRequest(string.Format("{0}/issue/{1}/comment/{2}", JiraAPIServiceURI, issueKey, commentId), Method.DELETE);

            request.RequestFormat = DataFormat.Json;

            IRestResponse<Comment> response = Client.Delete<Comment>(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }

            var data = response.Data;
        }

        /// <summary>
        /// Update existing issue summary
        /// </summary>
        /// <param name="issue">Issue to update</param>
        /// <param name="summary">New summary text</param>
        /// <returns></returns>
        public string UpdateIssueSummary(Issue issue, string summary)
        {
            IRestRequest request = new RestRequest(string.Format("{0}/issue/{1}", JiraAPIServiceURI, issue.Key), Method.PUT);

            request.RequestFormat = DataFormat.Json;

            request.AddBody(new
            {
                update = new
                {
                    summary = new[] {
                        new {
                            set = summary
                        }
                    }
                }
            });

            IRestResponse response = Client.Put(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }

            return summary;
        }
        #endregion

        #region Issue Type
        public List<IssueType> GetIssueTypes()
        {
            return GetItem<List<IssueType>>(JiraObjectEnum.IssueType);
        }
        #endregion

        #region Worklog
        public WorklogSearchResult GetWorkLogs(string issueKey)
        {
            return GetItem<WorklogSearchResult>(JiraObjectEnum.Worklog,
                       keys: new Dictionary<string, string>() { { "issueKey", issueKey } });
        }
        #endregion

        #region Transition
        public List<Transition> GetTransitions(string issueKey)
        {
            return GetItem<TransitionContainer>(JiraObjectEnum.Transitions,
                       keys: new Dictionary<string, string>() { { "issueKey", issueKey } }).Transitions;
        }

        public void TransitionIssue(Issue issue, Transition transition, Comment comment)
        {
            IRestRequest request = new RestRequest(string.Format("{0}/issue/{1}/transitions", JiraAPIServiceURI, issue.Key), Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddBody(
                new
                {
                    transition = new
                    {
                        id = transition.ID.ToString()
                    },
                    update = new
                    {
                        comment = new[]{
                            new {
                                add = new {
                                    body = comment.Body
                                }
                            }
                        }
                    }
                });

            IRestResponse response = Client.Post(request);

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }
        }
        #endregion

        #region Jira communication

        public T GetItem<T>(JiraObjectEnum objectType, Dictionary<string, string> parameters = null, Dictionary<string, string> keys = null) where T : new()
        {
            return Execute<T>(objectType, Method.GET, default(T), parameters, keys);
        }

        public List<T> GetList<T>(JiraObjectEnum objectType, Dictionary<string, string> parameters = null, Dictionary<string, string> keys = null) where T : new()
        {
            return Execute<List<T>>(objectType, Method.GET, null, parameters, keys);
        }

        public T CreateItem<T>(JiraObjectEnum objectType, T obj, Dictionary<string, string> parameters = null, Dictionary<string, string> keys = null) where T : new()
        {
            return Execute<T>(objectType, Method.POST, obj, parameters, keys);
        }

        public T UpdateItem<T>(JiraObjectEnum objectType, T obj, Dictionary<string, string> parameters = null, Dictionary<string, string> keys = null) where T : new()
        {
            return Execute<T>(objectType, Method.PUT, obj, parameters, keys);
        }

        public bool DeleteItem(JiraObjectEnum objectType, Dictionary<string, string> parameters = null, Dictionary<string, string> keys = null)
        {
            try
            {
                Execute(objectType, Method.DELETE, true, parameters, keys);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private T Execute<T>(JiraObjectEnum objectType, Method method, T obj, Dictionary<string, string> parameters = null, Dictionary<string, string> keys = null) where T : new()
        {
            if (!_methods.ContainsKey(objectType))
                throw new NotImplementedException();

            IRestRequest request = GetRequest(_methods[objectType], method, parameters ?? new Dictionary<string, string>(), keys ?? new Dictionary<string, string>());
            if (Method.POST == method)
            {
                //request.AddBody(obj);
                string json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                request.AddParameter("Application/Json", json, ParameterType.RequestBody);
            }

            IRestResponse<T> response = Client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(response.Request.Resource);
            }
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }

            return response.Data;
        }

        //public RestRequest GetRequest(JiraObjectEnum objectType, Method method, Dictionary<string, string> parameters, Dictionary<string, string> keys)
        //{
        //    if (!_methods.ContainsKey(objectType))
        //        throw new NotImplementedException();

        //    return GetRequest(_methods[objectType], method, parameters, keys);
        //}

        public RestRequest GetRequest(string url, Method method, Dictionary<string, string> parameters, Dictionary<string, string> keys)
        {
            RestRequest request = new RestRequest(url, method)
            {
                RequestFormat = DataFormat.Json,
                OnBeforeDeserialization = resp => resp.ContentType = "application/json",
                JsonSerializer = new RestSharpJsonNetSerializer()
            };


            foreach (KeyValuePair<string, string> key in keys)
            {
                request.AddParameter(key.Key, key.Value, ParameterType.UrlSegment);
            }

            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                request.AddParameter(parameter.Key, parameter.Value);
            }

            return request;
        }

        public T GetItem<T>(string url) where T : new()
        {
            IRestRequest request = GetRequest(url, Method.GET,  new Dictionary<string, string>(),  new Dictionary<string, string>());
           

            IRestResponse<T> response = Client.Execute<T>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedAccessException(response.Request.Resource);
            }
            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }
            if (response.ResponseStatus != ResponseStatus.Completed)
            {
                throw new Exception(response.ErrorMessage);
            }

            return response.Data;
        }

        //private Issue GetIssue(string url, Dictionary<string, string> parameters = null, Dictionary<string, string> keys = null)
        //{
        //    IRestResponse response = Client.Execute(GetRequest(url, Method.GET, parameters ?? new Dictionary<string, string>(), keys ?? new Dictionary<string, string>()));

        //    if (response.ErrorException != null)
        //    {
        //        throw response.ErrorException;
        //    }
        //    if (response.ResponseStatus != ResponseStatus.Completed)
        //    {
        //        throw new Exception(response.ErrorMessage);
        //    }

        //    return DeserializeIssue(response.Content);
        //}

        //private List<Issue> DeserializeIssues(string json)
        //{
        //    JObject jsonObject = JObject.Parse(json);

        //    return new IssueSearchResult(jsonObject).Issues;
        //}

        //private Issue DeserializeIssue(string json)
        //{
        //    JObject jsonObject = JObject.Parse(json);
        //    if (jsonObject["fields"] == null)
        //    {
        //        return null;
        //    }
        //    return new Issue((string)jsonObject["key"], (JObject)jsonObject["fields"]);
        //}
        #endregion

        #region User and Group
        //public User GetUser(string username)
        //{
        //    return GetItem<User>(JiraObjectEnum.User, new Dictionary<string, string>() { { "username", username } });
        //}

        public List<User> GetAssignableUsers(string projectKey)
        {
            return GetList<User>(JiraObjectEnum.AssignableUser,
                               parameters: new Dictionary<string, string>() { { "project", projectKey } });
        }

        //public Group GetGroup(string groupName)
        //{
        //    return Execute<Group>(JiraObjectEnum.Group, Method.GET, null, parameters: new Dictionary<string, string> { { "groupname", groupName } });
        //}


        #endregion
    }
}
