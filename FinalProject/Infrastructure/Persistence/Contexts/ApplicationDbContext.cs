using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderEvent> OrderEvents { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.Ignore<RoleClaim>();
            modelBuilder.Ignore<Role>();
            modelBuilder.Ignore<UserClaim>();
            modelBuilder.Ignore<UserLogin>();
            modelBuilder.Ignore<UserRole>();
            modelBuilder.Ignore<User>();
            modelBuilder.Ignore<UserToken>();


            base.OnModelCreating(modelBuilder);
        }
    }
}
