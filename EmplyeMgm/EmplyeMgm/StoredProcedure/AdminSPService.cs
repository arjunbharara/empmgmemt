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

        public IEnumerable<Employee> GetAlllEmployees()
        {
            return _context.Employees.FromSqlRaw("GetAllEmployees").ToList();
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

        public Task UpdateEmployee(Employee employee)
        {
            throw new NotImplementedException();
        }
    }
}
