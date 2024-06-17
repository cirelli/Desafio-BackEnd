using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infraestructure.Context;

public class DataContext(DbContextOptions<DataContext> options)
    : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Motorbike> Motorbikes { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<RentPlan> RentPlans { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid> { Id = new Guid("B0E04D58-7E5D-4CF9-8C7E-08EE031DEA77"), Name = "Admin", NormalizedName = "ADMIN" });
        modelBuilder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid> { Id = new Guid("81457630-EBB3-489D-AD9C-DEC899E231C9"), Name = "Driver", NormalizedName = "DRIVER" });

        modelBuilder.Entity<User>().HasData(new User() { Id = new Guid("F3CC7224-423A-4650-914E-F0766A4DDE3D"), UserName = "Admin", NormalizedUserName = "ADMIN", PasswordHash = "AQAAAAIAAYagAAAAEHO3xPFrhzqC0bZ131JxLyFdcU+XbxorAkAb6s2lacPndmK/rTTdla4b4/YC+R+cTg==", ConcurrencyStamp = new Guid("894E125D-220D-4DB3-BB96-669C801E2BD2").ToString(), SecurityStamp = "NYLLGA7J6F3UX3OE3GRZVNJUPXZHXRGB" });

        modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid> { RoleId = new Guid("B0E04D58-7E5D-4CF9-8C7E-08EE031DEA77"), UserId = new Guid("F3CC7224-423A-4650-914E-F0766A4DDE3D") });

        modelBuilder.Entity<RentPlan>().HasData(new RentPlan() { Id = new Guid("CE5E3D9C-B16F-4A2C-BD57-9F75A4A25650"), Days = 7, Price = 30, Fee = 20, AdditionalDailyPrice = 50, CreatedAt = new DateTime(2024, 6, 16, 17, 7, 55, 8, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 6, 16, 17, 7, 55, 8, DateTimeKind.Utc) });
        modelBuilder.Entity<RentPlan>().HasData(new RentPlan() { Id = new Guid("426BF15E-4CF2-4713-AF96-F800C7215CFA"), Days = 15, Price = 28, Fee = 40, AdditionalDailyPrice = 50, CreatedAt = new DateTime(2024, 6, 16, 17, 7, 55, 8, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 6, 16, 17, 7, 55, 8, DateTimeKind.Utc) });
        modelBuilder.Entity<RentPlan>().HasData(new RentPlan() { Id = new Guid("D507B101-DCDB-4D41-B85F-0C4E80AD15C7"), Days = 30, Price = 22, Fee = 60, AdditionalDailyPrice = 50, CreatedAt = new DateTime(2024, 6, 16, 17, 7, 55, 8, DateTimeKind.Utc), UpdatedAt = new DateTime(2024, 6, 16, 17, 7, 55, 8, DateTimeKind.Utc) });
    }
}
