using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;

namespace Kiki.CognitiveModels
{
    public partial class Ticket : IRecognizerConvert
    {
        public string Text;
        public string AlteredText;

        public enum Intent
        {
            CreateTicket,
            GetTicketStatus,
            UpdateTicketStatus,
            None
        };

        public Dictionary<Intent, IntentScore> Intents;

        public class _Entities
        {
            public string[] TicketId;
            public string[] TicketType;
            public string[] Status;
            public string[] Title;
            public string[] Description;
            public string[] Priority;
            public string[] UserId;
            public DateTimeSpec[] datetime;

            public class _Instance
            {
                public InstanceData[] TicketId;
                public InstanceData[] TicketType;
                public InstanceData[] Status;
                public InstanceData[] Description;
                public InstanceData[] Priority;
                public InstanceData[] UserId;
                public InstanceData[] datetime;
            }

            [JsonProperty("$instance")]
            public _Instance _instance;
        }

        public _Entities Entities;

        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> Properties { get; set; }

        public void Convert(dynamic result)
        {
            var app = JsonConvert.DeserializeObject<Ticket>(JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MaxDepth = null }));
            Text = app.Text;
            AlteredText = app.AlteredText;
            Intents = app.Intents;
            Entities = app.Entities;
            Properties = app.Properties;
        }

        public (Intent intent, double score) TopIntent()
        {
            Intent maxIntent = Intent.None;
            var max = 0.0;
            foreach (var entry in Intents)
            {
                if (entry.Value.Score > max)
                {
                    maxIntent = entry.Key;
                    max = entry.Value.Score.Value;
                }
            }
            return (maxIntent, max);
        }
    }
}
