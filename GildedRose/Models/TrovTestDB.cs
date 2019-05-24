using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GildedRose.Models
{
    public class TrovTestDB: DbContext
    {
        public TrovTestDB() : base()
        {
        }
        public TrovTestDB(DbContextOptions options) : base(options)
        {
        }
        public TrovTestDB(DbContextOptions<TrovTestDB> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("TrovTestMockDB");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Entity Relationship already defined by DataAnnotations
            // don't need to setup relationship here
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }


        public void InitializeData()
        {
            this.Customers.Add(new Customer { FirstName = "Bob",    LastName = "Foo", UserName = "user1", Password = "123" });
            this.Customers.Add(new Customer { FirstName = "Thomas", LastName = "Soo", UserName = "user2", Password = "abc" });

            this.Items.Add(new Item { Name = "Apple", Description = "Fine Apple",   Price = 2 });
            this.Items.Add(new Item { Name = "Mango", Description = "Fine Mango",   Price = 6 });
            this.Items.Add(new Item { Name = "Banana", Description = "Fine Banana", Price = 1 });
            this.SaveChanges();
        }
    }
}
