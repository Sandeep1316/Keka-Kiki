using Azure;
using System;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Configuration;

namespace KekaBot.kiki.IntentRecognition;

public class IntentRecognizer
{
    public IntentRecognizer(IConfiguration configuration)
    {
        this.IsConfigured = Convert.ToBoolean(configuration["CLU_IsConfigured"]);
        if (this.IsConfigured)
        {
        var endpoint = new Uri(configuration["CLU_Api_Endpoint"]);
        var credentials = new AzureKeyCredential(configuration["CLU_Api_Key"]);
        this.Client = new TextAnalyticsClient(endpoint, credentials);
        }
    }

    /// <summary>
    /// Gets or sets the client.
    /// </summary>
    public TextAnalyticsClient Client { get; }

    public bool IsConfigured { get; }
}
