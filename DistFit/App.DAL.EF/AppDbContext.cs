using App.Domain;
using App.Domain.Identity;
using Base.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    private readonly DbContextOptions<AppDbContext> _options;
    
    public DbSet<ExerciseType> ExerciseTypes { get; set; } = default!;
    public DbSet<Goal> Goals { get; set; } = default!;
    public DbSet<Measurement> Measurements { get; set; } = default!;
    public DbSet<MeasurementType> MeasurementTypes { get; set; } = default!;
    public DbSet<Performance> Performances { get; set; } = default!;
    public DbSet<Program> Programs { get; set; } = default!;
    public DbSet<ProgramSaved> ProgramsSaved { get; set; } = default!;
    public DbSet<Session> Sessions { get; set; } = default!;
    public DbSet<SessionExercise> SessionExercises { get; set; } = default!;
    public DbSet<SetEntry> SetEntries { get; set; } = default!;
    public DbSet<Unit> Units { get; set; } = default!;
    public DbSet<UserExercise> UserExercises { get; set; } = default!;
    public DbSet<UserSessionExercise> UserSessionExercises { get; set; } = default!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        _options = options;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory")
        {
            builder
                .Entity<ExerciseType>()
                .Property(e => e.Name)
                .HasConversion(
                    v => SerialiseLangStr(v),
                    v => DeserializeLangStr(v)!);
            builder
                .Entity<MeasurementType>()
                .Property(e => e.Name)
                .HasConversion(
                    v => SerialiseLangStr(v),
                    v => DeserializeLangStr(v)!);
            builder
                .Entity<Program>()
                .Property(e => e.Name)
                .HasConversion(
                    v => SerialiseLangStr(v),
                    v => DeserializeLangStr(v)!);
            builder
                .Entity<Program>()
                .Property(e => e.Description)
                .HasConversion(
                    v => SerialiseLangStr(v),
                    v => DeserializeLangStr(v));
            builder
                .Entity<Unit>()
                .Property(e => e.Name)
                .HasConversion(
                    v => SerialiseLangStr(v),
                    v => DeserializeLangStr(v)!);
            builder
                .Entity<Unit>()
                .Property(e => e.Symbol)
                .HasConversion(
                    v => SerialiseLangStr(v),
                    v => DeserializeLangStr(v)!);
        }

        // Remove cascade delete.
        foreach (var relationship in builder.Model
                     .GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }

    private static string? SerialiseLangStr(LangStr? lStr)
    {
        if (lStr != null)
        {
            return System.Text.Json.JsonSerializer.Serialize(lStr);
        }
        return null;
    }

    private static LangStr? DeserializeLangStr(string? jsonStr)
    {
        if (jsonStr != null)
        {
            return System.Text.Json.JsonSerializer.Deserialize<LangStr>(jsonStr) ?? new LangStr("");
        }
        return null;
    }
    
    public override int SaveChanges()
    {
        FixEntities(this);
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        FixEntities(this);
        return base.SaveChangesAsync(cancellationToken);
    }

    private void FixEntities(DbContext context)
    {
        var dateProperties = context.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?))
            .Select(z => new
            {
                ParentName = z.DeclaringEntityType.Name,
                PropertyName = z.Name
            });

        var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified)
            .Select(x => x.Entity);
        

        foreach (var entity in editedEntitiesInTheDbContextGraph)
        {
            var entityFields = dateProperties.Where(d => d.ParentName == entity.GetType().FullName);

            foreach (var property in entityFields)
            {
                var prop = entity.GetType().GetProperty(property.PropertyName);

                if (prop == null)
                    continue;

                var originalValue = prop.GetValue(entity) as DateTime?;
                if (originalValue == null)
                    continue;

                originalValue = originalValue.Value.ToUniversalTime();
                prop.SetValue(entity, DateTime.SpecifyKind(originalValue.Value, DateTimeKind.Utc));
            }
        } 
    }
}