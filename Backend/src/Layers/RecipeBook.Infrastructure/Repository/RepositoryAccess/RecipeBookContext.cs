﻿using Microsoft.EntityFrameworkCore;
using RecipeBook.Domain.Entities;

namespace RecipeBook.Infrastructure.Repository.RepositoryAccess;

public class RecipeBookContext : DbContext
{
    public RecipeBookContext(DbContextOptions<RecipeBookContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Code> Codes { get; set; }
    public DbSet<Connection> Connections { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeBookContext).Assembly);
    }
}
