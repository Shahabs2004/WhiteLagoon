using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Domain.Entities ;

namespace WhiteLagoon.Inrfastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }

        public DbSet<Amenity> Amenities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Description = "Luxurious villa fit for royalty.",
                    ImageUrl = "https://placehold.co/600x400",
                    Occupancy = 4,
                    Price = 200,
                    Sqft = 550,
                    Created_Date = DateTime.Now,
                    Update_Date = DateTime.Now
                },
                new Villa
                {
                    Id = 2,
                    Name = "Premium Pool Villa",
                    Description = "Exquisite villa with a private pool.",
                    ImageUrl = "https://placehold.co/600x401",
                    Occupancy = 4,
                    Price = 300,
                    Sqft = 550,
                    Created_Date = DateTime.Now,
                    Update_Date = DateTime.Now
                },
                new Villa
                {
                    Id = 3,
                    Name = "Luxury Pool Villa",
                    Description = "Opulent villa with a spacious pool.",
                    ImageUrl = "https://placehold.co/600x402",
                    Occupancy = 4,
                    Price = 400,
                    Sqft = 750,
                    Created_Date = DateTime.Now,
                    Update_Date = DateTime.Now
                }
            );
            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber
                {
                    Villa_Number = 101,
                    VillaId = 1
                },
                new VillaNumber
                {
                    Villa_Number = 102,
                    VillaId = 1
                },
                new VillaNumber
                {
                    Villa_Number = 201,
                    VillaId = 2
                },
                new VillaNumber
                {
                    Villa_Number = 202,
                    VillaId = 2
                },
                new VillaNumber
                {
                    Villa_Number = 301,
                    VillaId = 3
                },
                new VillaNumber
                {
                    Villa_Number = 302,
                    VillaId = 3
                }
            );
            modelBuilder.Entity<Amenity>().HasData(
                new Amenity
                {
                    Id = 1,
                    VillaId = 1,
                    Name = "Private Pool"
                },
                new Amenity
                {
                    Id = 2,
                    VillaId = 1,
                    Name = "Ocean View"
                },
                new Amenity
                {
                    Id = 3,
                    VillaId = 1,
                    Name = "Garden"
                },
                new Amenity
                {
                    Id = 4,
                    VillaId = 1,
                    Name = "Jacuzzi"
                },
                new Amenity
                {
                    Id = 5,
                    VillaId = 2,
                    Name = "Private Pool"
                },
                new Amenity
                {
                    Id = 6,
                    VillaId = 2,
                    Name = "Ocean View"
                },
                new Amenity
                {
                    Id = 7,
                    VillaId = 2,
                    Name = "Garden"
                },
                new Amenity
                {
                    Id = 8,
                    VillaId = 2,
                    Name = "Jacuzzi"
                },
                new Amenity
                {
                    Id = 9,
                    VillaId = 3,
                    Name = "Private Pool"
                },
                new Amenity
                {
                    Id = 10,
                    VillaId = 3,
                    Name = "Ocean View"
                },
                new Amenity
                {
                    Id = 11,
                    VillaId = 3,
                    Name = "Garden"
                },
                new Amenity
                {
                    Id = 12,
                    VillaId = 3,
                    Name = "Jacuzzi"
                }
            );
        }
    }

}
