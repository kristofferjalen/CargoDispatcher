using System.Collections.Generic;
using Newtonsoft.Json;

namespace CargoDispatcher
{
    public class Event
    {
        [JsonProperty("event")]
        public EventType EventName { get; set; }
        
        public int Time { get; set; }
        
        public int TransportId { get; set; }
        
        public Kind Kind { get; set; }
        
        public Location Location { get; set; }
        
        public Location? Destination { get; set; }
        
        public List<Cargo> Cargo { get; set; }
    }
}
