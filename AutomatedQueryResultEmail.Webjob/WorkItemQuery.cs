using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using AutomatedQueryResultEmail.Webjob.Models;

namespace AutomatedQueryResultEmail.Webjob
{
    class WorkItemQuery
    {
        public async Task<WorkItemList> GetWorkItemsByQuery()
        {
            string personalAccessToken = ConfigurationManager.AppSettings["PersonalAccessToken"];
            string credentials = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", personalAccessToken)));
            string baseAddress = ConfigurationManager.AppSettings["BaseAddress"];
            string project = ConfigurationManager.AppSettings["ProjectName"];
            string path = ConfigurationManager.AppSettings["QueryPath"];    //path to the query   

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://bluemetal.visualstudio.com");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                    //if you already know the query id, then you can skip this step
                    HttpResponseMessage queryHttpResponseMessage = client.GetAsync(project + "/_apis/wit/queries/" + path + "?api-version=2.2").Result;

                    if (queryHttpResponseMessage.IsSuccessStatusCode)
                    {
                        string responseBody = await queryHttpResponseMessage.Content.ReadAsStringAsync();
                        //bind the response content to the queryResult object
                        QueryResult queryResult = queryHttpResponseMessage.Content.ReadAsAsync<QueryResult>().Result;
                        string queryId = queryResult.id;

                        //using the queryId in the url, we can execute the query
                        HttpResponseMessage httpResponseMessage = client.GetAsync(project + "/_apis/wit/wiql/" + queryId + "?api-version=2.2").Result;

                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            //string responseBody1 = await httpResponseMessage.Content.ReadAsStringAsync();
                            WorkItemQueryResult workItemQueryResult = httpResponseMessage.Content.ReadAsAsync<WorkItemQueryResult>().Result;

                            //now that we have a bunch of work items, build a list of id's so we can get details
                            var builder = new System.Text.StringBuilder();
                            foreach (var item in workItemQueryResult.workItems)
                            {
                                builder.Append(item.id.ToString()).Append(",");
                            }

                            //clean up string of id's
                            string ids = builder.ToString().TrimEnd(new char[] { ',' });

                            //get the work items list 
                            HttpResponseMessage getWorkItemsHttpResponse = client.GetAsync("_apis/wit/workitems?ids=" + ids + "&fields=System.Id,System.Title,System.WorkItemType,System.State,System.Reason,System.AssignedTo,System.IterationPath&asOf=" + workItemQueryResult.asOf + "&api-version=2.2").Result;

                            if (getWorkItemsHttpResponse.IsSuccessStatusCode)
                            {
                                //  string responseBody2 = await getWorkItemsHttpResponse.Content.ReadAsStringAsync();
                                var result = getWorkItemsHttpResponse.Content.ReadAsStringAsync().Result;
                                WorkItemList list = getWorkItemsHttpResponse.Content.ReadAsAsync<WorkItemList>().Result;
                                return list;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
            }
            return null;
        }
    }
}
