using EmplyeMgm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace EmplyeMgm.Service
{
    public interface IAdminService
    { 
            Task<IEnumerable<Employee>> GetEmployeesAsync();
            Task<Employee> GetEmployeeByIdAsync(int id);
            Task CreateEmployeeAsync(Employee employee, string pass);
            Task UpdateEmployeeAsync(Employee employee);
            Task DeleteEmployeeAsync(int id);
            bool EmployeeExists(int id);
        
    }

}

