using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BitWiseBlog.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
        }

        //2 roles reader and writer role  reader for public users and writer for admin
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "c5e4d8fd-cf50-4350-ab53-883ca31cccea";
            var writerRoleId = "03add699-fdbb-4321-b1dd-802452b9b1f3";


            //create reader and writer role
            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id=writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp=writerRoleId
                }
            };

            //Seed the roles
            builder.Entity<IdentityRole>().HasData(roles);

            //create an admin user
            var adminUserId = "3e0f551e-ef48-4129-94c8-a04a15b9b95b";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@bitwiseblog.com",
                Email = "admin@bitwiseblog.com",
                NormalizedEmail = "admin@bitwiseblog.com".ToUpper(),
                NormalizedUserName = "admin@bitwiseblog.com".ToUpper()
            };

            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
            //seeding the admin user
            builder.Entity<IdentityUser>().HasData(admin);

            //giveroles to admin
            //admin will have both the roles reader and writer
            var adminRole = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId= readerRoleId
                },
                 new()
                {
                    UserId = adminUserId,
                    RoleId= writerRoleId
                }
            };
            //seed this
            builder.Entity<IdentityUserRole<string>>().HasData(adminRole);
        }
    }
}
