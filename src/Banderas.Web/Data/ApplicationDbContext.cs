using Banderas.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Banderas.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<FlagEntity> Flags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FlagEntity>()
                .HasQueryFilter(f => !f.IsDeleted); // Apply global filter to exclude soft-deleted entities

            base.OnModelCreating(builder);
        }
    }
}
