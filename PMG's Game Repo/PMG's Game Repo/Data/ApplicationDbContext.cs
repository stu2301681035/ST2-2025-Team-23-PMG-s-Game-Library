using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMG_s_Game_Repo.Models;

namespace PMG_s_Game_Repo.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<UserGame> UserGames { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var hasher = new PasswordHasher<User>();

            var admin1 = new User
            {
                Id = "1a2b3c4d-5e6f-7g8h-9i0j-1k2l3m4n5o6p",
                UserName = "georgeadmin",
                NormalizedUserName = "GEORGEADMIN",
                Email = "georgeadmin@pmggamerepo.com",
                NormalizedEmail = "GEORGEADMIN@PMGGAMEREPO.COM",
                EmailConfirmed = true,
                IsAdmin = true,
                IsBanned = false,
                ProfilePictureUrl = "https://i.ibb.co/2Wj9WzN/default-avatar.png",
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "George123!")
            };

            var admin2 = new User
            {
                Id = "2b3c4d5e-6f7g-8h9i-0j1k-2l3m4n5o6p7q",
                UserName = "miroslavadmin",
                NormalizedUserName = "MIROSLAVADMIN",
                Email = "miroslavadmin@pmggamerepo.com",
                NormalizedEmail = "MIROSLAVADMIN@PMGGAMEREPO.COM",
                EmailConfirmed = true,
                IsAdmin = true,
                IsBanned = false,
                ProfilePictureUrl = "https://i.ibb.co/2Wj9WzN/default-avatar.png",
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "Miroslav123!")
            };

            var admin3 = new User
            {
                Id = "3c4d5e6f-7g8h-9i0j-1k2l-3m4n5o6p7q8r",
                UserName = "petaradmin",
                NormalizedUserName = "PETARADMIN",
                Email = "petaradmin@pmggamerepo.com",
                NormalizedEmail = "PETARADMIN@PMGGAMEREPO.COM",
                EmailConfirmed = true,
                IsAdmin = true,
                IsBanned = false,
                ProfilePictureUrl = "https://i.ibb.co/2Wj9WzN/default-avatar.png",
                SecurityStamp = Guid.NewGuid().ToString(),
                PasswordHash = hasher.HashPassword(null, "Petar123!")
            };

            builder.Entity<User>().HasData(admin1, admin2, admin3);

    
        }
    }
}
