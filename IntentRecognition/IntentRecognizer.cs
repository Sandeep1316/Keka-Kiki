using Azure;
using System;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Configuration;

namespace KekaBot.kiki.IntentRecognition;

public class IntentRecognizer
{
    public IntentRecognizer(IConfiguration configuration)
    {
        var endpoint = new Uri(configuration["CLU_Api_Endpoint"]);
        var credentials = new AzureKeyCredential(configuration["CLU_Api_Key"]);
        this.Client = new TextAnalyticsClient(endpoint, credentials);
        // You will implement these methods later in the quickstart.
    }

    /// <summary>
    /// Gets or sets the client.
    /// </summary>
    public TextAnalyticsClient Client { get; set; }
}
