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

        public IEnumerable<User> GetUsers()
        {
            return Mapper.Map(_db.PizzaUser.AsNoTracking().ToList());
        }

        public void AddUser(User u)
        {
            _db.PizzaUser.Add(Mapper.Map(u));
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