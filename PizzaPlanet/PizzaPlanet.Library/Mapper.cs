using System;
using System.Collections.Generic;
using System.Linq;

namespace PizzaPlanet.Library
{
    public static class Mapper
    {
        public static Location Map(DBData.Store store)
        {
            Location loc = new Location(store.Id);
            //inventory
            loc.Dough = store.Dough.Value;
            loc.Toppings[(int)Pizza.ToppingType.Bacon] = store.Bacon.Value;
            loc.Toppings[(int)Pizza.ToppingType.Beef] = store.Beef.Value;
            loc.Toppings[(int)Pizza.ToppingType.Black_Olive] = store.BlackOlive.Value;
            loc.Toppings[(int)Pizza.ToppingType.Cheese] = store.Cheese.Value;
            loc.Toppings[(int)Pizza.ToppingType.Green_Pepper] = store.GreenPepper.Value;
            loc.Toppings[(int)Pizza.ToppingType.Ham] = store.Ham.Value;
            loc.Toppings[(int)Pizza.ToppingType.Mushroom] = store.Mushroom.Value;
            loc.Toppings[(int)Pizza.ToppingType.Onion] = store.Onion.Value;
            loc.Toppings[(int)Pizza.ToppingType.Pepperoni] = store.Pepperoni.Value;
            loc.Toppings[(int)Pizza.ToppingType.Sauce] = store.Sauce.Value;
            loc.Toppings[(int)Pizza.ToppingType.Sausage] = store.Sausage.Value;
            //other values
            loc.NextOrder = store.NextOrder.Value;
            loc.Income = store.Income.Value;
            //orderhistory is only loaded when needed
            return loc;
        }

        public static DBData.Store Map(Location loc)
        {
            DBData.Store store = new DBData.Store
            {
                Id = loc.Id,
                //inventory
                Dough = loc.Dough,
                Bacon = loc.Toppings[(int)Pizza.ToppingType.Bacon],
                Beef = loc.Toppings[(int)Pizza.ToppingType.Beef],
                BlackOlive = loc.Toppings[(int)Pizza.ToppingType.Black_Olive],
                Cheese = loc.Toppings[(int)Pizza.ToppingType.Cheese],
                GreenPepper = loc.Toppings[(int)Pizza.ToppingType.Green_Pepper],
                Ham = loc.Toppings[(int)Pizza.ToppingType.Ham],
                Mushroom = loc.Toppings[(int)Pizza.ToppingType.Mushroom],
                Onion = loc.Toppings[(int)Pizza.ToppingType.Onion],
                Pepperoni = loc.Toppings[(int)Pizza.ToppingType.Pepperoni],
                Sauce = loc.Toppings[(int)Pizza.ToppingType.Sauce],
                Sausage = loc.Toppings[(int)Pizza.ToppingType.Sausage],
                NextOrder = loc.NextOrder,
                Income = loc.Income
            };
            return store;
        }

        public static User Map(DBData.PizzaUser user)
        {
            User u;
            if (user.StoreId == -1)
                u = new User(user.Username);
            else
                u = new User(user.Username, Location.GetLocation(user.StoreId));
            return u;
        }

        public static DBData.PizzaUser Map(User user)
        {
            DBData.PizzaUser u = new DBData.PizzaUser();
            u.Username = user.Name;
            if (user.DefLocation == null)
                u.StoreId = -1;
            else
                u.StoreId = user.DefLocation.Id;
            return u;
        }
        
        public static Order Map(DBData.PizzaOrder order)
        {
            Order o = new Order(
                User.TryUser(order.Username), 
                Location.GetLocation(order.StoreId),
                order.OrderTime,
                (int)Math.Truncate(order.Id)
                );
            //gets pizzas
            var pizzas = PizzaRepository.Repo().GetPizzas(order.Id);
            foreach(DBData.Pizza p in pizzas)
            {
                for(int i =0;i<p.Quantity; i++)
                    o.AddPizza(new Pizza(p.Code));
            }
            return o;
        }

        //does nothing with pizzas
        public static DBData.PizzaOrder Map(Order order)
        {
            DBData.PizzaOrder o = new DBData.PizzaOrder();
            o.Id = order.IdFull();
            o.OrderTime = order.Time;
            o.StoreId = order.Store.Id;
            o.Total = order.Price();
            o.Username = order.Customer.Name;
            return o;
        }

        //below are for multiple at once (through IEnumerables)
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