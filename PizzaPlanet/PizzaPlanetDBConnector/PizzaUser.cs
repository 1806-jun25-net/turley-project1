using System;
using System.Collections.Generic;

namespace PizzaPlanet.DBData
{
    public partial class PizzaUser
    {
        public PizzaUser()
        {
            PizzaOrder = new HashSet<PizzaOrder>();
        }

        public string Username { get; set; }
        public int StoreId { get; set; }

        public ICollection<PizzaOrder> PizzaOrder { get; set; }
    }
}
