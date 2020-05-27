using System.Collections.Generic;

namespace CargoDispatcher
{
    public class Transportation
    {
        public int Id { get; set; }

        public Kind Kind { get; set; }

        public Location Location { get; set; }
        
        public Location? Destination { get; set; }

        public List<Cargo> Cargo { get; set; }
    }
}