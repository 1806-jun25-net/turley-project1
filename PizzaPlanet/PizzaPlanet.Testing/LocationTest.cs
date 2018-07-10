using System;
using Xunit;
using PizzaPlanet.Library;

namespace PizzaPlanet.Testing
{
    public class LocationTest
    {
        private static Location TestLoc;

        private static void SetUp()
        {
            TestLoc = new Location(100);
            TestLoc.FullInventory();
        }

        [Fact]
        public void LargeHandCheeseTest()
        {
            SetUp();
            
        }
    }
}
