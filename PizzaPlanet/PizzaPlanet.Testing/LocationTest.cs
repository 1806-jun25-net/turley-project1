using System;
using Xunit;
using PizzaPlanet.Library;

namespace PizzaPlanet.Testing
{
    public class LocationTest
    {
        private static Store TestLoc;

        private static void SetUp()
        {
            TestLoc = new Store(100);
            TestLoc.FullInventory();
        }

        [Fact]
        public void LargeHandCheeseTest()
        {
            SetUp();
            
        }
    }
}
