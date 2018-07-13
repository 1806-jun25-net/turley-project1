using System;
using Xunit;
using PizzaPlanet.Library;

namespace PizzaPlanet.Testing
{
    public class OrderTest
    {
        private static int LargeHandCheese = 162;
        private static int MaxPizza = (int)Math.Pow(2, 24) - 1 - 4 - 1;
        private static Store TestLoc;
        private static int StoreNum = 100;
        private static User user;
        private static void SetUp()
        {
            TestLoc = new Store(StoreNum);
            user = new User("test");
            TestLoc.FullInventory();
        }
    
        // requires repo
        //[Fact]
        //public void OrderIdFullTest()
        //{
        //    SetUp();
        //    Order o = new Order(user,TestLoc,DateTime.Now,0);
        //    o.AddPizza(new Pizza(LargeHandCheese));
        //    TestLoc.PlaceOrder(o);
        //    Assert.Equal(1, o.Id);
        //    Assert.Equal(1.100M, o.IdFull());
        //}

        [Fact]
        public void AddRemoveNumPizzaTest()
        {
            SetUp();
            Order o = new Order(user, TestLoc, DateTime.Now, 0);
            Assert.Equal(0, o.NumPizza);
            o.AddPizza(new Pizza(LargeHandCheese));
            Assert.Equal(1,o.NumPizza);
            o.RemovePizza(0);
            Assert.Equal(0, o.NumPizza);
        }

        [Fact]
        public void CanAddNumPizzaTest()
        {
            SetUp();
            Order o = new Order(user, TestLoc, DateTime.Now, 0);
            Assert.True(o.CanAddPizza());
            for(int i =0;i<Order.MaxPizzas;i++)
                o.AddPizza(new Pizza(LargeHandCheese));
            Assert.Equal(12, o.NumPizza);
            Assert.False(o.CanAddPizza());
        }

        [Fact]
        public void CanAddPricePizzaTest()
        {
            SetUp();
            Order o = new Order(user, TestLoc, DateTime.Now, 0);
            Assert.True(o.CanAddPizza());
            Pizza p = new Pizza(MaxPizza);
            for (int i = 0; i < 500/p.Price(); i++)
                o.AddPizza(p);
            Assert.False(o.AddPizza(p));
        }
    }
}
