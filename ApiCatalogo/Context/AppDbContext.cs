﻿using ApiCatalogo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {

        // DbSet is a collection of entities that can be queried, such as tables in a database.
        public DbSet<Category>? Category { get; set; }
        public DbSet<Product>? Product { get; set; }

        // The DbContextOptions parameter is used to configure the context.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }
}
