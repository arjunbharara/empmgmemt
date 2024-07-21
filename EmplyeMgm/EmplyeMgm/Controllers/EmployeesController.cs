using Microsoft.AspNetCore.Mvc;
using EmplyeMgm.Models;
using EmplyeMgm.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace EmplyeMgm.Controllers
{

    [Authorize(Roles ="Employee")]
    public class EmployeesController : HomeController
    {
        private readonly IEmployeeService _employeeService;
       
        
        public EmployeesController(IEmployeeService employeeService, ILogger<HomeController> logger):base(logger)
        {
            _employeeService = employeeService;

        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = HttpContext.User;
                var employees = await _employeeService.GetEmployeesAsync(user);
                return View(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting employees");
                return RedirectToAction("Error", new { statusCode = "500" });
            }
        }

        [Authorize(Roles = "Admin,SuperAdmin,Employee")]
        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }


        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Emial,DOB,City,IsAdmin")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _employeeService.UpdateEmployeeAsync(employee);
                   
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_employeeService.EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
