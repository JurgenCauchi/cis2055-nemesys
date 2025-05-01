using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nemesys.Models;

namespace Nemesys.Data
{
    public class NemesysContext : IdentityDbContext<AppUser> // Inherit from IdentityDbContext
    {
        public NemesysContext(DbContextOptions<NemesysContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            AppUser user = new AppUser()
            {
                Id = "134c1566-3f64-4ab4-b1e7-2ffe11f43e32", //https://www.guidgenerator.com/online-guid-generator.aspx
                AuthorAlias = "test",
                UserName = "admin@mail.com", //Has to be the email address for the login logic to work
                NormalizedUserName = "ADMIN@MAIL.COM ",
                Email = "admin@mail.com",
                NormalizedEmail = "ADMIN@MAIL.COM",
                LockoutEnabled = false,
                EmailConfirmed = true,
                PhoneNumber = "", 
                ConcurrencyStamp = "fd2ed7ec-6b31-4c6a-8fb3-46d892ad21d9",
                SecurityStamp = "872d1448-a4f1-442b-bae3-8523c6ec4902",
                Role = "Investigator",
            };
            user.PasswordHash = "AQAAAAIAAYagAAAAEP5xsWE39BRAj4jsqB9jWP23dDEzAEq8UJvSVzsl4Wk5S5JUKD115nEpnc4cgkYnpQ==";

            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
            //user.PasswordHash = passwordHasher.HashPassword(user, "Password.1");

            modelBuilder.Entity<AppUser>().HasData(user);

            modelBuilder.Entity<Category>().HasData(
                new Category()
                {
                    Id = 1,
                    Name = "Uncategorised"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Comedy"
                },
                new Category()
                {
                    Id = 3,
                    Name = "News"
                }
            );

        }

        // DbSet properties should be public and virtual
        public  DbSet<ReportPost> ReportPosts { get; set; } // Pluralized name convention
        public  DbSet<Category> Categories { get; set; } // Changed to DbSet<T>
    }
}