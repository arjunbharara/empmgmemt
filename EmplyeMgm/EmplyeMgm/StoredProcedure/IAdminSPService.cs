using EmplyeMgm.Models;
using System.Security.Claims;

namespace EmplyeMgm.StoredProcedure
{
    public interface IAdminSPService
    {
        Task<IEnumerable<Employee>> GetAlllEmployees( );
        Task CreateEmployee(Employee employee, string pass);
        Task UpdateEmployee(Employee employee);
        Task DeleteEmployee(int Id);
        Task<Employee> GetEmployeeById(int Id);
    }
}
