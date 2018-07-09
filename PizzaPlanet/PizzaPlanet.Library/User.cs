using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class User
    {
        /// <summary>
        /// Id number associated with the user
        /// </summary>
        public int Id { get; }
        
        /// <summary>
        /// Name of the user in string form
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

        public User(int id, string name)
        {
            Id = id;
            Name = name;
            DefLocation = null;
            LastOrder = null;
        }

        /// <summary>
        /// Most recent order. Null if user has never placed an order
        /// </summary>
        /// <returns></returns>
        public Order GetLastOrder()
        {
            if (LastOrder == null)
                LastOrder = Order.GetLastOrder(Id);
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

        public override bool Equals(Object user)
        {
            if(user.GetType() == typeof(User))
                return ((User)user).Id == this.Id;
            return false;
        }
        
    }
}
