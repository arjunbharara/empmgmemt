using EmplyeMgm.Models;
using System.Security.Claims;

namespace EmplyeMgm.StoredProcedure
{
    public interface IAdminSPService
    {
        IEnumerable<Employee> GetAlllEmployees( );
        Task CreateEmployee(Employee employee, string pass);
        Task UpdateEmployee(Employee employee);
    }
}
