using System;
using System.Collections.Generic;
using System.Text;

namespace PizzaPlanet.Library
{
    public class Pizza
    {
        
        public enum CrustType { Hand_Tossed, Pan, Thin }
        public enum ToppingType { Sauce, Cheese, Pepperoni, Sausage, Ham, Bacon, Beef, Onion, Green_Pepper, Mushroom, Black_Olive }
        public enum SizeType { Small=2, Medium, Large}
        public enum Amount { None, Light, Regular, Extra}

        public static string[] Sizes = Enum.GetNames(typeof(SizeType));
        public static string[] CrustTypes = Enum.GetNames(typeof(CrustType));
        public static string[] ToppingTypes = Enum.GetNames(typeof(ToppingType));
        public static string[] Amounts = Enum.GetNames(typeof(Amount));

        /// <summary>
        /// Transforms Amount enum to usable double modifier.
        /// None = 0.0, Light = 0.5, Regular = 1.0, Extra = 1.5
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double AmountToD(Pizza.Amount a)
        {
            return (int)a * 0.5;
        }

        /// <summary>
        /// Transforms SizeType enum to usable double modifier.
        /// Small = 1.0, Medium = 1.5, Large = 2.0
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double SizeTypeToD(Pizza.SizeType s)
        {
            return (int)s * 0.5;
        }

        public SizeType Size { get; set; }
        public CrustType Crust { get; set; }
        public Amount[] Toppings { get; }

        /// <summary>
        /// New Pizza with no toppings, Regular sauce/cheese and large size are default.
        /// </summary>
        /// <param name="crust"></param>
        /// <param name="sauce"></param>
        /// <param name="cheese"></param>
        public Pizza(
                SizeType size = SizeType.Large,
                CrustType crust = CrustType.Hand_Tossed,
                Amount sauce = Amount.Regular,
                Amount cheese = Amount.Regular
            )
        {
            Size = size;
            Crust = crust;
            Toppings = new Amount[ToppingTypes.Length];
            Toppings[(int)ToppingType.Cheese] = cheese;
            Toppings[(int)ToppingType.Sauce] = sauce;
        }
        /// <summary>
        /// Adds topping to pizza. Regular amount unless modified
        /// </summary>
        /// <param name="topping"></param>
        /// <param name="a"></param>
        public void AddTopping(ToppingType topping, Amount a = Amount.Regular)
        {
            Toppings[(int)topping] = a;
        }

        /// <summary>
        /// Removes topping from pizza. Regular amount unless modified
        /// </summary>
        /// <param name="topping"></param>
        public void RemoveTopping(ToppingType topping)
        {
            Toppings[(int)topping] = Amount.None;
        }

        /// <summary>
        /// Price of the pizza, calculated based on size/toppings. No Tax
        /// </summary>
        public double Price()
        {
            //possible todo: change calculation to "as-we-go"
            
            //Note base prices used here. Should be made static defaults if displayed to user.

            double price = 0.0;
            switch (Size)
            {
                case SizeType.Small:
                    price = 4.99;
                    break;
                case SizeType.Medium:
                    price = 6.99;
                    break;
                default:
                    price = 8.99;
                    break;
            }

            int totalToppings = 0;
            //Converts each Topping Amount to price. None = 0x, Light/Regular = 1x, Extra = 2x
            for (int i = (int)ToppingType.Pepperoni; i < Toppings.Length;i++)
                totalToppings += ((((int)Toppings[i])+1)/2);

            if (totalToppings > 0)
            {
                switch (Size)
                {
                    case SizeType.Small:
                        price += totalToppings * 0.99;
                        break;
                    case SizeType.Medium:
                        price += totalToppings * 1.49;
                        break;
                    default:
                        price += totalToppings * 1.99;
                        break;
                }
            }
            return Math.Round(price,2);
        }
        


    }
}
