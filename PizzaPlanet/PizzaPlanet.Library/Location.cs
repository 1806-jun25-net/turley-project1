using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class Location
    {
        private static List<Location> LocationsReal = null;
        
        public static List<Location> Locations()
        {
            if (LocationsReal == null)
                LoadLocationsTemp();
            return LocationsReal;
        }
        public static Location GetLocation(int id)
        {
            foreach(Location loc in Locations())
            {
                if (loc.Id == id)
                    return loc; 
            }
            return null;
        }


        public static void LoadLocationsTemp()
        {
            LocationsReal = new Dictionary<int,Location>();
            LocationsReal.Add(101,new Location(101));
            LocationsReal.Add(723,new Location(723));
            LocationsReal.Add(988,new Location(988));
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
        private Dictionary<int, Order> Orders = null;

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
        /// Creates new Location with given Id (Store Number) and 
        /// </summary>
        /// <param name="i"></param>
        public Location(int id)
        {
            if (id < 100 || id > 999)
                throw new ArgumentException("Input out of range, store number must be 3 digits");
            Id = id;
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
            Orders.Add(o.Id, o);
            Dough -= dough;
            for (int i = 0; i < toppings.Length; i++)
                Toppings[i] -= toppings[i];
            Income += o.Price();
            return true;
        }


    }
}
