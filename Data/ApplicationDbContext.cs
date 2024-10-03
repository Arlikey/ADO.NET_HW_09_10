using ADO.NET_HW_09_10.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_HW_09_10.Data
{
    internal class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public ApplicationDbContext() => Database.EnsureCreated();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(u =>
            {
                u.Property(u => u.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<Book>(b =>
            {
                b.Property(b => b.Title).HasMaxLength(50).IsRequired();
                b.Property(b => b.Author).HasMaxLength(50).IsRequired();
                b.ToTable(b => b.HasCheckConstraint("Genre", $"Genre >= {(int)Genres.Fantasy} AND Genre <= {(int)Genres.Horror}"));
                b.Property(b => b.Year).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
