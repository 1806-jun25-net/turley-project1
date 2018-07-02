using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class Pizza
    {
        public enum Crust_Type { Hand_Tossed, Pan, Thin }
        public enum Topping_Type { Pepperoni, Sausage, Ham, Bacon, Beef, Onion, Green_Pepper, Mushroom }
        public enum Size { Small, Medium, Large}
        public enum Amount { None, Light, Regular, Extra}

        public Crust_Type Crust { get; set; }
        public IEnumerable<Topping_Type> Toppings { get; set; }
        public Amount Sauce { get; set; }
        public Amount Cheese { get; set; }

        
        public Pizza() { }
        public Pizza(Crust_Type crust = Crust_Type.Hand_Tossed,
            Amount sauce = Amount.Regular,
            Amount cheese = Amount.Regular,
            IEnumerable<Topping_Type> toppings = NoToppings())
        { }

        public static IEnumerable<Topping_Type> NoToppings() { return new List<Topping_Type>(); }
        
        
        


    }
}
