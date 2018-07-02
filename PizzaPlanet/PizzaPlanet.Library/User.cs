using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public Order Favorite { get; set; }

    }
}
