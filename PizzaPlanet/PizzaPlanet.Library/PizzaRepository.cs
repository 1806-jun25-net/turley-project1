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
        

        public PizzaRepository(Project1PizzaPlanetContext db)
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
        /// Persist changes to the data source.
        /// </summary>
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}