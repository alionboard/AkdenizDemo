using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MRCase.Core.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRCase.EntityFrameworkCore.Configurations
{
    public class AdminConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        private const string adminId = "B22698B8-42A2-4115-9631-1C2D1E2AC5F7";
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            var admin = new ApplicationUser
            {
                Id = adminId,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true,
                SecurityStamp = new Guid().ToString("D")
            };

            admin.PasswordHash = PassGenerate(admin);

            builder.HasData(admin);
        }

        private string PassGenerate(ApplicationUser admin)
        {
            var passHash = new PasswordHasher<ApplicationUser>();
            return passHash.HashPassword(admin, "Password1.");
        }
    }
}
