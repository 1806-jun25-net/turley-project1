using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using PizzaPlanet.DBData;

namespace PizzaPlanet.Application
{
    public class Program
    {
        static void Main(string[] args)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = configBuilder.Build();
            var optionsBuilder = new DbContextOptionsBuilder<Project1PizzaPlanetContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("PizzaPlanet"));
            var options = optionsBuilder.Options;
            //new Repo(new YourAppDbContext(options))
            
            //start console app
            Console.StartScreen();
        }
    }
}
