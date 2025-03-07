using Newtonsoft.Json;
using System.Net;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using KekaBot.kiki.Services.Models;
using System.Collections;
using System.Collections.Generic;

namespace KekaBot.kiki.Services
{
    public class KekaServiceClient
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string helpdeskbaseUrl = "https://bestfriend.kekad.com/k/default";
        private readonly string leavebaseurl = "https://bestfriend.kekad.com/k/leave";
        private readonly string accessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjBGQ0JCNEZEQzNERjQ0Nzk4RjQyREY3ODc3ODgwN0E1MUVFNzUzMkUiLCJ4NXQiOiJEOHUwX2NQZlJIbVBRdDk0ZDRnSHBSN25VeTQiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2xvZ2luLmtla2FkLmNvbSIsIm5iZiI6MTc0MTMzMDY2MSwiaWF0IjoxNzQxMzMwNjYxLCJleHAiOjE3NDE0MTcwNjEsImF1ZCI6WyJrZWthaHIuYXBpIiwiaGlyby5hcGkiLCJodHRwczovL2xvZ2luLmtla2FkLmNvbS9yZXNvdXJjZXMiXSwic2NvcGUiOlsib3BlbmlkIiwia2VrYWhyLmFwaSIsImhpcm8uYXBpIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbInB3ZCJdLCJjbGllbnRfaWQiOiIyZmNiZTdlMC0wZmI0LTRmNmQtODZiYy0xOWZmYzQyZjJmMjUiLCJzdWIiOiJjMTQxOTZhZC1jOTBiLTQzZGYtYWNkMC02YzI3MjIxMDJhNTYiLCJhdXRoX3RpbWUiOjE3NDEwODA2NDEsImlkcCI6ImxvY2FsIiwidGVuYW50X2lkIjoiZTNiYjEwZmMtYzM5NS00MTk2LTkxYTMtZGNiYTYwZjQzMzA4IiwidGVuYW50aWQiOiJlM2JiMTBmYy1jMzk1LTQxOTYtOTFhMy1kY2JhNjBmNDMzMDgiLCJzdWJkb21haW4iOiJiZXN0ZnJpZW5kLmtla2FkLmNvbSIsInVzZXJfaWQiOiJhZmQxNjJmNy02NzU1LTRiMTgtOThiOS00MTlhZmFjM2I0N2UiLCJ1c2VyX2lkZW50aWZpZXIiOiJhZmQxNjJmNy02NzU1LTRiMTgtOThiOS00MTlhZmFjM2I0N2UiLCJ1c2VybmFtZSI6ImdlbWluaS5tdXBwYXJ0aHlAZ21haWwuY29tIiwiZW1haWwiOiJnZW1pbmkubXVwcGFydGh5QGdtYWlsLmNvbSIsImF1dGhlbnRpY2F0aW9uX3R5cGUiOiIxIiwic2lkIjoiOEMwOEU2NkJFRTI3MTA3RURGM0MzNUQ2RDJGMDhFMzEiLCJqdGkiOiIwMzAwNEMzN0U4RDg5MDQ3ODI5QTdBQkEzMzc2QTQ5NyJ9.dlmThYXcfkSm5NXTmT9CMFzfi6uNRwMRKIDFyLalkSUneMenuXhyrTK-OBFxf9YsMzRNZSCZJSubRzmv29mDekylYJY2kIg2g1OB-A1RRkRFZPsh371MlZaohbf-m3xvu0DPBt1HmIcz4PNrSx3DGH7BT76ka0w3AIioZ2dcx0pgZkCJrewNym6XB-_lyXpI4nN85ZwEK25vARBikdvAcnWikSxEC-16l_G9eDPAeal1MvmzkV7vrVa0rKSWs8dleJLuhRAqpUqyU_XsAQujOtaDTnGIKk2tuPPutJ7niC-ICVazmXI-esTFTUIF1hr4dbFR-SK7lnqGuPynjgDMOA";

        public KekaServiceClient()
        {
        }

        public async Task<ResponseModel<TicketCategoryListItem>> GetAllTicketCategoriesAsync()
        {
            var requestUrl = BuildRestRequest(this.helpdeskbaseUrl, KekaApiConstants.GetAllTicketCategories);
            return await ExecuteGetAsync<ResponseModel<TicketCategoryListItem>>(requestUrl, this.accessToken);
        }
        
        public async Task<EmployeeLeaveStats> GetEmployeeLeaves()
        {
            var requestUrl = BuildRestRequest(this.leavebaseurl, KekaApiConstants.LeaveSummary);
            return await ExecuteGetAsync<EmployeeLeaveStats>(requestUrl, this.accessToken);
        }

        public async Task PostTickect(RaiseTicketModel ticket)
        {
            var requestUrl = BuildRestRequest(this.helpdeskbaseUrl, KekaApiConstants.CreateTicket);
            await ExecutePostAsync<object>(requestUrl, ticket, this.accessToken);
        }
        
        public async Task RequestLeave(LeaveRequest leaveRequest)
        {
            var requestUrl = BuildRestRequest(this.leavebaseurl, KekaApiConstants.RequestLeave);
            await ExecutePostAsync<object>(requestUrl, leaveRequest, this.accessToken);
        }

        /// <summary>
        /// Executes an HTTP GET request and deserializes the response.
        /// </summary>
        private async Task<T> ExecuteGetAsync<T>(string url, string accessToken)
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            HttpResponseMessage response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error occurred: {response.StatusCode} - {response.ReasonPhrase}");
            }

            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        private async Task<T> ExecutePostAsync<T>(string url, dynamic requestObject, string accessToken = "", bool isPublicRequest = true)
        {
            if (isPublicRequest)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }

            var serializedObject = JsonConvert.SerializeObject(requestObject);
            HttpContent content = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);

            string responseBody = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(responseBody);
            }

            throw new Exception($"Error occurred: {response.StatusCode} - {response.ReasonPhrase}");
        }

        /// <summary>
        /// Builds the complete API request URL.
        /// </summary>
        private string BuildRestRequest(string baseurl, string urlPath)
        {
            return $"{baseurl}{urlPath}";
        }
    }
}
