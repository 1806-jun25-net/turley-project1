using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class Location
    {
        private static Dictionary<int,Location> LocationsReal = null;
        
        public static Dictionary<int,Location> Locations()
        {
            if (LocationsReal == null)
                FillLocations();
            return LocationsReal;
        }
        public static Location GetLocation(int id)
        {
            foreach(int i in LocationsReal.Keys)
            {
                if (i == id)
                    return LocationsReal[id]; 
            }
            return null;
        }
        //public static Location GetLocation(int num)
        //{
        //    return LocationsReal[num];
        //}
        private static void FillLocations()
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
        private double[] Toppings = new double[Pizza.ToppingTypes.Length];

        /// <summary>
        /// Inventory of Dough
        /// </summary>
        private double Dough = 0;

        /// <summary>
        /// Order History
        /// </summary>
        public Dictionary<int, Order> Orders = new Dictionary<int, Order>();

        /// <summary>
        /// Id Number for next order = Total number of orders + 1
        /// </summary>
        private int NextOrder = 1;

        /// <summary>
        /// Money store has earned since 'creation'
        /// </summary>
        private double Income { get; set; } = 0;
        /// <summary>
        /// Id number for store (store number) as a 3 digit number
        /// </summary>
        public readonly int Id;

        /// <summary>
        /// Creates new Location with given Id (Store Number) and 
        /// </summary>
        /// <param name="i"></param>
        public Location(int i)
        {
            if (i < 100 || i > 999)
                throw new Exception("Input out of range, store number must be 3 digits");
            Id = i;
        }

        /// <summary>
        /// Adds the given order to the order history, and subtracts ingredients from inventory
        /// Returns true if order was placed, false if order cannot be filled at this store.
        /// </summary>
        /// <param name="o"></param>
        public bool PlaceOrder(Order o)
        {
            //Possible todo: return failing ingredient rather than false

            //Calculates total dough, toppings required for order
            double dough = 0;
            double[] toppings = new double[Toppings.Length]; 
            foreach(Pizza p in o.Pizzas)
            {
                double s = Pizza.SizeTypeToD(p.Size);
                dough += s;
                for(int i = 0; i < toppings.Length;i++)
                    toppings[i] += s*Pizza.AmountToD(p.Toppings[i]);
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
            //Actually places the order
            o.Time = DateTime.Now;
            o.Id = NextOrder++;
            o.Customer.LastOrder = o;
            Orders.Add(NextOrder++, o);
            Dough -= dough;
            for (int i = 0; i < toppings.Length; i++)
                Toppings[i] -= toppings[i];
            Income += o.Price();
            return true;
        }


    }
}
