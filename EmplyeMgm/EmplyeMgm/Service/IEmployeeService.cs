
using EmplyeMgm.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using EmplyeMgm.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace EmplyeMgm.Services
{
    public interface IEmployeeService
    {
        Task<Employee>GetEmployeesAsync(ClaimsPrincipal user);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task CreateEmployeeAsync(Employee employee,string pass);
        Task UpdateEmployeeAsync(Employee employee);
        bool EmployeeExists(int id);
        Task<IdentityResult> ChangePass(ChangePassViewModel model,ApplicationUser user);
    }
}
