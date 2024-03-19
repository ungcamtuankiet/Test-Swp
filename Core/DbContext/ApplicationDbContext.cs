using be_artwork_sharing_platform.Core.Constancs;
using be_artwork_sharing_platform.Core.Dtos.User;
using be_artwork_sharing_platform.Core.Entities;
using be_project_swp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Reflection.Emit;

namespace be_artwork_sharing_platform.Core.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Log> Logs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<ApplicationUser> Users {  get; set; }
        public DbSet<Favourite> Favorites { get; set; }
        public DbSet<RequestOrder> RequestOrders { get; set; }
/*        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order> Orders { get; set; }*/

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //1
            builder.Entity<ApplicationUser>(e =>
            {
                e.ToTable("users");
                e.HasIndex(e => e.Email).IsUnique();
            });
            //2
            builder.Entity<IdentityUserClaim<string>>(e =>
            {
                e.ToTable("userclaims");
            });
            //3
            builder.Entity<IdentityUserLogin<string>>(e =>
            {
                e.ToTable("userlogins");
            });
            //4
            builder.Entity<IdentityUserToken<string>>(e =>
            {
                e.ToTable("usertokens");
            });
            //5
            builder.Entity<IdentityRole>(e =>
            {
                e.ToTable("roles");
            });
            //6
            builder.Entity<IdentityRoleClaim<string>>(e =>
            {
                e.ToTable("roleclaims");
            });
            //7
            builder.Entity<IdentityUserRole<string>>(e =>
            {
                e.ToTable("userroles");
            });

            builder.Entity<Log>(e =>
            {
                e.ToTable("logs");
            });

            builder.Entity<Artwork>()
                .HasOne(a => a.User)
                .WithMany(u => u.Artworks)
                .HasForeignKey(a => a.User_Id);

            builder.Entity<Favourite>()
                .HasOne(f => f.User)
                .WithMany(f => f.Favorites)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Favourite>()
                .HasOne(f => f.Artworks)
                .WithMany(f => f.Favourites)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
