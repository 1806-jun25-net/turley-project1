using System;
using System.Collections.Generic;

namespace PizzaPlanet.DBData
{
    public partial class Pizza
    {
        public decimal OrderId { get; set; }
        public int Code { get; set; }
        public int? Quantity { get; set; }

        public PizzaOrder Order { get; set; }
    }
}
