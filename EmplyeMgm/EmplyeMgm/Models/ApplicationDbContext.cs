using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace EmplyeMgm.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //seeding the roles and injecting super admin.
        public static async Task SeedRolesAndSuperAdmin(IServiceProvider serviceProvider)
        {
            var roleManager=serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager=serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = { "Admin", "SuperAdmin", "Employee" };
            IdentityResult result;

            foreach (var item in roleNames)
            {
              var roleExist=await roleManager.RoleExistsAsync(item);
                if (!roleExist)
                {
                    result=await roleManager.CreateAsync(new IdentityRole(item));
                }
            }
            var dob = new DateOnly(2001, 3, 12);
            var superadmin = new ApplicationUser
            {
                FirstName="super",
                LastName="Admin",
                
                UserName = "superadmin@gmail.com",
                Email = "superadmin@gmail.com",
                NormalizedEmail = "superadmin@gmail.com",
                City = "Pune",
                DOB=dob,
                IsAdmin=true

            };
            var adminPass = "SuperAdmin@123";
            var user=await userManager.FindByEmailAsync("superadmin@gmail.com");
            if (user == null)
            {
                var createSuperAdmin = await userManager.CreateAsync(superadmin, adminPass);
                if (createSuperAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(superadmin, "SuperAdmin");
                }
            }
            else
            {
                Console.WriteLine("unable to create super admin");
            }

        }
        public DbSet<Employee> Employees { get; set; }
    }
}
