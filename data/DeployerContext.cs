using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using data.DataModels;
using data.Models;
using Microsoft.EntityFrameworkCore;

namespace data;

public class DeployerContext : DbContext
{
    public DbSet<ApplicationDTO> Applications { get; set; }
    public DbSet<ConfigDTO> Configs { get; set; }
    public DbSet<OrganizationDTO> Organizations { get; set; }
    public DbSet<PipelineDTO> Pipelines { get; set; }
    public DbSet<PipelineVersionDTO> PipelineVersions { get; set; }

    /// <summary>
    /// Move this to startup later
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=deployer;Username=postgres;Password=testpwd");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PipelineDTO>()
            .HasMany<PipelineVersionDTO>()
            .WithOne()
            .HasForeignKey(x => x.PipelineId);
        
        modelBuilder.Entity<ApplicationDTO>()
            .HasOne<PipelineVersionDTO>()
            .WithMany()
            .HasForeignKey(x => x.PipelineVersionId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<OrganizationDTO>()
            .HasMany<ApplicationDTO>()
            .WithOne()
            .HasForeignKey(x => x.Organization)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<OrganizationDTO>()
            .HasMany<PipelineDTO>()
            .WithOne()
            .HasForeignKey(x => x.Organization)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
