using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CargoDispatcher
{
    public class EventPublisher : IEventPublisher
    {
        public void Publish(params Event[] events)
        {
            foreach (var e in events)
            {
                var log = JsonConvert.SerializeObject(e, Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    }
                });

                Console.WriteLine(log);
            }
        }
    }
}