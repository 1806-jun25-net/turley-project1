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
        /// Order number with the store. -1 if unplaced
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
        /// Max number of pizzas allowed in an order
        /// </summary>
        public static readonly int MaxPizzas = 12;
        /// <summary>
        /// Pizzas included in the order
        /// </summary>
        public Pizza[] Pizzas { get; }

        /// <summary>
        /// Current Number of pizzas in the order
        /// </summary>
        public int NumPizza { get; private set; }

        /// <summary>
        /// Singular constructor for Order. Pizzas must be added after
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="store"></param>
        public Order(User customer, Location store)
        {
            Store = store;
            Customer = customer;
            Time = DateTime.MinValue;
            NumPizza = 0;
            Pizzas = new Pizza[MaxPizzas];
            Id = -1;//denotes the order has not been placed
        }

        /// <summary>
        /// Adds a pizza to the order
        /// </summary>
        /// <param name="p"></param>
        public bool AddPizza(Pizza p)
        {
            //possible todo: add cause of failure to false
            if (NumPizza == MaxPizzas || (Price() + p.Price()) > MaxPrice)
                return false;
            Pizzas[NumPizza] = p;
            NumPizza++;
            return true;
        }

        public bool RemovePizza(int p)
        {
            //possible todo, better restriction/permissions here
            if (Pizzas[p] == null)
                return false;
            for(int i = p + 1; i < NumPizza; i++)
                Pizzas[i - 1] = Pizzas[i];
            NumPizza--;
            Pizzas[NumPizza] = null;
            return true;
        }

        /// <summary>
        /// Maximum Price allowed for an order
        /// </summary>
        public static readonly decimal MaxPrice = 500.0M;

        /// <summary>
        /// Calculates the current total price of the order
        /// </summary>
        /// <returns></returns>
        public decimal Price()
        {
            //possible todo: change calculation to "as-we-go"
            decimal totalPrice = 0.0M;
            for(int i =0;i<NumPizza;i++)
                totalPrice += Pizzas[i].Price();
            return totalPrice;
        }

        /// <summary>
        /// String representation of order, for display to console
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "";
            s += "Store: " + Store.Id;
            for(int i = 0; i < NumPizza; i++)
                s += "\r\n" + Pizzas[i].ToString();
            s += "\r\nTotal Price: " + Price();
            return s;
        }

        public bool CanAddPizza()
        {
            return NumPizza < 12 && Price() < 500;
        }

        public static Order GetLastOrder(string userName)
        {
            //TODO
            return null;
        }
        
    }
}
