using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Application.Common.Utility;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Infrastructure.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DbInitializer(ApplicationDbContext db)
        {
            _db = db;
        }
        public void Initialize()
        {
            try
            {
                 if (_db.Database.GetPendingMigrations().Any())
                 {
                     _db.Database.Migrate();
                 }

                 if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                 {
                     _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait(); 
                     _userManager.CreateAsync(new ApplicationUser
                                          {
                                           UserName = "admin@hi2.in",
                                           Email = "admin@hi2.in",
                                           Name = "Shahab Admin",
                                           NormalizedUserName = "SHAHAB ADMIN",
                                           NormalizedEmail = "ADMIN@HI2.IN",
                                           PhoneNumber = "09177388400"
                                           },"az*96321");
                 ApplicationUser user = _db.Users.FirstOrDefault(u => u.NormalizedEmail == "ADMIN@HI2.IN");
                 _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
                 }
                 if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
                 { _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait(); }



            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
