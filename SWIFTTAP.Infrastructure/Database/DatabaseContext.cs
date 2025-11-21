using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWIFTTAP.Domain.Administration;
using SWIFTTAP.Domain.Base;
using SWIFTTAP.Domain.Cards;
using SWIFTTAP.Domain.ContactForms;
using SWIFTTAP.Domain.Statistics;
using SWIFTTAP.Domain.System;

namespace SWIFTTAP.Infrastructure.Database;

public class DatabaseContext : IdentityDbContext<User, IdentityRole<long>, long>
{
    private bool _updateAuditableSubjectProperties = true;

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {  
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected DatabaseContext()
    {
    }
    public new DbSet<User> Users { get; set; }
    public DbSet<DeletedUser> DeletedUsers { get; set; }
    public DbSet<UserKeys> UserKeys { get; set; }
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public DbSet<Translation> Translations { get; set; }

    public DbSet<Link> Links => Set<Link>();
    public DbSet<LinkIcon> LinkIcons => Set<LinkIcon>();
    public DbSet<Theme> Themes => Set<Theme>();
    public DbSet<Logo> Logos => Set<Logo>();
    public DbSet<Card> Cards => Set<Card>();

    public DbSet<ContactForm> ContactForms => Set<ContactForm>();

    public DbSet<CardVisitStatistic> CardVisitStatistics => Set<CardVisitStatistic>();
    public DbSet<CardVisitGeoStatistic> CardVisitGeoStatistics => Set<CardVisitGeoStatistic>();
    public DbSet<LinkVisitStatistic> LinkVisitStatistics => Set<LinkVisitStatistic>();
    public DbSet<UserCountStatistic> UserCountStatistics => Set<UserCountStatistic>();
    public DbSet<VcfDownloadStatistic> VcfDownloadStatistics => Set<VcfDownloadStatistic>();

    // Add sets here...

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(DependencyInjection).Assembly);

        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users", "Administration");
        builder.Entity<IdentityUserClaim<long>>().ToTable("UserClaims", "Administration");
        builder.Entity<IdentityUserLogin<long>>().ToTable("UserLogins", "Administration");
        builder.Entity<IdentityUserToken<long>>().ToTable("UserTokens", "Administration");
        builder.Entity<IdentityRole<long>>().ToTable("Roles", "Administration");
        builder.Entity<IdentityRoleClaim<long>>().ToTable("RoleClaims", "Administration");
        builder.Entity<IdentityUserRole<long>>().ToTable("UserRoles", "Administration");
    }

    internal void ChangeAuditableSubjectUpdateBehaviour(bool updateAuditableSubjectProperties)
    {
        _updateAuditableSubjectProperties = updateAuditableSubjectProperties;
    }

    public override int SaveChanges()
    {
        HandleAuditableEntityChanges();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        HandleAuditableEntityChanges();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HandleAuditableEntityChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        HandleAuditableEntityChanges();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void HandleAuditableEntityChanges()
    {
        if (!_updateAuditableSubjectProperties)
            return;

        var entities = ChangeTracker
            .Entries()
            .Where(entry => entry.Entity is IAuditableEntity &&
                (entry.State == EntityState.Added || entry.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            ((IAuditableEntity)entity.Entity).ModifiedAt = DateTime.UtcNow;

            if (entity.State == EntityState.Added)
            {
                ((IAuditableEntity)entity.Entity).CreatedAt = DateTime.UtcNow;
            }
        }
    }
}
