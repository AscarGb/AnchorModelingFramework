using AnchorModeling.Extensions;
using AnchorModeling.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WikiConvensionExampleModels
{
    public class MyDbContext : AnchorModelContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = "Server=ASUS;Database=TestDb;Integrated Security=true";
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.SetFKsRestrict();
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SomeEntity> SomeEntityTableName { get; set; }
        public DbSet<AnotherEntity> AnotherEntityTableName { get; set; }
    }
}
