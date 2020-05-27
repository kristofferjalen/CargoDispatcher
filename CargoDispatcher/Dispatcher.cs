using System;
using System.Collections.Generic;
using System.Linq;

namespace CargoDispatcher
{
    public class Dispatcher
    {
        private static readonly Queue<Cargo> FactoryQueue = new Queue<Cargo>();
        private static readonly Queue<Cargo> PortQueue = new Queue<Cargo>();
        private readonly IEventPublisher _eventPublisher = new EventPublisher();
        private readonly List<Transportation> _transportations;
        private readonly int _cargosToDeliver;
        private int _time;

        public Dispatcher(IReadOnlyList<Location> destinations)
        {
            _cargosToDeliver = destinations.Count;

            for (var i = 0; i < destinations.Count; i++)
            {
                var cargo = new Cargo
                {
                    CargoId = i,
                    Origin = Location.Factory,
                    Destination = destinations[i]
                };

                FactoryQueue.Enqueue(cargo);
            }

            _transportations = new List<Transportation>
            {
                new Transportation {Id = 0, Kind = Kind.Truck, Cargo = null, Location = Location.Factory, Destination = null},
                new Transportation {Id = 1, Kind = Kind.Truck, Cargo = null, Location = Location.Factory, Destination = null},
                new Transportation {Id = 2, Kind = Kind.Ship, Cargo = null, Location = Location.Port, Destination = null}
            };
        }

        public int Start()
        {
            var arrived = 0;

            while (arrived < _cargosToDeliver)
            {
                foreach (var t in _transportations)
                {
                    if (t.Destination.HasValue && t.Destination.Value == t.Location)
                    {
                        // Arrive
                        var arrive = new Event
                        {
                            EventName = EventType.Arrive,
                            Location = t.Location,
                            Cargo = t.Cargo,
                            Kind = t.Kind,
                            Time = _time,
                            TransportId = t.Id
                        };
                        _eventPublisher.Publish(arrive);
                        t.Destination = null;
                    }

                    // Unload
                    if (t.Kind == Kind.Ship && t.Location == Location.A)
                    {
                        t.Cargo = null;
                        t.Destination = Location.Port;
                        arrived++;
                    }
                    if (t.Kind == Kind.Truck && t.Location == Location.Port)
                    {
                        PortQueue.Enqueue(t.Cargo!.Single());
                        t.Cargo = null;
                        t.Destination = Location.Factory;
                    }
                    if (t.Kind == Kind.Truck && t.Location == Location.B)
                    {
                        t.Cargo = null;
                        t.Destination = Location.Factory;
                        arrived++;
                    }

                    // Load
                    if (t.Kind == Kind.Truck && t.Location == Location.Factory && FactoryQueue.TryDequeue(out var cargo))
                    {
                        t.Cargo ??= new List<Cargo>();
                        t.Cargo.Add(cargo);
                        t.Destination = cargo.Destination == Location.A ? Location.Port : Location.B;
                    }
                    if (t.Kind == Kind.Ship && t.Location == Location.Port && PortQueue.TryDequeue(out var cargoP))
                    {
                        t.Cargo ??= new List<Cargo>();
                        t.Cargo.Add(cargoP);
                        t.Destination = Location.A;
                    }

                    // Location update (move)
                    var location = Move[t.Location](t);
                    if (location != t.Location)
                    {
                        if (t.Kind == Kind.Truck && (t.Location == Location.Factory || t.Location == Location.Port || t.Location == Location.B) ||
                            t.Kind == Kind.Ship && (t.Location == Location.Port || t.Location == Location.A))
                        {
                            // Depart
                            var depart = new Event
                            {
                                EventName = EventType.Depart,
                                Destination = t.Destination!.Value,
                                Location = t.Location,
                                Cargo = t.Cargo,
                                Kind = t.Kind,
                                Time = _time,
                                TransportId = t.Id
                            };
                            _eventPublisher.Publish(depart);
                        }

                        t.Location = location;
                    }
                }

                _time++;
            }

            return _time - 1;
        }

        internal Dictionary<Location, Func<Transportation, Location>> Move = new Dictionary<Location, Func<Transportation, Location>>
        {
            {Location.Factory, transportation => transportation.Cargo?.Any() ?? false
                ? transportation.Cargo.First().Destination is Location.A ? Location.Port : Location.Fb1
                : Location.Factory
            },
            {Location.Port, transportation => transportation.Kind == Kind.Ship && (transportation.Cargo?.Any() ?? false) ? Location.Pa1 :
                transportation.Kind == Kind.Truck ? Location.Factory : Location.Port
            },
            {Location.Pa1, transportation => Location.Pa2},
            {Location.Pa2, transportation => Location.Pa3},
            {Location.Pa3, transportation => Location.A},
            {Location.A,   transportation => Location.Ap1},
            {Location.Ap1, transportation => Location.Ap2},
            {Location.Ap2, transportation => Location.Ap3},
            {Location.Ap3, transportation => Location.Port},
            {Location.Fb1, transportation => Location.Fb2},
            {Location.Fb2, transportation => Location.Fb3},
            {Location.Fb3, transportation => Location.Fb4},
            {Location.Fb4, transportation => Location.B},
            {Location.B,   transportation => Location.Bf1},
            {Location.Bf1, transportation => Location.Bf2},
            {Location.Bf2, transportation => Location.Bf3},
            {Location.Bf3, transportation => Location.Bf4},
            {Location.Bf4, transportation => Location.Factory}
        };
    }
}
