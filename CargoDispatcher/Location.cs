using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CargoDispatcher
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Location
    {
        [EnumMember(Value = "FACTORY")]
        Factory,
        [EnumMember(Value = "PORT")]
        Port,
        Pa1,
        Pa2,
        Pa3,
        A,
        Ap1,
        Ap2,
        Ap3,
        Fb1,
        Fb2,
        Fb3,
        Fb4,
        B,
        Bf4,
        Bf3,
        Bf2,
        Bf1
    }
}