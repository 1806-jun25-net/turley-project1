using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class Location
    {

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

        public int Id { get; set; }

        /// <summary>
        /// Creates new Location with given Id (Store Number)
        /// </summary>
        /// <param name="Id"></param>
        public Location(int Id) { }

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
            o.Id = NextOrder++;
            Orders.Add(NextOrder++, o);
            Dough -= dough;
            for (int i = 0; i < toppings.Length; i++)
                Toppings[i] -= toppings[i];
            Income += o.Price();

            return true;
        }


    }
}
