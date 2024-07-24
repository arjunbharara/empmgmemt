using EmplyeMgm.Models;
using System.Security.Claims;

namespace EmplyeMgm.StoredProcedure
{
    public interface IEmployeeSPService
    {
       
        Task UpdateEmployee(Employee employee);
        Task<Employee> GetEmployeeById(int Id);
        
    }
}
