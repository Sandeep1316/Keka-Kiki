using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;

namespace Kiki.CognitiveModels
{
    public partial class Leave : IRecognizerConvert
    {
        public string Text;
        public string AlteredText;
        public enum Intent
        {
            ApplyLeave,
            CancelLeave,
            GetLeaveBalance,
            None
        };
        public Dictionary<Intent, IntentScore> Intents;

        public class _Entities
        {
            public DateTimeSpec[] datetime;

            public string[] EmployeeName;
            public string[] EmployeeId;
            public string[] LeaveType;
            public string[] Days;
            public string[] LeaveRequestId;

            public class _Instance
            {
                public InstanceData[] datetime;
                public InstanceData[] EmployeeName;
                public InstanceData[] EmployeeId;
                public InstanceData[] LeaveType;
                public InstanceData[] Days;
                public InstanceData[] LeaveRequestId;
            }
            [JsonProperty("$instance")]
            public _Instance _instance;
        }
        public _Entities Entities;

        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> Properties { get; set; }

        public void Convert(dynamic result)
        {
            var app = JsonConvert.DeserializeObject<Leave>(JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MaxDepth = null }));
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