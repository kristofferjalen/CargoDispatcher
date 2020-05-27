using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CargoDispatcher
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum EventType
    {
        [EnumMember(Value = "DEPART")]
        Depart,
        [EnumMember(Value = "ARRIVE")]
        Arrive
    }
}