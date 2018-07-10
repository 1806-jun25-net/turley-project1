using Microsoft.EntityFrameworkCore;
using PizzaPlanet.DBData;
using System;
using System.Collections.Generic;
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
                throw new Exception("Repository never instantiated.");
            return RepoReal;
        }

        /// <summary>
        /// instantiates the real repository with the given context
        /// </summary>
        /// <param name="db"></param>
        public static void OpenRepository(Project1PizzaPlanetContext db)
        {
            RepoReal = new PizzaRepository(db);
        }

        private PizzaRepository(Project1PizzaPlanetContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <summary>
        /// Get all stores/locations
        /// </summary>
        /// <returns>The collection of Locations (stores)</returns>
        public IEnumerable<Location> GetLocations()
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
            var pizzas = new Dictionary<int,DBData.Pizza>();
            for(int i = 0; i < o.NumPizza; i++)
            {
                int code = o.Pizzas[i].ToInt();
                if (pizzas.Keys.Contains(code))
                    pizzas[code].Quantity++;
                else
                {
                    var dbpizza = new DBData.Pizza
                    { Quantity=1, OrderId = o.IdFull(), Code = code };
                    pizzas.Add(code, dbpizza);
                }
            }
            foreach (DBData.Pizza DBp in pizzas.Values)
                _db.Pizza.Add(DBp);
            _db.Store.Update(Mapper.Map(o.Store));
            Save();
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