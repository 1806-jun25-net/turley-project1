using System;
using Xunit;
using PizzaPlanet.Library;

namespace PizzaPlanet.Testing
{
    public class PizzaTest
    {
        [Fact]
        public void TestLargeHandCheese()
        {
            Pizza p1 = new Pizza(95);
            Pizza p2 = new Pizza(91);
            System.Console.WriteLine(95 + " : " + p1.ToString());
            System.Console.WriteLine(91 + " : " + p2.ToString());
            Assert.True(false, 91 + " : " + p2.ToString() + "\r\n" + 95 + " : " + p1.ToString());
        }

        [Fact]
        public void TestPizzaEncodeDecode()
        {
            int MaxPizza = Pizza.ToppingTypes.Length+2

        } 
    }
}