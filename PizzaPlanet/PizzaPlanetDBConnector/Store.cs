using System;
using System.Collections.Generic;

namespace PizzaPlanet.DBData
{
    public partial class Store
    {
        public Store()
        {
            PizzaOrder = new HashSet<PizzaOrder>();
        }

        public int Id { get; set; }
        public decimal? Income { get; set; }
        public int? NextOrder { get; set; }
        public decimal? Dough { get; set; }
        public decimal? Sauce { get; set; }
        public decimal? Cheese { get; set; }
        public decimal? Pepperoni { get; set; }
        public decimal? Sausage { get; set; }
        public decimal? Ham { get; set; }
        public decimal? Bacon { get; set; }
        public decimal? Beef { get; set; }
        public decimal? Onion { get; set; }
        public decimal? GreenPepper { get; set; }
        public decimal? Mushroom { get; set; }
        public decimal? BlackOlive { get; set; }

        public ICollection<PizzaOrder> PizzaOrder { get; set; }
    }
}
