using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.Net.Models
{
    public class Epic : Issue
    {
        private List<Issue> _issues;
        public List<Issue> Issues
        {
            get
            {
                if (_issues == null)
                {
                    throw new ArgumentException("The issues aren't loaded yet");
                }
                return _issues;
            }
            private set
            {
                _issues = value;
            }
        }

        public List<Sprint> Sprints { private get; set; }

        public string EpicStatus
        {
            get
            {
                return (GetCustomFieldValue("Epic Status") != null ? GetCustomFieldValue("Epic Status") : "");
            }
        }

        public double TimeSpentInSeconds { get; private set; }
        public double EstimateInSeconds { get; private set; }
        public double RemainingEstimateInSeconds { get; private set; }

        public double GetCost(double costPerSecond)
        {
            return Math.Round(TimeSpentInSeconds * costPerSecond, 2);
        }

        public double GetEstimatedCost(double costPerSecond)
        {
            return Math.Round(EstimateInSeconds * costPerSecond, 2);
        }


        public Double GetRemainingCost(Double costPerSecond)
        {
            return Math.Round(RemainingEstimateInSeconds * costPerSecond, 2);
        }

        public void LoadIssues(List<Sprint> sprints)
        {
            IssueSearchResults issues = GetJira().Client.GetIssuesWithEpicLink(this.Key);
            issues.Issues.ForEach(issue => issue.SetJira(GetJira()));

            foreach (Issue issue in issues.Issues)
            {
                issue.Sprint = sprints.Where(sprint => sprint.ID == issue.SprintID).FirstOrDefault();
            }

            LoadIssues(issues.Issues);
        }

        public void LoadIssues()
        {
            LoadIssues(GetJira().Client.GetIssuesWithEpicLink(this.Key).Issues);
            Issues.ForEach(issue => issue.SetJira(this.GetJira()));
        }

        public void LoadIssues(List<Issue> issues)
        {
            Double timeSpent = issues.Sum(issue => issue.TimeTracking.TimeSpentSeconds);
            LoadIssues(issues, timeSpent);
        }

        public void LoadIssues(List<Issue> issues, DateTime worklogStartdate, DateTime worklogEnddate)
        {            
            //Get all worklogs within the reach of the start- and enddate provided in the method.
            List<Worklog> worklogs = issues.SelectMany(issue => issue.GetWorklogs()).ToList();
            worklogs = worklogs.Where(worklog => worklog.Started.Value.CompareTo(worklogStartdate) >= 0 && worklog.Started.Value.CompareTo(worklogEnddate) <= 0).ToList();
            Double timeSpent = worklogs.Sum(worklog => worklog.TimeSpentSeconds);

            LoadIssues(issues, timeSpent);
        }

        private void LoadIssues(List<Issue> issues, double timeSpentInSeconds)
        {
            Issues = issues;

            TimeSpentInSeconds = Issues.Sum(issue => (issue.TimeTracking != null ? issue.TimeTracking.TimeSpentSeconds : 0));
            RemainingEstimateInSeconds = Issues.Sum(issue => (issue.TimeTracking != null ? issue.TimeTracking.RemainingEstimateSeconds : 0));
            EstimateInSeconds = Issues.Sum(issue => (issue.TimeTracking != null ? issue.TimeTracking.OriginalEstimateSeconds : 0));
        }

        public static Epic UndefinedEpic
        {
            get
            {
                return new Epic("NONE", "Issues without feature");
            }
        }

        //The private constructor for undefined epics
        private Epic(string key, IssueFields fields, Jira jira)
        {
            base.Key = key;
            base.Fields = fields;
            base.SetJira(jira);
        }

        private Epic(string key, string summary)
        {
            Key = key;

            Fields = new IssueFields()
            {
                Assignee = User.UndefinedUser,
                Reporter = User.UndefinedUser,
                Summary = "",
                Created = DateTime.MinValue,
                Updated = DateTime.MinValue,
                Status = new Status() { ID = "0", Name = "Open" },
                TimeTracking = new TimeTracking()
                {
                    Issue = this,
                    OriginalEstimateSeconds = 0,
                    RemainingEstimateSeconds = 0
                }
            };
                        
            Reporter = User.UndefinedUser;
            Assignee = User.UndefinedUser;

            Issues = new List<Issue>();
            EstimateInSeconds = 0;
            TimeSpentInSeconds = 0;
        }

        public static Epic FromIssue(Issue issue)
        {
            return new Epic(issue.Key, issue.Fields, issue.GetJira());
        }
    }
}
