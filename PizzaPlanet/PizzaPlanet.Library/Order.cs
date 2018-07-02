using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class Order
    {
        /// <summary>
        /// Store the order is placed at.
        /// </summary>
        public Location Store { get; set; }

        /// <summary>
        /// Order number
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Customer who placed the order
        /// </summary>
        public User Customer { get; set; }

        /// <summary>
        /// Time the order was placed
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Pizzas included in the order
        /// </summary>
        public List<Pizza> Pizzas { get; }

        /// <summary>
        /// Singular constructor for Order. Pizzas must be added after
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="store"></param>
        public Order(User customer, Location store)
        {
            Store = store;
            Customer = customer;
            Time = DateTime.Now;
            Pizzas = new List<Pizza>();
        }

        /// <summary>
        /// Adds a pizza to the order
        /// </summary>
        /// <param name="p"></param>
        public bool AddPizza(Pizza p)
        {
            //possible todo: add cause of failure to false
            if (Pizzas.Count >= 12 || (Price() + p.Price()) > 500)
                return false;
            Pizzas.Add(p);
            return true;
        }
                

        public double Price()
        {
            //possible todo: change calculation to "as-we-go"
            double totalPrice = 0.0;
            foreach (Pizza p in Pizzas)
                totalPrice += p.Price();
            return totalPrice;
        }
    }
}
