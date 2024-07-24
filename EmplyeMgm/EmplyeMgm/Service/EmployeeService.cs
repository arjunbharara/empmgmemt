﻿
using EmplyeMgm.Models;
using EmplyeMgm.ViewModel;
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

        public async Task<ApplicationUser> GetEmployeesAsync(ClaimsPrincipal user)
        {
            try
            {
               // var listOfEmployees = await _userManager.Users.ToListAsync();
                //var listOfEmployees = await _context.Employees.ToListAsync();
                return  await _userManager.GetUserAsync(user);
              /*  if (emp != null)
                {
                    foreach (var item in listOfEmployees)
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
                                Id = item.Id
                            };
                            return employee;
                        }
                    }*/
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving employees.", ex);
            }

            return null; 
        }

        public async Task<ApplicationUser> GetEmployeeByIdAsync(string id)
        {
            try
            {
                return await _userManager.FindByIdAsync(id);
            } catch (Exception ex) {
                {

                    throw new ApplicationException($"An error occurred while retrieving the employee with ID {id}.", ex);
                }
            }
        }

       /* public async Task CreateEmployeeAsync(Employee employee, string pass)
        {
            try { 
            _context.Add(employee);
            await _context.SaveChangesAsync();
            var user = new ApplicationUser { FirstName = employee.FirstName,City=employee.City,DOB=employee.DOB, LastName = employee.LastName, UserName = employee.Emial, Email = employee.Emial, IsAdmin = employee.IsAdmin };
                var result = await _userManager.CreateAsync(user, pass);
                    if (result.Succeeded)
                    {
                        var role = employee.IsAdmin ? "Admin" : "Employee";
                        await _userManager.AddToRoleAsync(user, role);
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
        }*/

        public async Task UpdateEmployeeAsync(ApplicationUser employee)
        {
            try { 
           /* _context.Update(employee);
           
            await _context.SaveChangesAsync();*/
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
                user.IsAdmin = employee.IsAdmin;
                user.City = employee.City;
                user.DOB = employee.DOB;
                user.UserName = employee.Email;
                user.NormalizedEmail = employee.Email;
                user.NormalizedUserName = employee.Email;
                await _userManager.UpdateAsync(user);
            }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating the employee.", ex);
            }
        }

        public async Task<bool> EmployeeExists(string id)
        {
            try
            {
                var user= await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured while checking if the employee exists", ex);
            }
        }

        public async Task<IdentityResult> ChangePass(ChangePassViewModel model,ApplicationUser user)
        {
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            return result;
          
        }
    }
}
