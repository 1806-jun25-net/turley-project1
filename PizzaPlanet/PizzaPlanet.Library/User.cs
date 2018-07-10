using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PizzaPlanet.Library
{
    public class User
    {

        /// <summary>
        /// storage for list of users. instantiated on first call to Users()
        /// </summary>
        private static IEnumerable<User> UsersReal = null;

        /// <summary>
        /// public accessor for list of users
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<User> Users()
        { 
            if(UsersReal == null)
            {
                UsersReal = PizzaRepository.Repo().GetUsers();
            }
            return UsersReal;
        }

        //old xml version 
        public static void LoadUsersOld()
        {
            
        }
        
        /// <summary>
        /// Attempts to find user by name. If user exists, returns that user. else null
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static User TryUser(string name)
        {
            foreach (User u in Users())
            {
                if (u.Name == name)
                    return u;
            }
            return null;
        }
        
        /// <summary>
        /// Returns the user if he exists (failsafe), else makes a new one with given name and returns
        /// Addition to database and local is handled here
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static User MakeUser(string name)
        {
            User ret = TryUser(name);
            if (ret == null)
            {
                ret = new User(name);
                PizzaRepository.Repo().AddUser(ret);
                UsersReal = UsersReal.Concat(new[] { ret });
            }
            return ret;
        }

        /// <summary>
        /// Name of the user in string form. Minimum 4 characters
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// User's chosen Default Location for ordering. Null until set
        /// </summary>
        public Location DefLocation { get; }

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
        }

        public User(string name, Location defLocation) : this(name)
        {
            DefLocation = defLocation;

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
