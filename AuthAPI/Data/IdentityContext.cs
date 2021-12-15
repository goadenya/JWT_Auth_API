using AuthAPI.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuthAPI.Data
{
    public class IdentityContext : IdentityDbContext<Data.Entities.User>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            const string rootId = "root-0c0-aa65-4af8-bd17-00bd9344e575";

            const string adminId = "admin-c0-aa65-4af8-bd17-00bd9344e575";


            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = rootId,
                Name = "root",
                NormalizedName = "ROOT"
            });

            var hasher = new PasswordHasher<User>();

            builder.Entity<User>().HasData(new User
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@core.api",
                NormalizedEmail = "ADMIN@CORE.API",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "AdminPass1!"),
                SecurityStamp = Guid.NewGuid().ToString(),
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = rootId,
                UserId = adminId
            });
        }
    }
}
