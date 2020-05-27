using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CargoDispatcher
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Kind
    {
        [EnumMember(Value = "TRUCK")]
        Truck,
        [EnumMember(Value = "SHIP")]
        Ship
    }
}