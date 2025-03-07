using Azure;
using System;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Configuration;
using Microsoft.Bot.Components.Recognizers;
using Microsoft.Bot.Builder;

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

            this.CluRecognizer = new CluAdaptiveRecognizer();
            this.CluRecognizer.ProjectName = configuration["CLU_ProjectName"];
            this.CluRecognizer.Endpoint = configuration["CLU_Endpoint"];
            this.CluRecognizer.EndpointKey = configuration["CLU_Api_Key"];
            this.CluRecognizer.DeploymentName = configuration["CLU_DeploymentName"];
        }
    }

    /// <summary>
    /// Gets or sets the client.
    /// </summary>
    public TextAnalyticsClient Client { get; }

    public CluAdaptiveRecognizer CluRecognizer { get; }

    public bool IsConfigured { get; }
}
