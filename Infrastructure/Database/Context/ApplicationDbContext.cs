using jonas.Domain.Entities.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace jonas.Infrastructure.Database.Context;

public class ApplicationDbContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .Property(e => e.FirstName).HasColumnType("varchar(50)");
        builder.Entity<User>()
            .Property(e => e.LastName).HasColumnType("varchar(50)");
        builder.Entity<User>()
            .Property(e => e.PhoneNumber).HasColumnType("varchar(50)");
        base.OnModelCreating(builder);
    }
}
