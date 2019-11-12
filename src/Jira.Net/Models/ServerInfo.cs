using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Jira.Net.Models
{
    [Serializable]
    [DataContract]
    public class ServerInfo
    {
        [DataMember(Name = "baseUrl")]
        public string BaseUrl { get; set; }
        [DataMember(Name = "version")]
        public string Version { get; set; }
        [DataMember(Name = "versionNumbers")]
        public List<int> VersionNumbers { get; set; }
        [DataMember(Name = "buildNumber")]
        public string BuildNumber { get; set; }
        [DataMember(Name = "buildDate")]
        public string BuildDate { get; set; }
        [DataMember(Name = "serverTime")]
        public string ServerTime { get; set; }
        [DataMember(Name = "scmInfo")]
        public string ScmInfo { get; set; }
        [DataMember(Name = "serverTitle")]
        public string ServerTitle { get; set; }
        
        //TODO
        [DataMember(Name = "defaultLocale")]
        public Dictionary<string, string> DefaultLocale { get; set; }
        [DataMember(Name = "deploymentType")]
        public string DeploymentType { get; set; }
    }


}
