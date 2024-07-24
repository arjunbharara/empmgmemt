using EmplyeMgm.Models;
using EmplyeMgm.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace EmplyeMgm.Service
{
    public interface IAdminService
    { 
            Task<IEnumerable<ApplicationUser>> GetEmployeesAsync();
            Task<ApplicationUser> GetEmployeeByIdAsync(string id);
            Task CreateEmployeeAsync(ApplicationUser employee, string pass);
            Task UpdateEmployeeAsync(ApplicationUser employee);
            Task DeleteEmployeeAsync(string id);
            Task<bool> EmployeeExists(string id);
           Task<IdentityResult> ChangePass(ChangePassViewModel model, ApplicationUser user);
    }

}



