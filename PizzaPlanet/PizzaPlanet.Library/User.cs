using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class User
    {
        /// <summary>
        /// Name of the user in string form
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Favorite order, suggested to user when placing new order
        /// </summary>
        public Order Favorite { get; set; }

        /// <summary>
        /// User's chosen Default Location for ordering
        /// </summary>
        public Location DefaultLocation { get; set; }

        /// <summary>
        /// Last order placed, to prevent multiple orders from same location within 2 hours
        /// </summary>
        public Order LastOrder { get; set; }

        public User(string name)
        {
            Name = name;
        }
    }
}
