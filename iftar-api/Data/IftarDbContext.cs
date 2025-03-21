using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace iftar_api.Data;

public class IftarDbContext(DbContextOptions<IftarDbContext> options, IHttpContextAccessor httpContextAccessor)
    : DbContext(options)
{

    public DbSet<Iftar> Iftars { get; set; }
    public DbSet<User> Users { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Soft delete pour les iftars
        modelBuilder.Entity<Iftar>().HasQueryFilter(i => i.DeletedAt == null);

        // Conversion des dates en UTC
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
            v => v.ToUniversalTime(),
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
        );

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
            }
        }
    }

    public override int SaveChanges()
    {
        var user = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Anonymous";

        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.CurrentValues["CreatedAt"] = DateTime.UtcNow;
                    entry.CurrentValues["CreatedBy"] = user;
                    break;
                case EntityState.Modified:
                    entry.CurrentValues["UpdatedAt"] = DateTime.UtcNow;
                    entry.CurrentValues["UpdatedBy"] = user;
                    break;
                case EntityState.Deleted:
                    entry.CurrentValues["DeletedAt"] = DateTime.UtcNow;
                    entry.CurrentValues["DeletedBy"] = user;
                    entry.State = EntityState.Modified;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return base.SaveChanges();
    }
}