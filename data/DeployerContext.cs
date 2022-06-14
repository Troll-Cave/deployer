using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using data.Models;
using Microsoft.EntityFrameworkCore;

namespace data;

public class DeployerContext : DbContext
{
    public DbSet<ApplicationDTO> Applications { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=deployer;Username=postgres;Password=testpwd");
}
