namespace CargoDispatcher
{
    public class Cargo
    {
        public int CargoId { get; set; }

        public Location Destination { get; set; }

        public Location Origin { get; set; }
    }
}