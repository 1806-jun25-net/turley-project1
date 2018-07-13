using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PizzaPlanet.DBData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PizzaPlanet.Library
{
    /// <summary>
    /// A repository managing data access
    /// </summary>
    public class PizzaRepository
    {
        private readonly Project1PizzaPlanetContext _db;

        /// <summary>
        /// Single permanent Repository for use by console
        /// </summary>
        private static PizzaRepository RepoReal = null;

        /// <summary>
        /// Accessor for pizzarepository. Calls exception if instantiation failed
        /// </summary>
        /// <returns></returns>
        public static PizzaRepository Repo()
        {
            if (RepoReal == null)
            {
                var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).FullName)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = configBuilder.Build();
                var optionsBuilder = new DbContextOptionsBuilder<Project1PizzaPlanetContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("PizzaPlanet"));
                var options = optionsBuilder.Options;
                RepoReal = new PizzaRepository(new Project1PizzaPlanetContext(options));
            }
            return RepoReal;
        }
        

        private PizzaRepository(Project1PizzaPlanetContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Get all stores/locations
        /// </summary>
        /// <returns>The collection of Locations (stores)</returns>
        public IEnumerable<Store> GetStores()
        {
            // disable pointless tracking for performance
            return Mapper.Map(_db.Store.AsNoTracking().ToList());
        }

        /// <summary>
        /// Gets all previous users
        /// </summary>
        /// <returns></returns>
        public IEnumerable<User> GetUsers()
        {
            return Mapper.Map(_db.PizzaUser.AsNoTracking().ToList());
        }

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="u"></param>
        public void AddUser(User u)
        {
            _db.PizzaUser.Add(Mapper.Map(u));
            Save();
        }

        /// <summary>
        /// Updates the database with the order and associated pizzas.
        /// Also updates the location's inventory
        /// </summary>
        /// <param name="o"></param>
        public void PlaceOrder(Order o)
        {
            //check if the order has been placed to the location
            if (o.Id == -1)
                throw new ArgumentException("Order was not placed first.");
            _db.PizzaOrder.Add(Mapper.Map(o));
            var pizzas = new Dictionary<int, DBData.Pizza>();
            for (int i = 0; i < o.NumPizza; i++)
            {
                int code = o.Pizzas[i].ToInt();
                if (pizzas.Keys.Contains(code))
                    pizzas[code].Quantity++;
                else
                {
                    var dbpizza = new DBData.Pizza
                    { Quantity = 1, OrderId = o.IdFull(), Code = code };
                    pizzas.Add(code, dbpizza);
                }
            }
            foreach (DBData.Pizza DBp in pizzas.Values)
                _db.Pizza.Add(DBp);
            _db.Store.Update(Mapper.Map(o.Store));
            Save();
        }
        /// <summary>
        /// All orders placed by User with name, ordered by newest first
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetOrders(string name)
        {
            return Mapper.Map(_db.PizzaOrder.Where(o => (o.Username == name)).OrderBy(o => (DateTime.Now - o.OrderTime)));
        }

        /// <summary>
        /// All orders placed at Store with given Id, ordered with newest first
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public IEnumerable<Order> GetOrders(int storeId)
        {
            return Mapper.Map(_db.PizzaOrder.Where(o => (o.StoreId == storeId)).OrderBy(o => (DateTime.Now - o.OrderTime)));
        }

        public IEnumerable<DBData.Pizza> GetPizzas(decimal orderId)
        {
            return _db.Pizza.Where(p => p.OrderId == orderId);
        }

        /// <summary>
        /// Persist changes to the data source.
        /// </summary>
        public void Save()
        {
            _db.SaveChanges();
        }

    }
}