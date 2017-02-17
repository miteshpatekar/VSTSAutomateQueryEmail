using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedQueryResultEmail.Webjob.Models
{
    public class WorkItemListValue
    {
        public int id { get; set; }
        public int rev { get; set; }
        public string url { get; set; }
        public WorkItemFields fields { get; set; }
    }
}
