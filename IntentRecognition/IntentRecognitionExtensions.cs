using Azure.AI.TextAnalytics;
using System.Collections.Generic;
using System.Linq;

namespace KekaBot.kiki.IntentRecognition;

public static class IntentRecognitionExtensions
{
    public static List<CategorizedEntity> EntityRecognition(this TextAnalyticsClient client, string text)
    {
        var response = client.RecognizeEntities(text);
        return response.Value.Select(_ => _).ToList();
    }

    public static List<PiiEntity> RecognizePII(this TextAnalyticsClient client, string text)
    {
        PiiEntityCollection entities = client.RecognizePiiEntities(text).Value;
        return entities.Select(_ => _).ToList();
    }


    public static List<LinkedEntity> RecognizeLinkedEntities(this TextAnalyticsClient client, string text)
    {
        var response = client.RecognizeLinkedEntities(text);
        return response.Value.Select(_ => _).ToList();
    }

    public static List<string> ExtractKeyPhrases(this TextAnalyticsClient client, string text)
    {
        var response = client.ExtractKeyPhrases(text);
        return response.Value.Select(_ => _).ToList();
    }
}
