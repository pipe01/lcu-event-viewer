using LCU.NET.WAMP;
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

        public TimeSpan TimeSinceStart => Time.Subtract(RecordingStartTime);
        
        public EventData(DateTime recordingStartTime, JsonApiEvent jsonEvent)
        {
            this.Time = DateTime.Now;
            this.RecordingStartTime = recordingStartTime;
            this.JsonEvent = jsonEvent;
        }
    }
}
