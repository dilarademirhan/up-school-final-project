using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Security.Principal;
using FinalProject.Domain.Entities;
using FinalProject.Persistence.EntityFramework.Configurations;
//using FinalProject.Persistence.EntityFramework.Seeders;

namespace FinalProject.Persistence.EntityFramework.Contexts
{
    public class FinalProjectDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }


        public FinalProjectDbContext(DbContextOptions<FinalProjectDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurations
            modelBuilder.ApplyConfiguration(new ProductConfiguration());

            // Seeds
          //  modelBuilder.ApplyConfiguration(new ProductSeeder());

            base.OnModelCreating(modelBuilder);
        }
    }
}