using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedQueryResultEmail.Webjob.Models
{
    public class WorkItemFields
    {
        [JsonProperty("System.Title")]
        public string title { get; set; }
        [JsonProperty("System.State")]
        public string state { get; set; }
        [JsonProperty("System.WorkItemType")]
        public string workItemType { get; set; }
        [JsonProperty("System.Reason")]
        public string reason { get; set; }
        [JsonProperty("System.AssignedTo")]
        public string assignedTo { get; set; }
        [JsonProperty("System.IterationPath")]
        public string iterationPath { get; set; }
    }
}
