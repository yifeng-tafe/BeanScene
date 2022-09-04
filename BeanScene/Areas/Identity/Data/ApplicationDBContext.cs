using BeanScene.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BeanScene.Models;

namespace BeanScene.Areas.Identity.Data
{

    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<BeanScene.Models.Category> Category { get; set; }
        public DbSet<BeanScene.Models.Food> Food { get; set; }
        public DbSet<Reservation> Reservation { get; set; }
        public DbSet<Role> Role { get; set; }

        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>(c => c.Property(c => c.Id).ValueGeneratedOnAdd());
            builder.Entity<Food>(f => f.Property(f => f.Id).ValueGeneratedOnAdd());
            builder.Entity<Role>(r => r.Property(r => r.Id).ValueGeneratedOnAdd());
            builder.Entity<Role>().HasData(
                    new Role
                    {
                        Id = 1,
                        Name = "Member"
                    },

                    new Role
                    {
                        Id = 2,
                        Name = "Staff"
                    },

                    new Role
                    {
                        Id = 3,
                        Name = "Manager"
                    }
                );

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());

        }

        public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
        {
            public void Configure(EntityTypeBuilder<ApplicationUser> builder)
            {
                builder.Property(u => u.FirstName).HasMaxLength(255);
                builder.Property(u => u.LastName).HasMaxLength(255);

            }
        }

    }
}

    