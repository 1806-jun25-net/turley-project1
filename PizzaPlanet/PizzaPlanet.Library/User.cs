using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class User
    {

        private static Dictionary<string, User> Users = new Dictionary<string, User>();

        //old xml version 
        public static void LoadUsers()
        {
            //TODO
        }

        public static User TryUser(string name)
        {
            if (Users.ContainsKey(name))
                return Users[name];
            return null;
        }

        public static User MakeUser(string name)
        {
            User ret = TryUser(name);
            if (ret == null)
                ret = new User(name);
            return ret;
        }

        /// <summary>
        /// Name of the user in string form. Minimum 4 characters
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// User's chosen Default Location for ordering. Null until set
        /// </summary>
        public Location DefLocation { get; set; }

        /// <summary>
        /// Last order placed, to prevent multiple orders from same location within 2 hours
        /// </summary>
        private Order LastOrder;

        public User(string name)
        {
            if (name.Length < 4)
                throw new ArgumentOutOfRangeException("Username too short.");
            Name = name;
            DefLocation = null;
            LastOrder = null;
            Users.Add(name, this);
        }

        /// <summary>
        /// Most recent order. Null if user has never placed an order
        /// </summary>
        /// <returns></returns>
        public Order GetLastOrder()
        {
            if (LastOrder == null)
                LastOrder = Order.GetLastOrder(Name);
            return LastOrder;
        }
        /// <summary>
        /// Sets Most recent order
        /// </summary>
        /// <param name="order"></param>
        public void SetLastOrder(Order order)
        {
            LastOrder = order;
        }
        
    }
}
