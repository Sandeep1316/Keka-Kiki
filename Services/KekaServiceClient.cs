using KekaBot.kiki.PolicyData;
using KekaBot.kiki.Services.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace KekaBot.kiki.Services
{
    public class KekaServiceClient
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string helpdeskbaseUrl = "https://kikibot.kekad.com/k/default";
        private readonly string leavebaseurl = "https://kikibot.kekad.com/k/leave";
        private readonly string accessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjBGQ0JCNEZEQzNERjQ0Nzk4RjQyREY3ODc3ODgwN0E1MUVFNzUzMkUiLCJ4NXQiOiJEOHUwX2NQZlJIbVBRdDk0ZDRnSHBSN25VeTQiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2xvZ2luLmtla2FkLmNvbSIsIm5iZiI6MTc0MTU4NTQ2MCwiaWF0IjoxNzQxNTg1NDYwLCJleHAiOjE3NDE2NzE4NjAsImF1ZCI6WyJrZWthaHIuYXBpIiwiaGlyby5hcGkiLCJodHRwczovL2xvZ2luLmtla2FkLmNvbS9yZXNvdXJjZXMiXSwic2NvcGUiOlsib3BlbmlkIiwia2VrYWhyLmFwaSIsImhpcm8uYXBpIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbIm1mYSJdLCJjbGllbnRfaWQiOiIyZmNiZTdlMC0wZmI0LTRmNmQtODZiYy0xOWZmYzQyZjJmMjUiLCJzdWIiOiJlYzhjYjBmMy02NDY2LTRhNGMtOWI5Ny1mMTdhYjViMWEzNjQiLCJhdXRoX3RpbWUiOjE3NDE0MjU5MzEsImlkcCI6ImxvY2FsIiwidGVuYW50X2lkIjoiN2MzZWRmN2ItM2U0Yi00MDdjLWFhYTAtMzE0Yjk5MTY1MmEzIiwidGVuYW50aWQiOiI3YzNlZGY3Yi0zZTRiLTQwN2MtYWFhMC0zMTRiOTkxNjUyYTMiLCJzdWJkb21haW4iOiJraWtpYm90Lmtla2FkLmNvbSIsInVzZXJfaWQiOiI0Nzg3NTQ4Zi01MzY5LTQyYjQtOTI1OS0yMzYyYTI4ZjE0OGEiLCJ1c2VyX2lkZW50aWZpZXIiOiI0Nzg3NTQ4Zi01MzY5LTQyYjQtOTI1OS0yMzYyYTI4ZjE0OGEiLCJ1c2VybmFtZSI6Imtpa2lib3RAc2ltaGEuaW4iLCJlbWFpbCI6Imtpa2lib3RAc2ltaGEuaW4iLCJhdXRoZW50aWNhdGlvbl90eXBlIjoiMSIsInNpZCI6IjdBODBFNjRGRkQ5OEEyQ0E0OTk5NDM4OTZCQUJCRDIyIiwianRpIjoiMUQ3MjExRjNGNTM3NEI3MDQ2ODFGQjhCQzE5RDY4MzMifQ.fS3h7NdtRBR2ZeMwg9MH7D0Qf6NdVbeiGZOOgXeae27IOXDWtWaeu0pHonEVnxApHxALszR-xNOUVQ36eAz5zafHCxCfMDoS0NRoA4zehHBIkb4UWWJ5VqY-R_HkFSkSiAR8p3JqnzHXxsp3A41xJSzRR8XFY0XmHdZ0MqMUfpe96lQULS04bg1mknNItS987aln7yXrFzdGzPkZBCxIYUL9-GwYmnXAo9gQQ4bsmKYwwuquApv21Su34QK_BpfBx7j7CmBl1QnbwtAIZEPYbQmYp_wEaFf-CVrQeHveyGHYE7nrm1WVlrCd5kthGUQaeJ2z6igKPStzTtYU0s5Okw";

        public KekaServiceClient()
        {
        }

        public async Task<ResponseModel<List<TicketCategoryListItem>>> GetAllTicketCategoriesAsync()
        {
            var requestUrl = BuildRestRequest(this.helpdeskbaseUrl, KekaApiConstants.GetAllTicketCategories);
            return await ExecuteGetAsync<ResponseModel<List<TicketCategoryListItem>>>(requestUrl, this.accessToken);
        }

        public async Task<ResponseModel<EmployeeLeaveStats>> GetEmployeeLeaves()
        {
            var requestUrl = BuildRestRequest(this.leavebaseurl, KekaApiConstants.LeaveSummary);
            return await ExecuteGetAsync<ResponseModel<EmployeeLeaveStats>>(requestUrl + $"?forDate={DateTime.UtcNow.ToString("yyyy-MM-dd")}", this.accessToken);
        }

        public async Task PostTicket(RaiseTicketModel ticket)
        {
            var requestUrl = BuildRestRequest(this.helpdeskbaseUrl, KekaApiConstants.CreateTicket);
            await ExecutePostAsync<object>(requestUrl, ticket, this.accessToken);
        }

        public async Task RequestLeave(LeaveRequest leaveRequest)
        {
            var requestUrl = BuildRestRequest(this.leavebaseurl, KekaApiConstants.RequestLeave);
            await ExecutePostAsync<object>(requestUrl, leaveRequest, this.accessToken);
        }

        public string GetLeavePolicy(string policyType)
        {
            return policyType switch
            {
                var type when type.Contains("sick") => LeavePolicies.SickLeavePolicy,
                var type when type.Contains("Casual") => LeavePolicies.CasualLeavePolicy,
                var type when type.Contains("comp") => LeavePolicies.CompOffLeavePolicy,
                var type when type.Contains("maternity") => LeavePolicies.MaternityLeavePolicy,
                _ => LeavePolicies.DefaultPolicy
            };
        }

        public string GetAttendancePolicy(string policyType)
        {
            return policyType switch
            {
                var type when type.Contains("capture") => AttendancePolicies.CaptureScheme,
                var type when type.Contains("penal") => AttendancePolicies.PenalisationPolicy,
                _ => AttendancePolicies.PenalisationPolicy
            };
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
            return $"{baseurl}/{urlPath}";
        }
    }
}
