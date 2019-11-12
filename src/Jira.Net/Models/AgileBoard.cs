using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class AgileBoard
	{
        [DataMember(Name = "id")]
        public int? ID { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "canEdit")]
        public bool? CanEdit { get; set; }
        [DataMember(Name = "sprintSupportEnabled")]
        public bool? SprintSupportEnabled { get; set; }
        [DataMember(Name = "filter")]
        public Filter Filter { get; set; }
        //boardAdmins

        private Jira _jira { get; set; }
		

		private List<Sprint> _sprints;
		public List<Sprint> GetSprints()
		{
			if (_sprints == null)
			{
				_sprints = GetDetailedSprints(_jira.Client.GetSprintsFromAgileBoard(this.ID.Value).OrderByDescending(sprint => sprint.Name).ToList());
			}
			return _sprints;
		}

		public List<Sprint> GetSprintsBetween(DateTime from, DateTime until)
		{
			return GetSprints().Where(sprint =>
				(sprint.EndDate.CompareTo(from) >= 0) // The end date of the sprint comes after the start date of the period
				|| (sprint.EndDate.CompareTo(from) >= 0 && sprint.StartDate.CompareTo(until) <= 0) //OR The end date of the sprint comes after the start date of the period AND the start date of the sprint comes after the start date of the period
			).ToList();
		}

		public List<Sprint> GetAllSprints()
		{
			List<Sprint> sprints = GetSprints();
            List<Sprint> backlogSprints = GetBacklogSprints();
            //Remove the active sprint from the backlog sprints because it is already listed in the sprints list.
            backlogSprints = backlogSprints.Where(sprint => !sprint.State.Equals("ACTIVE")).ToList();
            //Add the backlog sprints to the sprints list.
            sprints.AddRange(backlogSprints);

			return sprints;
		}

		private Sprint _currentSprint;
		public Sprint GetCurrentSprint()
		{
			if (_currentSprint == null)
			{
				_currentSprint = GetSprints().Where(sprint => (!sprint.StartDate.Equals(DateTime.MinValue) ? sprint.StartDate : DateTime.MaxValue).Date.CompareTo(DateTime.Now.Date) <= 0 && (!sprint.EndDate.Equals(DateTime.MinValue) ? sprint.EndDate : DateTime.MaxValue).Date.CompareTo(DateTime.Now.Date) >= 0).FirstOrDefault();
			}
			return _currentSprint;
		}

		private Sprint _nextSprint;
		public Sprint GetNextSprint()
		{
			if (_nextSprint == null)
			{
				_nextSprint = GetBacklogSprints().Where(sprint => sprint.StartDate.Date.CompareTo(DateTime.MinValue) == 0).OrderBy(sprint => sprint.Name).FirstOrDefault();
			}
			return _nextSprint;
		}

		private List<Sprint> _backlogsprints;
		public List<Sprint> GetBacklogSprints()
		{
			if (_backlogsprints == null)
			{
				_backlogsprints = GetDetailedSprints(_jira.Client.GetBacklogSprintsFromAgileBoard(this.ID.Value));
			}
			return _backlogsprints;
		}

		private List<Sprint> GetDetailedSprints(List<Sprint> sprints)
		{
            ConcurrentBag<Sprint> detailedSprints = new ConcurrentBag<Sprint>();
			Parallel.ForEach(sprints, sprint =>
			{
				detailedSprints.Add(GetSprintDetail(sprint));
			});

			return detailedSprints.ToList();
		}

		public Sprint GetSprintDetail(Sprint sprint)
		{
			Sprint detailedSprint = GetSprintByID(sprint.ID.Value);
			if (detailedSprint == null)
			{
				detailedSprint = sprint;
			}
			detailedSprint.SetJira(_jira);

			return detailedSprint;
		}

		public Sprint GetSprintByID(int sprintID)
		{
			Sprint sprint = _jira.Client.GetSprint(this.ID.Value, sprintID);
			if (sprint != null)
			{
				sprint.SetJira(_jira);
			}

			return sprint;
		}

		public List<User> GetUserDetailsForPeriod(DateTime from, DateTime until, List<Issue> additionalIssues)
		{
			List<User> userDetails = new List<User>();
			List<Sprint> sprints = GetSprintsBetween(from, until);
			Sprint firstSprint = sprints.FirstOrDefault();

			if (additionalIssues == null)
			{
				additionalIssues = new List<Issue>();
			}

			if (firstSprint != null)
			{
				sprints.Remove(firstSprint);
				additionalIssues.AddRange(sprints.SelectMany(sprint => sprint.GetIssues()));

				userDetails = firstSprint.GetAssignableUsers(additionalIssues, from, until);
			}

			return userDetails;
		}
	}

    [Serializable]
    [DataContract]
    public class AgileBoardView
    {
        [DataMember(Name = "views")]
        public List<AgileBoard> Views { get; set; }
    }
}
