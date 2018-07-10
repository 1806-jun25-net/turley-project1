using System;
using Xunit;
using PizzaPlanet.Library;

namespace PizzaPlanet.Testing
{
    public class PizzaTest
    {
        [Fact]
        public void LargeHandCheeseTest()
        {
            Pizza p = new Pizza();
            Assert.Equal(Pizza.SizeType.Large, p.Size);
            Assert.Equal(Pizza.CrustType.Hand_Tossed, p.Crust);
            Assert.Equal(Pizza.Amount.Regular, p.Toppings[(int)Pizza.ToppingType.Sauce]);
            Assert.Equal(Pizza.Amount.Regular, p.Toppings[(int)Pizza.ToppingType.Cheese]);
            Assert.Equal("Large Hand_Tossed Cheese", p.ToString());
        }

        [Fact]
        public void EncodeDecodeTest()
        {
            //Pizza with all toppings extra, large, thin
            int MaxPizza = (int)Math.Pow(2, 24) - 1 - 4 - 1; 
            for (int i = 0; i < MaxPizza; i++)
            {
                if (i % 4 == 3 || (i / 4) % 4 == 3)//these pizza ints are not possible
                    continue;
                else
                {
                    Pizza p = new Pizza(i);
                    Assert.Equal(p.ToInt(), i);
                }
            }
        }

        [Fact]
        public void AddToppingTest()
        {
            Pizza p = new Pizza();
            p.AddTopping(Pizza.ToppingType.Pepperoni);
            Assert.Equal(Pizza.Amount.Regular, p.Toppings[(int)Pizza.ToppingType.Pepperoni]);
        }

        [Fact]
        public void RemoveToppingTest()
        {
            Pizza p = new Pizza();
            p.AddTopping(Pizza.ToppingType.Pepperoni);
            Assert.Equal(Pizza.Amount.Regular, p.Toppings[(int)Pizza.ToppingType.Pepperoni]);
            p.RemoveTopping(Pizza.ToppingType.Pepperoni);
            Assert.Equal(Pizza.Amount.No, p.Toppings[(int)Pizza.ToppingType.Pepperoni]);
        }

        [Fact]
        public void BasePriceTest()
        {
            Pizza p = new Pizza();
            Assert.Equal(8.99M, p.Price());
        }

        [Fact]
        public void ToppingPriceTest()
        {
            Pizza p = new Pizza();
            Assert.Equal(8.99M, p.Price());
            p.AddTopping(Pizza.ToppingType.Pepperoni);
            Assert.Equal(8.99M + 1.99M, p.Price());
        }
    }
}