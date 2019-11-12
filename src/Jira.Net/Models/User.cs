using System;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class User
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
        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "accountId")]
        public string AccountId { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "emailAddress")]
        public string EmailAddress { get; set; }
        //avatarUrls
        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }
        [DataMember(Name = "active")]
        public bool? Active { get; set; }
        [DataMember(Name = "timeZone")]
        public string TimeZone { get; set; }
        //groups
        //applicationRoles

        public string Username { get { return Key ?? Name; } }
        public string Fullname { get { return DisplayName ?? Name; } }

        public bool IsProjectLead { get; set; }

        //private List<WorkDay> _workdays;
        //public void SetWorkDays(List<Issue> issues, DateTime from, DateTime until)
        //{
        //    List<Worklog> worklogs = issues.SelectMany(issue => issue.GetWorklogs()).Where(worklog => worklog.Author.Equals(this)).ToList();
        //    List<DateTime> days = from.GetDaysUntil(until, DateAndTimeExtentions.DaysBetweenOptions.IgnoreWeekends);
        //    _workdays = days.Select(day => new WorkDay(day, worklogs.Where(worklog => worklog.Started.Date.Equals(day)).ToList())).ToList();
        //}

        //public List<WorkDay> GetWorkDays()
        //{
        //    return _workdays;
        //}

        public override bool Equals(object obj)
        {
            if (obj != null && obj is User)
            {
                return this.Username.Equals((obj as User).Username);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.Username.GetHashCode();
        }

        public static User UndefinedUser
        {
            get
            {
                return new User()
                {
                    DisplayName = "Unassigned",
                    EmailAddress = "",
                    Name = "Unassigned",
                    Key = "Unassigned"
                };
            }
        }
    }
}
