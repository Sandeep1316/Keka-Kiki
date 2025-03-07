using Azure;
using System;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

namespace KekaBot.kiki.IntentRecognition;

public class IntentRecognizer
{
    private readonly string restApi = "{0}language/:analyze-conversations?api-version={1}";
    private CLURequest _payload;
    private readonly Uri _endpoint;

    public IntentRecognizer(IConfiguration configuration)
    {
        this.IsConfigured = Convert.ToBoolean(configuration["CLU_IsConfigured"]);
        if (this.IsConfigured)
        {
            var endpoint = new Uri(configuration["CLU_Api_Endpoint"]);
            var credentials = new AzureKeyCredential(configuration["CLU_Api_Key"]);
            this.Client = new TextAnalyticsClient(endpoint, credentials);

            this.CluClient = new HttpClient();
            this._endpoint = new Uri(string.Format(restApi, configuration["CLU_Api_Endpoint"], configuration["API_Version"]));
            this.CluClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", configuration["CLU_Api_Key"]);
            this._payload = new CLURequest(configuration["CLU_ProjectName"], configuration["CLU_DeploymentName"]);
        }
    }

    /// <summary>
    /// Gets or sets the client.
    /// </summary>
    public TextAnalyticsClient Client { get; }

    public HttpClient CluClient { get; }

    public bool IsConfigured { get; }

    private HttpContent GetContent(string text)
    {
        this._payload.AnalysisInput.ConversationItem.Text = text;
        return new StringContent(JsonSerializer.Serialize(this._payload), Encoding.UTF8, "application/json");
    }

    public async Task<CLUResponse> RecognizeAsync(string text, CancellationToken cancellationToken = default)
    {
        var response = await this.CluClient.PostAsync(this._endpoint, this.GetContent(text), cancellationToken);
        if ( response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CLUResponse>(result, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        return null;
    }
}
