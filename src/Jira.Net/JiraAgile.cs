using Jira.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.Net
{
    public class JiraAgile
    {
        private IJiraClient _client;
        //public JiraAgile()
        //{ }

        public void Connect(IJiraClient client)
        {
            _client = client;
        }

        public void Connect(string url)
        {
            Connect(new JiraClient(url));
        }

        public void Connect(string url, string username, string password)
        {
            Connect(new JiraClient(url, username, password));
        }

        private List<AgileBoard> _agileBoards;
        public List<AgileBoard> GetAgileBoards()
        {
            _agileBoards = _client.GetItem<AgileBoardView>(JiraObjectEnum.AgileBoards).Views;
            //_agileBoards.ForEach(board => board.SetJira(this));
            return _agileBoards;
        }

        public AgileBoard GetAgileBoard(int agileBoardID)
        {
            if (null == _agileBoards)
            {
                GetAgileBoards();
            }
            AgileBoard board = _agileBoards.Where(b => b.ID == agileBoardID).FirstOrDefault();
            if (board == null)
            {
                throw new Exception(string.Format("The board with ID {0} does not exist", agileBoardID));
            }

            return board;
        }

        public List<Sprint> GetSprintsFromAgileBoard(int agileBoardID)
        {
            return _client.GetSprintsFromAgileBoard(agileBoardID);
        }

        public IssueSearchResults GetIssuesFromSprint(int sprintID)
        {
            return _client.GetIssuesFromSprint(sprintID);
        }
    }
}
