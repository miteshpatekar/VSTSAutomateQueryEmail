using AutomatedQueryResultEmail.Webjob.Models;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AutomatedQueryResultEmail.Webjob
{
    class Program
    {
        static void Main(string[] args)
        {
            string instrukey = ConfigurationManager.AppSettings["AppInsightsInstruKey"];    //path to the query   
            var log = new LoggerConfiguration()
                    .WriteTo
                    .ApplicationInsightsEvents(instrukey)
                    .CreateLogger();

            log.Information("Sending Email");
            try
            {
                WorkItemQuery wq = new WorkItemQuery();
                // get list of pending work items
                WorkItemList wlist = wq.GetWorkItemsByQuery().Result;

                // send email if list is not null
                if (wlist != null)
                {
                    EmailClient ec = new EmailClient();
                    String emailMessageBody = ec.getEmailBodyContent(wlist);
                    ec.sendEmail(emailMessageBody);
                    log.Information("Email sent successfully");
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception "+e.Message);
                log.Error("Exception {e}",e.Message);
            }
            
        }
    }
}
