using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaPlanet.Library
{
    public static class Mapper
    {
        public static Location Map(DBData.Store store)
        {
            //TODO
            return null;
        }

        public static DBData.Store Map(Location location)
        {
            //TODO
            return null;
        }
        public static User Map(DBData.PizzaUser user)
        {
            //ToDO
            return null;
        }

        public static DBData.PizzaUser Map(User user)
        {
            //TODO
            return null;
        }

        //Pizzas must be done here as well
        public static Order Map(DBData.PizzaOrder order)
        {
            //TODO
            return null;
        }

        //Pizzas must be done here as well
        public static DBData.PizzaOrder Map(Order order)
        {
            //TODO
            return null;
        }

        //for multiple at once (through IEnumerables)
        public static IEnumerable<Location> Map(IEnumerable<DBData.Store> stores)
        {
            return stores.Select(Map);
        }

        public static IEnumerable<DBData.Store> Map(IEnumerable<Location> locations)
        {
           return locations.Select(Map);
        }

        public static IEnumerable<User> Map(IEnumerable<DBData.PizzaUser> users)
        {
            return users.Select(Map);
        }

        public static IEnumerable<DBData.PizzaUser> Map(IEnumerable<User> users)
        {
            return users.Select(Map);
        }

        public static IEnumerable<Order> Map(IEnumerable<DBData.PizzaOrder> orders)
        {
            return orders.Select(Map);
        }

        public static IEnumerable<DBData.PizzaOrder> Map(IEnumerable<Order> orders)
        {
            return orders.Select(Map);
        }
    }
}