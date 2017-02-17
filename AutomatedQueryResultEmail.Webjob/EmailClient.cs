using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AutomatedQueryResultEmail.Webjob.Models;

namespace AutomatedQueryResultEmail.Webjob
{
    class EmailClient
    {
        public void sendEmail(string emailMessageBody)
        {
            string userName = ConfigurationManager.AppSettings["EmailUserName"];
            string pwd = ConfigurationManager.AppSettings["EmailUserPassword"];
            try
            {
                MailMessage msg = new MailMessage();
                string emailTo = ConfigurationManager.AppSettings["EmailTo"];
                var emailToList = emailTo.Split(';');
                foreach (var item in emailToList)
                {
                    msg.To.Add(new MailAddress(item));
                }
                msg.From = new MailAddress(userName);
                msg.Subject = ConfigurationManager.AppSettings["EmailSubject"];
                msg.Body = emailMessageBody;
                msg.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = ConfigurationManager.AppSettings["SMTPHost"];
            
                smtpClient.Credentials = new System.Net.NetworkCredential(userName, pwd);
                smtpClient.Port = Int32.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
                smtpClient.EnableSsl = true;
                smtpClient.Send(msg);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in sending email " + ex.Message);
            }
        }

        public string getEmailBodyContent(WorkItemList wlist)
        {
            StringBuilder email = new StringBuilder();
            string baseAddress = ConfigurationManager.AppSettings["BaseAddress"];
            string projectName = ConfigurationManager.AppSettings["ProjectName"];
            string workItemUrl = baseAddress+"/"+projectName+"/_workitems?id=";
            email.Append("<html><style>table{ border-collapse: collapse;}table, td, th{border: 1px solid black;}</style><body>");
            email.Append("<p>Hi User,</p>");
            email.Append("<p>Number of Pending queries : " + wlist.count + "</p>");
            email.Append("<p>Following is the list of queries : </p>");
            email.Append("<table>");

            email.Append("<tr>");
            email.Append("<th>Id</th><th>Title</th><th>Work Item Type</th><th>State</th><th>Reason</th><th>Assigned To</th><th>Iteration Path</th>");
            email.Append("</tr>");
            foreach (var item in wlist.value)
            {
                email.Append("<tr bgcolor=\"#f2f2f2\">");
                email.Append("<td><a href='" + workItemUrl + item.id + "'>");
                email.Append(item.id);
                email.Append("</a></td>");
                email.Append("<td>");
                email.Append(item.fields.title);
                email.Append("</td>");
                email.Append("<td>");
                email.Append(item.fields.workItemType);
                email.Append("</td>");
                email.Append("<td>");
                email.Append(item.fields.state);
                email.Append("</td>");
                email.Append("<td>");
                email.Append(item.fields.reason);
                email.Append("</td>");
                email.Append("<td>");
                email.Append(item.fields.assignedTo);
                email.Append("</td>");
                email.Append("<td>");
                email.Append(item.fields.iterationPath);
                email.Append("</td>");
                email.Append("</tr>");
            }
            email.Append("</table>");
            email.Append("<p>Thank you,</p><p>BlueMetal Team</p>");
            email.Append("</body></html>");
            return email.ToString();
        }
    }
}
