
using EmplyeMgm.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EmplyeMgm.Services
{
    public interface IEmployeeService
    {
        Task<Employee>GetEmployeesAsync(ClaimsPrincipal user);
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task CreateEmployeeAsync(Employee employee,string pass);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        bool EmployeeExists(int id);
    }
}
