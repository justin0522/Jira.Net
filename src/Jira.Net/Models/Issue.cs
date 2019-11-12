using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.IO;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class Issue
    {
        [DataMember(Name = "expand")]
        public string Expand { get; set; }
        [DataMember(Name = "id")]
        public string ID { get; set; }
        [DataMember(Name = "self")]
        public string Self { get; set; }
        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "fields")]
        public IssueFields Fields { get; set; }

        private Jira _jira { get; set; }
        public Jira GetJira()
        {
            return _jira;
        }

        public void SetJira(Jira jira)
        {
            _jira = jira;
        }

        public Issue() { }

        public void Reload()
        {
            if (string.IsNullOrEmpty(Key))
                throw new ArgumentNullException("issue key can not be null.");
            this.Fields = GetJira().GetIssue(Key).Fields;
        }

        private string _attachmentRootPath = Directory.GetCurrentDirectory();
        public void SetAttachmentRootPath(string path)
        {
            _attachmentRootPath = path;
        }

        public string GetCustomFieldValue(string customFieldName)
        {
            Field field = GetJira().Fields.FirstOrDefault(f => f.Name.Equals(customFieldName));
            if (field == null)
            {
                throw new ArgumentException(string.Format("The field with name {0} does not exist.", customFieldName), customFieldName);
            }
            string fieldId = field.ID;
            return (Fields.CustomFields[fieldId] != null ? Fields.CustomFields[fieldId].Value : "");
        }

        public void SetCustomFieldValue(string customFieldName, string value)
        {
            Field field = GetJira().Fields.FirstOrDefault(f => f.Name.Equals(customFieldName));
            if (field == null)
            {
                throw new ArgumentException(string.Format("The field with name {0} does not exist.", customFieldName), customFieldName);
            }

            if (Fields.CustomFields[field.ID] == null)
            {
                //Fields.CustomFields[field.ID] = new CustomField(value);
            }
            Fields.CustomFields[field.ID].Value = value;
        }

        public string Url
        {
            get
            {
                return string.Format("{0}browse/{1}", _jira.Client.BaseUrl, Key);
            }
        }


        //private List<Issue> _subtasks;
        //public List<Issue> Subtasks
        //{
        //    get
        //    {
        //        if (_subtasks == null)
        //        {
        //            _subtasks = _jira.Client.GetSubtasksFromIssue(this.Key).Issues;
        //            _subtasks.ForEach(subtask => subtask.SetJira(_jira));
        //        }
        //        return _subtasks;
        //    }
        //    set
        //    {
        //        _subtasks = value;
        //    }
        //}

        public List<string> Labels
        {
            get
            {
                return Fields.Labels;
            }
        }

        /// <summary>
        /// Update issue summary
        /// </summary>
        /// <param name="summary">New summary text</param>
        public void UpdateSummary(string summary)
        {
            GetJira().Client.UpdateIssueSummary(this, summary);
            this.Fields.Summary = summary; //TODO reload ?
        }

        private TimeTracking _timeTracking;
        public TimeTracking TimeTracking
        {
            get
            {
                if (_timeTracking == null)
                {
                    if (Fields.TimeTracking != null)
                    {
                        _timeTracking = Fields.TimeTracking;
                    }
                    else
                    {
                        Issue issue = _jira.Client.GetIssue(this.Key);
                        _timeTracking = issue.Fields.TimeTracking;
                    }

                    _timeTracking.Issue = this;
                }
                return _timeTracking;
            }
        }

        private List<Worklog> _worklogs;
        public List<Worklog> GetWorklogs()
        {
            if (_worklogs == null)
            {
                _worklogs = _jira.Client.GetWorkLogs(this.Key).Worklogs;
            }
            return _worklogs;
        }

        public void RefreshWorklogs()
        {
            // this will force the re-loading of any work logs
            _worklogs = _jira.Client.GetWorkLogs(this.Key).Worklogs;
        }

        public List<Transition> Transitions { get; set; }
        private List<Transition> _transitions;
        public List<Transition> GetTransitions()
        {
            if (_transitions == null)
            {
                if (Transitions != null)
                {
                    _transitions = Transitions;
                }
                else
                {
                    _transitions =
                       _jira.Client.GetTransitions(this.Key);
                }
            }
            return _transitions;
        }

        public void TransitionIssue(Transition transition, Comment comment)
        {
            _jira.Client.TransitionIssue(this, transition, comment);
        }

        private Issue _parent;
        public Issue GetParent()
        {
            if (_parent == null && Fields.Parent != null)
            {
                _parent = _jira.GetIssue(Fields.Parent.Key);
                _parent.SetJira(_jira);
            }
            return _parent;
        }

        public User Assignee
        {
            get { return Fields.Assignee ?? User.UndefinedUser; }
            set { Fields.Assignee = value; }
        }

        public User Reporter
        {
            get { return Fields.Reporter ?? User.UndefinedUser; }
            set { Fields.Reporter = value; }
        }

        public DateTime Created
        {
            get { return Fields.Created.Value; }
            set { Fields.Created = value; }
        }
        public DateTime Updated
        {
            get { return Fields.Updated.Value; }
            set { Fields.Updated = value; }
        }

        public DateTime Resolved
        {
            get
            {
                if (Fields.ResolutionDate.Value.CompareTo(DateTime.MinValue) == 0)
                {
                    Fields.ResolutionDate = DateTime.MaxValue;
                }
                return Fields.ResolutionDate.Value;
            }
        }

        private Project _project = null;
        public Project Project
        {
            get
            {
                if (_project == null)
                {
                    _project = this.Fields.Project;
                    _project.SetJira(_jira);
                }
                return _project;
            }
        }

        public Sprint Sprint { get; set; }

        public Dictionary<string, string> CustomFields
        {
            get;
            set;
        }

        //private List<IssueLink> IssueLinks
        //{
        //    get
        //    {
        //        return Fields.IssueLinks;
        //    }
        //    set
        //    {
        //        Fields.IssueLinks = value;
        //    }
        //}

        /// <summary>
        /// This method returns all issues which where cloned from this one.
        /// </summary>
        /// <returns>The list of issues which where cloned from this one</returns>
        //public List<Issue> GetClones()
        //{
        //    var clones = IssueLinks.Where(link => link.Type.ToEnum() == IssueLinkType.IssueLinkTypeEnum.Cloners && link.InwardIssue != null).Select(link => link.InwardIssue).ToList();
        //    var result = loadIssues(clones);

        //    return result;
        //}

        /// <summary>
        /// This method returns all issues which where cloned from this one.
        /// </summary>
        /// <returns>The list of issues which where cloned from this one</returns>
        //public Issue GetClonedIssue()
        //{
        //    var cloned = IssueLinks.Where(link => link.Type.ToEnum() == IssueLinkType.IssueLinkTypeEnum.Cloners && link.OutwardIssue != null).Select(link => link.OutwardIssue).FirstOrDefault();
        //    if (cloned != null)
        //    {
        //        var result = loadIssues(new List<LinkedIssue>() { cloned });
        //        return result.First();
        //    }

        //    return null;
        //}

        /// <summary>
        /// This method returns all issues which are blocking this one.
        /// </summary>
        /// <returns>The list of issues which are blocking this one</returns>
        //public List<Issue> GetBlockingIssues()
        //{
        //    var blockingIssues = IssueLinks.Where(link => link.Type.ToEnum() == IssueLinkType.IssueLinkTypeEnum.Blocks && link.InwardIssue != null).Select(link => link.InwardIssue).ToList();
        //    var result = loadIssues(blockingIssues);

        //    return result;
        //}

        /// <summary>
        /// This method returns all of the issues which are blocked by this one.
        /// </summary>
        /// <returns>The list of issues which are blocked by this one</returns>
        //public List<Issue> GetImpactedIssues()
        //{
        //    var impactedIssues = IssueLinks.Where(link => link.Type.ToEnum() == IssueLinkType.IssueLinkTypeEnum.Blocks && link.OutwardIssue != null).Select(link => link.OutwardIssue).ToList();
        //    var result = loadIssues(impactedIssues);

        //    return result;
        //}

        /// <summary>
        /// This method returns all of the issues which are duplicates from this one.
        /// </summary>
        /// <returns>The list of issues which are duplicates from this one</returns>
        //public List<Issue> GetDuplicateIssues()
        //{
        //    var duplicateIssues = IssueLinks.Where(link => link.Type.ToEnum() == IssueLinkType.IssueLinkTypeEnum.Duplicate).Select(link => (link.InwardIssue != null ? link.InwardIssue : link.OutwardIssue)).ToList();
        //    var result = loadIssues(duplicateIssues);

        //    return result;
        //}

        /// <summary>
        /// This method returns all of the issues which relate to this one.
        /// </summary>
        /// <returns>The list of issues which are relate to this one</returns>
        //public List<Issue> GetRelatedIssues()
        //{
        //    var relatedIssues = IssueLinks.Where(link => link.Type.ToEnum() == IssueLinkType.IssueLinkTypeEnum.Relates).Select(link => (link.InwardIssue != null ? link.InwardIssue : link.OutwardIssue)).ToList();
        //    var result = loadIssues(relatedIssues);

        //    return result;
        //}

        /// <summary>
        /// This method iterates every issue in the issue list and makes sure this issue is loaded and ready for querying.
        /// </summary>
        /// <param name="issues"></param>
        private void loadIssues(List<Issue> issues)
        {
            issues.Where(issue => issue != null).ToList().ForEach(issue =>
            {
                issue.SetJira(this._jira);
                issue.Reload();
            });
        }

        private List<Issue> loadIssues(List<LinkedIssue> issues)
        {
            //issues.Where(issue => issue != null).ToList().ForEach(issue =>
            //{
            //    issue.SetJira(this._jira);
            //    issue.Reload();
            //});
            List<Issue> result = new List<Issue>();
            foreach (var item in issues)
            {
                var newIssue = new Issue() { Key = item.Key };
                newIssue.Reload();
                result.Add(newIssue);
            }
            return result;
        }

        public List<User> LoadWatches()
        {
            if (null == this.Fields.Watches.Watchers &&
                !string.IsNullOrEmpty(this.Fields.Watches.Self))
            {
                this.Fields.Watches = _jira.Client.GetItem<IssueWatchers>(this.Fields.Watches.Self);
            }

            return this.Fields.Watches.Watchers;
        }

        public Attachment AddAttachment(string fileName, string filePath)
        {
            return _jira.Client.AddAttachment(this.Key, fileName, filePath);
        }

        public Attachment GetAttachment(string id)
        {
            return _jira.Client.GetAttachment(id);
        }

        public List<string> DownloadAllAttachments()
        {
            if (string.IsNullOrEmpty(_attachmentRootPath) || !Directory.Exists(_attachmentRootPath))
                throw new DirectoryNotFoundException("Attachment Root Path Not Found");

            string currentIssuePath = Path.Combine(_attachmentRootPath, this.Key);
            if (Directory.Exists(currentIssuePath))
            {
                Directory.Delete(currentIssuePath, true);
            }
            Directory.CreateDirectory(currentIssuePath);

            List<string> paths = new List<string>();
            foreach (var item in this.Fields.Attachments)
            {
                var data = _jira.Client.DownloadAttachment(item.ID);
                var path = Path.Combine(currentIssuePath, item.FileName);
                File.WriteAllBytes(path, data);
                paths.Add(path);
            }
            return paths;
        }

        public CommentContainer Comments { get; set; }

        public void LoadComments()
        {
            Comments = _jira.Client.GetAllComments(this.Key);
        }

        public Comment AddComment(Comment comment)
        {
            return _jira.Client.AddComment(this.Key, comment);
        }

        public Comment GetComment(string commentId)
        {
            return _jira.Client.GetComment(this.Key, commentId);
        }

        public Comment UpdateComment(Comment comment)
        {
            return _jira.Client.UpdateComment(this.Key, comment);
        }

        public void DeleteComment(string commentId)
        {
            _jira.Client.DeleteComment(this.Key, commentId);
        }

        #region Custom fields for Jira Agile
        public int SprintID
        {
            get
            {
                string sprintDescription = GetCustomFieldValue("Sprint");
                if (!string.IsNullOrEmpty(sprintDescription))
                {
                    MatchCollection matches = Regex.Matches(sprintDescription, ",id=(?<SprintID>\\d+)]");
                    int id = -1;

                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            id = int.Parse(match.Groups["SprintID"].Value);
                        }
                    }

                    return id;
                }
                return -1;
            }
        }

        public string SprintNames
        {
            get
            {
                string sprintDescription = GetCustomFieldValue("Sprint");
                if (!string.IsNullOrEmpty(sprintDescription))
                {
                    MatchCollection matches = Regex.Matches(sprintDescription, ",name=(?<SprintName>.*?),");
                    string names = "";

                    foreach (Match match in matches)
                    {
                        if (match.Success)
                        {
                            names += match.Groups["SprintName"].Value;
                            if (match.NextMatch().Success)
                            {
                                names += ", ";
                            }
                        }
                    }

                    return names;
                }
                return "";
            }
        }

        private Epic _epic;
        public Epic Epic
        {
            get
            {
                if (_epic == null && !string.IsNullOrEmpty(GetCustomFieldValue("Epic Link")))
                {
                    Issue issue = GetJira().GetIssue(GetCustomFieldValue("Epic Link"));
                    if (issue != null)
                    {
                        issue.SetJira(GetJira());
                        _epic = Epic.FromIssue(issue);
                    }

                }
                return _epic;
            }
            set
            {
                _epic = value;
            }
        }

        public int Rank
        {
            get
            {
                MatchCollection matches = Regex.Matches(GetCustomFieldValue("Rank"), ",^(?<Rank>\\d+)]");
                int rank = -1;

                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        rank = int.Parse(match.Groups["Rank"].Value);
                    }
                }

                return rank;
            }
            set
            {
                SetCustomFieldValue("Rank", value.ToString());
            }
        }

        public string Severity
        {
            get
            {
                return (GetCustomFieldValue("Severity") != null ? GetCustomFieldValue("Severity") : "");
            }
        }
        #endregion

        #region equality

        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Issue)
                return Key.Equals(((Issue)obj).Key);
            return false;
        }

        #endregion
    }

    [Serializable]
    [DataContract]
    public class IssueFields
    {
        #region details
        [DataMember(Name = "summary")]
        public string Summary { get; set; }
        [DataMember(Name = "issuetype")]
        public IssueType IssueType { get; set; }
        [DataMember(Name = "status")]
        public Status Status { get; set; }
        [DataMember(Name = "components")]
        public List<Component> Components { get; set; }
        [DataMember(Name = "project")]
        public Project Project { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "fixVersions")]
        public List<ProjectVersion> FixVersions { get; set; }
        [DataMember(Name = "versions")]
        public List<ProjectVersion> AffectsVersions { get; set; }
        [DataMember(Name = "priority")]
        public Priority Priority { get; set; }
        [DataMember(Name = "labels")]
        public List<string> Labels { get; set; }
        [DataMember(Name = "resolution")]
        public Resolution Resolution { get; set; }
        #endregion


        [DataMember(Name = "timetracking")]
        public TimeTracking TimeTracking { get; set; }
        [DataMember(Name = "attachment")]
        public List<Attachment> Attachments { get; set; }
        [DataMember(Name = "comment")]
        public CommentContainer Comments { get; set; }

        #region people
        [DataMember(Name = "reporter")]
        public User Reporter { get; set; }
        [DataMember(Name = "assignee")]
        public User Assignee { get; set; }
        [DataMember(Name = "watches")]
        public IssueWatchers Watches { get; set; }
        [DataMember(Name = "creator")]
        public User Creator { get; set; }
        #endregion

        #region dates
        [DataMember(Name = "created")]
        public DateTime? Created { get; set; }
        [DataMember(Name = "updated")]
        public DateTime? Updated { get; set; }
        [DataMember(Name = "duedate")]
        public DateTime? DueDate { get; set; }
        [DataMember(Name = "resolutiondate")]
        public DateTime? ResolutionDate { get; set; }
        #endregion

        #region links
        [DataMember(Name = "parent")]
        public ParentIssue Parent { get; set; }
        [DataMember(Name = "subtasks")]
        public List<Subtask> Subtasks { get; set; }
        [DataMember(Name = "issuelinks")]
        public List<IssueLink> IssueLinks { get; set; }
        #endregion
        public Dictionary<string, CustomField> CustomFields { get; set; }


    }
}
