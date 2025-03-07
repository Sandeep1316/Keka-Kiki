using System;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

namespace KekaBot.kiki.IntentRecognition;

public class CLURequest
{
    public CLURequest(string projectName, string deploymentName)
    {
        AnalysisInput = new AnalysisInput();
        Parameters = new CLUParameters(projectName, deploymentName);
    }

    public CLURequest(string text, string projectName, string deploymentName)
    {
        AnalysisInput = new AnalysisInput
        {
            ConversationItem = new ConversationItem
            {
                Text = text
            }
        };
        Parameters = new CLUParameters(projectName, deploymentName);
    }

    [JsonPropertyName("kind")]
    public string Kind { get; set; } = "Conversation";

    [JsonPropertyName("analysisInput")]
    public AnalysisInput AnalysisInput { get; set; }

    [JsonPropertyName("parameters")]
    public CLUParameters Parameters { get; set; }
}

public class AnalysisInput
{
    public AnalysisInput()
    {
        ConversationItem = new ConversationItem();
    }

    [JsonPropertyName("conversationItem")]
    public ConversationItem ConversationItem { get; set; }
}

public class ConversationItem
{
    public ConversationItem()
    {
        Id = RandomNumberGenerator.GetInt32(1, Int32.MaxValue).ToString();
        ParticipantId = $"user{RandomNumberGenerator.GetInt32(1, Int32.MaxValue).ToString()}";
    }

    public ConversationItem(string id, string participantId)
    {
        Id = id;
        ParticipantId = participantId;
    }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("participantId")]
    public string ParticipantId { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}

public class CLUParameters
{
    public CLUParameters(string projectName, string deploymentName)
    {
        ProjectName = projectName;
        DeploymentName = deploymentName;
    }

    [JsonPropertyName("projectName")]
    public string ProjectName { get; set; }

    [JsonPropertyName("deploymentName")]
    public string DeploymentName { get; set; }

    [JsonPropertyName("stringIndexType")]
    public string StringIndexType { get; set; } = "TextElement_V8";
}
