
using EmplyeMgm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public async Task<Employee> GetEmployeesAsync(ClaimsPrincipal user)
        {
            var listOfEmployees = await _context.Employees.ToListAsync();
            var emp= await _userManager.GetUserAsync(user);
            if (emp != null)
            {
                foreach(var item in listOfEmployees)
                {
                    if (item.Emial == emp.Email)
                    {
                        var employee = new Employee
                        {
                            City = emp.City,
                            FirstName = emp.FirstName,
                            LastName = emp.LastName,
                            DOB = emp.DOB,
                            Emial = emp.Email,
                            IsAdmin = emp.IsAdmin,
                            Id=item.Id
                        };
                        return employee;
                    }
                } 
            }

            return null;
             
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task CreateEmployeeAsync(Employee employee, string pass)
        {
            _context.Add(employee);
            await _context.SaveChangesAsync();
            var user = new ApplicationUser { FirstName = employee.FirstName,City=employee.City,DOB=employee.DOB, LastName = employee.LastName, UserName = employee.Emial, Email = employee.Emial, IsAdmin = employee.IsAdmin };
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
