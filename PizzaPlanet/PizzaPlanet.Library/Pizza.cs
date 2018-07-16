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
        public enum Amount { No, Light, Regular, Extra}

        public static readonly string[] Sizes = Enum.GetNames(typeof(SizeType));
        public static readonly string[] CrustTypes = Enum.GetNames(typeof(CrustType));
        public static readonly string[] ToppingTypes = Enum.GetNames(typeof(ToppingType));
        public static readonly string[] Amounts = Enum.GetNames(typeof(Amount));

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
        public Amount[] Toppings { get; set; } = new Amount[ToppingTypes.Length];

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
        /// Creates a pizza object from stored code
        /// </summary>
        /// <param name="code"></param>
        public Pizza (int code)
        {
            int c = code;
            Size = (SizeType)((c % 4)+2);
            c /= 4;
            Crust = (CrustType)(c % 4);
            c /= 4;
            Toppings = new Amount[ToppingTypes.Length]; 
            for(int i = 0;i<Toppings.Length; i++)
            {
                if (c == 0) { break; }
                Toppings[i] = (Amount)(c % 4);
                c /= 4;
            }
        }
        //Base pizza
        public Pizza()
        {
            Size = SizeType.Large;
            Crust = CrustType.Hand_Tossed;
            Toppings = new Amount[ToppingTypes.Length];
            Toppings[(int)ToppingType.Cheese] = Amount.Regular;
            Toppings[(int)ToppingType.Sauce] = Amount.Regular;
            for(int i = 2; i < ToppingTypes.Length; i++)
            {
                Toppings[i] = Amount.No;
            }

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
            Toppings[(int)topping] = Amount.No;
        }

        /// <summary>
        /// Transorms pizza into int for storage
        /// </summary>
        /// <returns></returns>
        public int ToInt()
        {
            int p = 0;
            p += ((int)Size-2);
            p += (int)Crust * 4;
            for (int i = 0; i < Toppings.Length; i++)
                p += (int)Toppings[i] * (int)Math.Pow(2,(2*i + 4));
            return p;
        }

        public override string ToString()
        {
            string s = "";
            s += Size.ToString();
            s += " " + Crust.ToString();
            if (Toppings[(int)ToppingType.Sauce] != Amount.Regular)
                s += ", " + Toppings[(int)ToppingType.Sauce].ToString() + " Sauce";
            bool cheese = true;
            if (Toppings[(int)ToppingType.Cheese] != Amount.Regular)
            {
                s += ", " + Toppings[(int)ToppingType.Cheese].ToString() + " Cheese";
                cheese = false;
            }
            for (int i = (int)ToppingType.Pepperoni;i<Toppings.Length;i++)
            {
                if (Toppings[i] != Amount.No)
                {
                    cheese = false;
                    s += ", " + ToppingTypes[i];
                    if (Toppings[i] != Amount.Regular)
                        s += " (" + Toppings[i].ToString() + ")";
                }
            }
            if (cheese)
                s += " Cheese";
            return s;
        }


        /// <summary>
        /// Price of the pizza, calculated based on size/toppings. No Tax
        /// </summary>
        public decimal Price()
        {
            //possible todo: change calculation to "as-we-go"
            //Note base prices used here. Should be made static defaults if displayed to user.

            decimal price = 0.0M;
            switch (Size)
            {
                case SizeType.Small:
                    price = 4.99M;
                    break;
                case SizeType.Medium:
                    price = 6.99M;
                    break;
                default:
                    price = 8.99M;
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
                        price += totalToppings * 0.99M;
                        break;
                    case SizeType.Medium:
                        price += totalToppings * 1.49M;
                        break;
                    default:
                        price += totalToppings * 1.99M;
                        break;
                }
            }
            return Math.Round(price,2);
        }

        public string ToppingsString()
        {
            string ret = "";
            bool first = true;
            for(int i = (int)ToppingType.Pepperoni; i < Toppings.Length; i++)
            {
                string next = "";
                if (!first) next = ", ";
                switch (Toppings[i])
                {
                    case Amount.No:
                        continue;
                    case Amount.Extra:
                        next += "(Extra) ";
                            break;
                    case Amount.Light:
                        next += "(Light) ";
                        break;
                    case Amount.Regular:
                        break;
                }
                first = false;
                next += ToppingTypes[i];
                ret += next;
            }
            if (ret == "") ret = "<None>";
            return ret;
        }

        //public override bool Equals(object obj)
        //{
        //    if (obj.GetType() == typeof(Pizza))
        //        return ((Pizza)obj).ToInt() == this.ToInt();
        //    else
        //        return false;
        //}

    }
}
