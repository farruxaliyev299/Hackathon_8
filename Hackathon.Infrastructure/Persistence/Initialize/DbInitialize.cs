using Hackathon.Infrastructure.Persistence.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Hackathon.Infrastructure.Persistence.Initialize
{
    public class DbInitialize : IDbInitialize
    {
        private readonly HackathonDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitialize(HackathonDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            if (_roleManager.FindByNameAsync("Admin").Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole("Admin")).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole("Customer")).GetAwaiter().GetResult();
            }
            else { return; }

            ApplicationUser adminUser = new ApplicationUser()
            {
                UserName = "admin1@gmail.com",
                Email = "admin1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "111111111111",
                FirstName = "Admin",
                LastName = "FullPermission"
            };

            _userManager.CreateAsync(adminUser, "s3cr3tP4ssw0rd").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();

            ApplicationUser customerUser = new ApplicationUser()
            {
                UserName = "customer1@gmail.com",
                Email = "customer1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "111111111111",
                FirstName = "Customer",
                LastName = "SomePermission"
            };

            _userManager.CreateAsync(customerUser, "s3cr3tP4ssw0rd").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customerUser, "Customer").GetAwaiter().GetResult();
        }
    }
}
