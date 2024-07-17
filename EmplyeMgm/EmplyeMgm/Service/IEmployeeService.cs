﻿
using EmplyeMgm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmplyeMgm.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<Employee>> GetEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task CreateEmployeeAsync(Employee employee);
        Task UpdateEmployeeAsync(Employee employee);
        Task DeleteEmployeeAsync(int id);
        bool EmployeeExists(int id);
    }
}
