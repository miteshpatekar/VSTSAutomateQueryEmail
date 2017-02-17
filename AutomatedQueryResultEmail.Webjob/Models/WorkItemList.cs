using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedQueryResultEmail.Webjob.Models
{
    public class WorkItemList
    {
        public int count { get; set; }
        public WorkItemListValue[] value { get; set; }
    }
}
