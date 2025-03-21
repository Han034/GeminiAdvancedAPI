using GeminiAdvancedAPI.Domain.Entities.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeminiAdvancedAPI.Persistence.Data
{
    public static class SeedData
    {
        public static async Task SeedRolesAndAdminUserAsync(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                // Rolleri oluştur
                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new AppRole { Name = "Admin" });
                }

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await roleManager.CreateAsync(new AppRole { Name = "User" });
                }

                // Admin kullanıcısını oluştur
                var adminUser = await userManager.FindByEmailAsync("admin@example.com");
                if (adminUser == null)
                {
                    adminUser = new AppUser
                    {
                        UserName = "admin@example.com",
                        Email = "admin@example.com",
                        FirstName = "Admin",
                        LastName = "User",
                        RefreshToken = Guid.NewGuid().ToString(), // RefreshToken ataması
                        RefreshTokenExpiryTime = DateTime.Now.AddDays(7) // RefreshTokenExpiryTime ataması (7 yerine JwtSettings'den okuyabilirsiniz)
                    };
                    await userManager.CreateAsync(adminUser, "Admin.123"); // Şifreye dikkat!

                    // Admin kullanıcısına Admin rolünü ata
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else // Kullanıcı ZATEN VARSA, RefreshToken'ı güncelle:
                {
                    adminUser.RefreshToken = Guid.NewGuid().ToString();
                    adminUser.RefreshTokenExpiryTime = DateTime.Now.AddDays(7); // Veya JwtSettings'den
                    await userManager.UpdateAsync(adminUser); // GÜNCELLEME ÖNEMLİ
                }
            }
        }
    }
}
