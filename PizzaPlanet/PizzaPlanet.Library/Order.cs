using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class Order
    {
        public static readonly int TimeBetweenOrders = 120;

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
        /// Constructor for trying to place a new Order. Pizzas must be added after
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="store"></param>
        public Order(User customer, Location store)
        {
            var LastOrder = customer.LastOrder();
            //order fails if customer placed too soon
            if (LastOrder.Store.Id == store.Id && (DateTime.Now - LastOrder.Time).TotalMinutes < TimeBetweenOrders)
                throw new PizzaTooSoonException("User has ordered from this store too recently.");
            Store = store;
            Customer = customer;
            Time = DateTime.MinValue;
            NumPizza = 0;
            Pizzas = new Pizza[MaxPizzas];
            Id = -1;//denotes the order has not been placed
        }
        /// <summary>
        /// Attempts to create a new order from a previous template
        /// </summary>
        /// <param name="temp"></param>
        public Order(Order temp): this(temp.Customer,temp.Store)
        {
            for (int i = 0; i < temp.NumPizza; i++)
                AddPizza(temp.Pizzas[i]);
        }

        /// <summary>
        /// Creating an order that was already placed, but pizzas need added
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="store"></param>
        /// <param name="orderTime"></param>
        /// <param name="id"></param>
        public Order(User customer, Location store, DateTime orderTime, int id)
        {
            Store = store;
            Customer = customer;
            NumPizza = 0;
            Pizzas = new Pizza[MaxPizzas];
            Time = orderTime;
            Id = id;
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
            s += "Store: <" + Store.Id+">";
            for(int i = 0; i < NumPizza; i++)
                s += "\r\n" + Pizzas[i].ToString();
            s += "\r\nTotal Price: $" + Price();
            return s;
        }

        /// <summary>
        /// full version of id, which includes store number as a decimal
        /// </summary>
        /// <returns></returns>
        public decimal IdFull()
        {
            decimal ret = (decimal)Store.Id;
            ret = ret / 1000.0M;
            ret += (decimal)Id;
            return ret;
        }

        public bool CanAddPizza()
        {
            return NumPizza < MaxPizzas && Price() < MaxPrice;
        }

        public string FullDetails()
        {
            string ret = "Store: <" + Store.Id + "> Order: <" + Id + "> \r\nTime: <" +
                Time.ToShortDateString() + " " + Time.Hour + ":" + Time.Minute +
                ">\r\nBy: <" + Customer.Name + ">\r\n";
            for (int i = 0; i < NumPizza; i++)
                ret += Pizzas[i].ToString() + "\r\n";
            ret+="Total: <$" + Price() + ">";
            return ret;
        }

        public string Details()
        {
            string ret = "Store: <" + Store.Id + "> Time: <" + 
                Time.ToShortDateString()+" "+Time.Hour+":"+Time.Minute + 
                "> By: <" + Customer.Name + "> Total: <$" + Price()+">";
            return ret;
        }
        
    }
}
