
using EmplyeMgm.Models;
using EmplyeMgm.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmplyeMgm.Service
{
    public class AdminService : IAdminService
    {
            private readonly ApplicationDbContext _context;
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public AdminService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
            {
                _context = context;
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<IEnumerable<ApplicationUser>> GetEmployeesAsync()
            {
            try
            {
                return await _userManager.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving employees.", ex);
            }
            }


            public async Task<ApplicationUser> GetEmployeeByIdAsync(string id)
            {
            try
            {
                return await _context.Emp.FindAsync(id);
            }
            catch (Exception ex)
            {

                throw new ApplicationException($"An error occurred while retrieving the employee with ID {id}.", ex);
            }

            }


        public async Task CreateEmployeeAsync(ApplicationUser employee, string pass)
        {
            try
            {
             
                var result = await _userManager.CreateAsync(employee, pass);

                if (result.Succeeded)
                {
                    var role = employee.IsAdmin ? "Admin" : "Employee";
                    await _userManager.AddToRoleAsync(employee, role);
                }
                else
                {
                    throw new ApplicationException("Failed to create user account.");
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the employee.", ex);
            }
        }

            public async Task UpdateEmployeeAsync(ApplicationUser employee)
            {
            try
            {
               //_context.Update(employee);

               //await _context.SaveChangesAsync();
                var user = await _userManager.FindByEmailAsync(employee.Email);
                if (user == null)
                {
                    throw new KeyNotFoundException("User not found.");
                }
                else
                {
                    user.FirstName = employee.FirstName;
                    user.LastName = employee.LastName;
                    user.Email = employee.Email;
                    user.City=employee.City;
                    user.IsAdmin = employee.IsAdmin;
                    user.UserName = employee.Email;
                    user.NormalizedEmail = employee.Email;
                    user.NormalizedUserName = employee.Email;
                    user.DOB = employee.DOB;
                    await _userManager.UpdateAsync(user);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the employee.", ex);
            }
            }

            public async Task DeleteEmployeeAsync(string id)
            {
            try
            {
                //var employee = await _context.Emp.FindAsync(id);
                var employee = await _userManager.FindByIdAsync(id);
                if (employee == null)
                {
                    throw new KeyNotFoundException("Employee Not Found");
                }
                if (employee != null)
                {
                    await _userManager.DeleteAsync(employee);
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured while deleting the employee.", ex);
            }
            }

            public async Task<bool> EmployeeExists(string id)
            {
            try
            {
                //  return _context.Employees.Any(e => e.Id == id);
                var emp= await _userManager.FindByIdAsync(id);
                if(emp!= null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured while checking if the employee exists",ex);   
            }
            }

        public async Task<IdentityResult> ChangePass(ChangePassViewModel model, ApplicationUser user)
        {
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            return result;

        }
    }
}
