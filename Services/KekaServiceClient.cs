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
        private readonly string baseUrl = "https://bestfriend.kekad.com/k/default";
        private readonly string accessToken = "eyJhbGciOiJSUzI1NiIsImtpZCI6IjBGQ0JCNEZEQzNERjQ0Nzk4RjQyREY3ODc3ODgwN0E1MUVFNzUzMkUiLCJ4NXQiOiJEOHUwX2NQZlJIbVBRdDk0ZDRnSHBSN25VeTQiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL2xvZ2luLmtla2FkLmNvbSIsIm5iZiI6MTc0MTM1MTI3MywiaWF0IjoxNzQxMzUxMjczLCJleHAiOjE3NDE0Mzc2NzMsImF1ZCI6WyJrZWthaHIuYXBpIiwiaGlyby5hcGkiLCJodHRwczovL2xvZ2luLmtla2FkLmNvbS9yZXNvdXJjZXMiXSwic2NvcGUiOlsib3BlbmlkIiwia2VrYWhyLmFwaSIsImhpcm8uYXBpIiwib2ZmbGluZV9hY2Nlc3MiXSwiYW1yIjpbInB3ZCJdLCJjbGllbnRfaWQiOiIyZmNiZTdlMC0wZmI0LTRmNmQtODZiYy0xOWZmYzQyZjJmMjUiLCJzdWIiOiI1OGNlYTBlZi03ZWRiLTQyZDMtOWQxMS05ZGI0YmEwNmQwZGQiLCJhdXRoX3RpbWUiOjE3Mzk4ODg5ODAsImlkcCI6ImxvY2FsIiwidGVuYW50X2lkIjoiZTNiYjEwZmMtYzM5NS00MTk2LTkxYTMtZGNiYTYwZjQzMzA4IiwidGVuYW50aWQiOiJlM2JiMTBmYy1jMzk1LTQxOTYtOTFhMy1kY2JhNjBmNDMzMDgiLCJzdWJkb21haW4iOiJiZXN0ZnJpZW5kLmtla2FkLmNvbSIsInVzZXJfaWQiOiIzNzNiNThkZi03NmE2LTQzMWEtOWFkOC0wMDE4OWJjODJiODEiLCJ1c2VyX2lkZW50aWZpZXIiOiIzNzNiNThkZi03NmE2LTQzMWEtOWFkOC0wMDE4OWJjODJiODEiLCJ1c2VybmFtZSI6InNpcmVlc2hhLmJmZkBzaW1oYS5pbiIsImVtYWlsIjoic2lyZWVzaGEuYmZmQHNpbWhhLmluIiwiYXV0aGVudGljYXRpb25fdHlwZSI6IjEiLCJzaWQiOiJCM0ZEMDhCRTE4ODRERjVGODY0OEJGQjc3NzVGOEYxMiIsImp0aSI6Ijg2NUE5RTBDOTAxOUVCQzRFRTc4QjQyQjEzRDE0QTQxIn0.ypfiy1twpoLn3VA3IFKa3nWLaVN38RKIAm4FbcHqq5vzutfesAPvHwcgebeGkCZbISfIVJZL6sYLop4PgKGertJoMsE0NLT76ZK-em09IZD76HKirdl8-hRTJhuviu3XEnm1i6_R_qmwvtoMJwzyE2uQoGGdlG9IMQWFQriV6S_P1DCojvkII03UTbL9dzZ5qKe_Ulk5aDgEMukTKlGbutbVtxwIYvqUa8DsJJQG8jKKG7jwaNQvgZ1bLg43uyi54oBjzEsk2P9SffwnlDi0pJbgoxEGtGtKMUoXaJrILbEEE-Om5iRGYAUQyzpz1eCxHkToNN3WTHXFLF5cwu1lhQ";

        public KekaServiceClient()
        {
        }

        public async Task<ResponseModel<TicketCategoryListItem>> GetAllTicketCategoriesAsync()
        {
            var requestUrl = BuildRestRequest(KekaApiConstants.GetAllTicketCategories);
            return await ExecuteGetAsync<ResponseModel<TicketCategoryListItem>>(requestUrl, this.accessToken);
        }

        public async Task PostTicket(RaiseTicketModel ticket)
        {
            var requestUrl = BuildRestRequest(KekaApiConstants.CreateTicket);
            await ExecutePostAsync<object>(requestUrl, ticket, this.accessToken);
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
        private string BuildRestRequest(string urlPath)
        {
            return $"{this.baseUrl}/{urlPath}";
        }
    }
}
