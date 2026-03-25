using Microsoft.EntityFrameworkCore;

namespace WebUD5

public class ApplicationDbContext : DbContext
{
 public ApplicationDbContext() {}


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}