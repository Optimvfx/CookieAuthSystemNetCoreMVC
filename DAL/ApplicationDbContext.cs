using DAL.Entities;
using DAL.Entities.Configuration;
using DAL.Entities.Configuration.Context;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public bool IsInitialized()
    {
        return Users.Any() || Roles.Any() || Tasks.Any();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserContextConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}