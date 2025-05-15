using Banderas.Web.Business.UserInfo;
using Banderas.Web.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Banderas.Web.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IFlagUserDetails userDetails) : IdentityDbContext(options)
    {
        public DbSet<FlagEntity> Flags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FlagEntity>()
                .HasQueryFilter(f => !f.IsDeleted // Apply global filter to exclude soft-deleted entities
                && f.UserId == userDetails.UserId); // Filter by the current user's ID

            base.OnModelCreating(builder);
        }
    }
}
