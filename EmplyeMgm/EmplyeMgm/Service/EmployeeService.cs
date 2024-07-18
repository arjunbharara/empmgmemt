
using EmplyeMgm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmplyeMgm.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeeService(ApplicationDbContext context,UserManager<ApplicationUser>userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task CreateEmployeeAsync(Employee employee, string pass)
        {
            _context.Add(employee);
            await _context.SaveChangesAsync();
            var user = new ApplicationUser { FirstName = employee.FirstName, LastName = employee.LastName, UserName = employee.Emial, Email = employee.Emial, IsAdmin = employee.IsAdmin };
            var result = await _userManager.CreateAsync(user, pass);

            if (result.Succeeded)
            {
                if (employee.IsAdmin)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "Employee");
                }
            }
            
        }

        public async Task UpdateEmployeeAsync(Employee employee)
        {
            _context.Update(employee);
           
            await _context.SaveChangesAsync();
            var user = await _userManager.FindByEmailAsync(employee.Emial);
            if (user == null)
            {
                return;
            }
            else
            {
                user.FirstName = employee.FirstName;
                user.LastName = employee.LastName;
                user.Email = employee.Emial;
                user.IsAdmin = employee.IsAdmin;
                user.UserName = employee.Emial;
                user.NormalizedEmail = employee.Emial;
                user.NormalizedUserName = employee.Emial;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            var user = await _userManager.FindByEmailAsync(employee.Emial);
            if (employee != null)
            {
               
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                if(user != null)
                {
                    await _userManager.DeleteAsync(user);
                }

            }
        }

        public bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
