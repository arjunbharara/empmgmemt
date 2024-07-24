using EmplyeMgm.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace EmplyeMgm.StoredProcedure
{
    public class EmployeeSPService : IEmployeeSPService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmployeeSPService(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<Employee> GetEmployeeById(int Id)
        {
            try
            {
                var parameter = new SqlParameter("@Id", SqlDbType.Int) { Value = Id };
                var employee = await _context.Employees.FromSqlRaw("EXEC GetEmployees @Id", parameter).ToListAsync();
                return employee.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving an employee by Id.", ex);
            }
        }

       
        public async Task UpdateEmployee(Employee employee)
        {
            try
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
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while updating an employee.", ex);
            }
        }
    }
}
