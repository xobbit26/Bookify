using Bookify.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure;

internal sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    //This method is implemented in DbContext, that's why compiler doesn't require to implement it in the current class
    // public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    // {
    //     var result = await base.SaveChangesAsync(cancellationToken);
    //     return result;
    // }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}