using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jira.Net.Models
{
    public class IssueSearchResult
    {
        public int Total { get; set; }
        public List<Issue> Issues { get; set; }

        //public IssueSearchResult(JObject searchResult)
        //{
        //    Total = (int)searchResult["total"];

        //    JArray issues = (JArray)searchResult["issues"];
        //    //Issues = issues.Select(issue => new Issue((string)issue["key"], (JObject)issue["fields"])).ToList();
        //}
    }
}

