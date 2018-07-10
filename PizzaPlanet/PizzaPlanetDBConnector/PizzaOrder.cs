using System;
using System.Collections.Generic;

namespace PizzaPlanet.DBData
{
    public partial class PizzaOrder
    {
        public PizzaOrder()
        {
            Pizza = new HashSet<Pizza>();
        }

        public decimal Id { get; set; }
        public int StoreId { get; set; }
        public string Username { get; set; }
        public DateTime OrderTime { get; set; }
        public decimal Total { get; set; }

        public Store Store { get; set; }
        public PizzaUser UsernameNavigation { get; set; }
        public ICollection<Pizza> Pizza { get; set; }
    }
}
