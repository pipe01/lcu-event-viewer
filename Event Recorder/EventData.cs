using LCU.NET.WAMP;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Recorder
{
    public struct EventData
    {
        public DateTime Time { get; }
        public DateTime RecordingStartTime { get; }
        public JsonApiEvent JsonEvent { get; }

        [JsonIgnore]
        public TimeSpan TimeSinceStart => Time.Subtract(RecordingStartTime);

        [JsonConstructor]
        public EventData(DateTime time, DateTime recordingStartTime, JsonApiEvent jsonEvent)
        {
            this.Time = time;
            this.RecordingStartTime = recordingStartTime;
            this.JsonEvent = jsonEvent;
        }

        public EventData(DateTime recordingStartTime, JsonApiEvent jsonEvent) 
            : this(DateTime.Now, recordingStartTime, jsonEvent)
        {
        }
    }
}
