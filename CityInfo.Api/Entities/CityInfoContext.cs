using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.Api.Entities
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            : base(options)
        {
            // Force database
            //Database.EnsureCreated();

            Database.Migrate();
        }

        public DbSet<City> Cities { get; set; }

        public DbSet<PointOfInterest> PointOfInterests { get; set; }

        // One option for configuring - but can also pass to constructor!
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("connectionString");

        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Only works with a migration OR EnsureCreated :(
            modelBuilder.Entity<City>().HasData(
                new City()
                {
                    Id = 1,
                    Name = "Leeds",
                    Description = "Capital of yorkshire",
                });

            modelBuilder.Entity<PointOfInterest>().HasData(
                new {Id = 1, CityId =1 , Name = "Royal Armories", Description = "Lots of swords and stuff"});

            modelBuilder.Entity<City>().HasData(
                new City() {Id = 500, Name = "London", Description = "Capital of UK"});

            modelBuilder.Entity<PointOfInterest>().HasData(
                new {Id = 2, CityId = 500 , Name = "Westminister", Description = "Parliement and stuff"});

            base.OnModelCreating(modelBuilder);
        }
    }
}
