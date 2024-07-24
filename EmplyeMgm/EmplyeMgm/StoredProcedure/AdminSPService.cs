using EmplyeMgm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EmplyeMgm.StoredProcedure
{
    public class AdminSPService : IAdminSPService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public AdminSPService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Employee>> GetAlllEmployees()
        {
            return await _context.Employees.FromSqlRaw("EXEC GetEmployees").ToListAsync();
        }

        public async Task CreateEmployee(Employee employee, string pass)
        {
            var parameters = new[]
           {
                new SqlParameter("@Id", SqlDbType.Int){ Value = 0 },
                new SqlParameter("@FirstName", employee.FirstName),
                new SqlParameter("@LastName", employee.LastName),
                new SqlParameter("@Email", employee.Emial),
                new SqlParameter("@DOB", employee.DOB),
                new SqlParameter("@City", employee.City),
                new SqlParameter("@IsAdmin", employee.IsAdmin)
            };

            _context.Database.ExecuteSqlRaw("EXEC CreateOrUpdateEmployee @Id, @FirstName, @LastName, @Email, @DOB, @City, @IsAdmin", parameters);

            var user = new ApplicationUser { FirstName = employee.FirstName, City = employee.City, DOB = employee.DOB, LastName = employee.LastName, UserName = employee.Emial, Email = employee.Emial, IsAdmin = false };
  
           var result =  await _userManager.CreateAsync(user, pass);
            
            if (result.Succeeded)
            {
                var role = employee.IsAdmin ? "Admin" : "Employee";
                await _userManager.AddToRoleAsync(user, role);
            }
            return;
        }

        public async Task UpdateEmployee(Employee employee)
        {
                var parameters = new[]
                {
                     new SqlParameter("@Id", SqlDbType.Int) { Value = employee.Id },
                     new SqlParameter("@FirstName", employee.FirstName),
                     new SqlParameter("@LastName", employee.LastName),
                     new SqlParameter("@Email", employee.Emial),
                     new SqlParameter("@DOB", employee.DOB),
                     new SqlParameter("@City", employee.City),
                     new SqlParameter("@IsAdmin", employee.IsAdmin)
                 };

                _context.Database.ExecuteSqlRaw("EXEC CreateOrUpdateEmployee @Id, @FirstName, @LastName, @Email, @DOB, @City, @IsAdmin", parameters);

                var user = await _userManager.FindByEmailAsync(employee.Emial);
                if (user != null)
                {
                    user.FirstName = employee.FirstName;
                    user.LastName = employee.LastName;
                    user.City = employee.City;
                    user.DOB = employee.DOB;
                    user.UserName = employee.Emial;
                    user.Email = employee.Emial;
                    user.IsAdmin = employee.IsAdmin;

                     await _userManager.UpdateAsync(user);
                }
                return;
            }

        public async Task  DeleteEmployee(int Id)
        {
            var parameters = new[]
            {
                 new SqlParameter("@Id", SqlDbType.Int) { Value = Id }
            };
            Employee employee = await GetEmployeeById(Id);
            _context.Database.ExecuteSqlRaw("EXEC DeleteEmployee @Id", parameters);

           // Employee employee=await GetEmployeeById(Id);
            var user = await _userManager.FindByEmailAsync(employee.Emial);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            return;
        }

        public async Task<Employee> GetEmployeeById(int id)
        {
            var parameter = new SqlParameter("@Id", SqlDbType.Int) { Value = id };
            var employee = await _context.Employees.FromSqlRaw("EXEC GetEmployees @Id", parameter).ToListAsync();
            return employee.FirstOrDefault();
        }

    }

}
