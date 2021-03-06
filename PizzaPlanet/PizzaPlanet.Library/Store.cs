﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaPlanet.Library
{
    
    public class Store
    {
        private static IEnumerable<Store> StoresReal = null;

        public static IEnumerable<Store> Stores()
        {
            return PizzaRepository.Repo().GetStores();
        }

        public static Store GetStore(int id)
        {
            foreach (Store loc in Stores())
            {
                if (loc.Id == id)
                    return loc;
            }
            return null;
        }

        //temp for location creation
        public static IEnumerable<Store> LoadStoresTemp()
        {
            var LocationsTemp = new List<Store>();
            LocationsTemp.Add(new Store(101));
            LocationsTemp.Add(new Store(723));
            LocationsTemp.Add(new Store(988));
            foreach (Store loc in StoresReal)
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
        internal decimal[] Toppings = new decimal[Pizza.ToppingTypes.Count];

        /// <summary>
        /// Inventory of Dough
        /// </summary>
        internal decimal Dough = 0;

        /// <summary>
        /// Order History
        /// </summary>
        private IEnumerable<Order> OrdersReal;

        
        public IEnumerable<Order> Orders()
        {
            if (OrdersReal == null)
                OrdersReal =  PizzaRepository.Repo().GetOrders(this.Id);
            return OrdersReal;
        }

        /// <summary>
        /// Adds order to list when it is placed
        /// </summary>
        /// <param name="order"></param>
        private void AddOrder(Order order)
        {
            if (OrdersReal == null)
                Orders();
            OrdersReal = OrdersReal.Concat(new[] { order }).OrderBy(o=>o.Time);
        }

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
        public Store(int id)
        {
            if (id < 100 || id > 999)
                throw new ArgumentException("Input out of range, store number must be 3 digits");
            Id = id;
            OrdersReal = null;
            //orders are loaded when asked for
        }

        /// <summary>
        /// Adds the given order to the order history, and subtracts ingredients from inventory
        /// Returns true if order was placed, false if order cannot be filled at this store.
        /// </summary>
        /// <param name="o"></param>
        public bool PlaceOrder(Order o)
        {
            //Possible: return failing ingredient rather than false
            //Possible: change "check" to its own method

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
            o.Id = NextOrder++;
            o.Customer.AddOrder(o);
            AddOrder(o);
            Dough -= dough;
            for (int i = 0; i < toppings.Length; i++)
                Toppings[i] -= toppings[i];
            Income += o.Price();
            return true;
        }


    }
}
