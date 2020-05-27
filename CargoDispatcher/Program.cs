using System.Collections.Generic;

namespace CargoDispatcher
{
    internal class Program
    {
        private static void Main()
        {
            var dispatcher = new Dispatcher(new List<Location>
            {
                Location.A,
                Location.B
            }.AsReadOnly());

            dispatcher.Start();
        }
    }
}
