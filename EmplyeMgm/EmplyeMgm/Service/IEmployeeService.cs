
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
        Task<ApplicationUser>GetEmployeesAsync(ClaimsPrincipal user);
        Task<ApplicationUser> GetEmployeeByIdAsync(string id);
        Task UpdateEmployeeAsync(ApplicationUser employee);
        Task<bool> EmployeeExists(string id);
        Task<IdentityResult> ChangePass(ChangePassViewModel model,ApplicationUser user);
    }
}
