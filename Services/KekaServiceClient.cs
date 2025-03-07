using Newtonsoft.Json;
using System.Net;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using KekaBot.kiki.Services.Models;

namespace KekaBot.kiki.Services
{
    public class KekaServiceClient
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly string baseUrl = "https://bestfriend.kekad.com/k/default";
        private readonly string accessToken = "";

        public KekaServiceClient()
        {
        }

        public async Task<TicketCategoryListItem> GetAllTicketCategoriesAsync()
        {
            var requestUrl = BuildRestRequest(KekaApiConstants.GetAllTicketCategories);
            return await ExecuteGetAsync<TicketCategoryListItem>(requestUrl, this.accessToken);
        }

        public async Task PostTickect(RaiseTicketModel ticket)
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
            return $"{this.baseUrl}{urlPath}";
        }
    }
}
