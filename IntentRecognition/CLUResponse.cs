using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace KekaBot.kiki.IntentRecognition;

public class CLUResponse
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; }

    [JsonPropertyName("result")]
    public CLUResult Result { get; set; }
}

public class CLUResult
{
    [JsonPropertyName("query")]
    public string Query { get; set; }

    [JsonPropertyName("prediction")]
    public Prediction Prediction { get; set; }
}

public class Prediction
{
    [JsonPropertyName("topIntent")]
    public string TopIntent { get; set; }

    [JsonPropertyName("projectKind")]
    public string ProjectKind { get; set; }

    [JsonPropertyName("intents")]
    public List<Intent> Intents { get; set; }

    [JsonPropertyName("entities")]
    public List<Entity> Entities { get; set; }
}

public class Intent
{
    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("confidenceScore")]
    public float ConfidenceScore { get; set; }
}

public class Entity
{
    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("offset")]
    public int Offset { get; set; }

    [JsonPropertyName("length")]
    public int Length { get; set; }

    [JsonPropertyName("confidenceScore")]
    public float ConfidenceScore { get; set; }
}
