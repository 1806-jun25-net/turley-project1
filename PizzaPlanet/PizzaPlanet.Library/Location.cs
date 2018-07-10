using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class Location
    {
        private static IEnumerable<Location> LocationsReal = null;

        public static IEnumerable<Location> Locations()
        {
            if (LocationsReal == null)
                LocationsReal = PizzaRepository.Repo().GetLocations();
            return LocationsReal;
        }

        public static Location GetLocation(int id)
        {
            foreach (Location loc in Locations())
            {
                if (loc.Id == id)
                    return loc;
            }
            return null;
        }

        //temp for location creation
        public static IEnumerable<Location> LoadLocationsTemp()
        {
            var LocationsTemp = new List<Location>();
            LocationsTemp.Add(new Location(101));
            LocationsTemp.Add(new Location(723));
            LocationsTemp.Add(new Location(988));
            foreach (Location loc in LocationsReal)
                loc.FullInventory();
            return LocationsTemp;
        }

        /// <summary>
        /// One month worth of supplies
        /// </summary>
        public static readonly decimal full = 2500.0M;
        /// <summary>
        /// Sets inventory to full. used for testing and database setup
        /// </summary>
        public void FullInventory()
        {
            Dough = full;
            for(int i = 0; i < Toppings.Length; i++)
            {
                Toppings[i] = full;
            }
        }

        /*  Inventory Notes: 1 amount = 1 Small pizza worth, for each type
            Medium = 1.5, large = 2
            'light' = 0.5 multiplier, 'extra' = 1.5 multiplier 
            For now, all crust types draw from Dough inventory
        */

        /// <summary>
        /// Inventory of each topping.
        /// </summary>
        internal decimal[] Toppings = new decimal[Pizza.ToppingTypes.Length];

        /// <summary>
        /// Inventory of Dough
        /// </summary>
        internal decimal Dough = 0;

        /// <summary>
        /// Order History
        /// </summary>
        internal List<Order> Orders;

        /// <summary>
        /// Id Number for next order = Total number of orders + 1
        /// </summary>
        internal int NextOrder = 1;

        /// <summary>
        /// Money store has earned since 'creation'
        /// </summary>
        internal decimal Income { get; set; } = 0;
        /// <summary>
        /// Id number for store (store number) as a 3 digit number
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// Creates new Location with given Id (Store Number) and empty orderhistory and empty inventory
        /// </summary>
        /// <param name="i"></param>
        public Location(int id)
        {
            if (id < 100 || id > 999)
                throw new ArgumentException("Input out of range, store number must be 3 digits");
            Id = id;
            Orders = new List<Order>();
        }

        /// <summary>
        /// Adds the given order to the order history, and subtracts ingredients from inventory
        /// Returns true if order was placed, false if order cannot be filled at this store.
        /// </summary>
        /// <param name="o"></param>
        public bool PlaceOrder(Order o)
        {
            //Possible todo: return failing ingredient rather than false
            //Possible todo: change "check" to its own method

            //Calculates total dough, toppings required for order
            decimal dough = 0;
            decimal[] toppings = new decimal[Toppings.Length]; 
            for(int i = 0;i<o.NumPizza;i++)
            {
                decimal s = (decimal)Pizza.SizeTypeToD(o.Pizzas[i].Size);
                dough += s;
                for(int j = 0; j < toppings.Length;j++)
                    toppings[j] += s*(decimal)Pizza.AmountToD(o.Pizzas[i].Toppings[j]);
            }

            //Check if required dough, toppings exist in store
            if (dough > Dough)
                return false;

            for (int i = 0; i < toppings.Length; i++)
            {
                if (toppings[i] > Toppings[i])
                    return false;
            }

            //If yes, adds order to list and decreases inventory
            //Places the order
            o.Time = DateTime.Now;
            o.Customer.SetLastOrder(o);
            o.Id = NextOrder++;
            Orders.Add(o);
            Dough -= dough;
            for (int i = 0; i < toppings.Length; i++)
                Toppings[i] -= toppings[i];
            Income += o.Price();
            return true;
        }


    }
}
