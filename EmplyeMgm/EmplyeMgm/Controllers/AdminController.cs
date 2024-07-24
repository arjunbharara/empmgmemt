using Microsoft.AspNetCore.Mvc;
using EmplyeMgm.Models;
using EmplyeMgm.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using EmplyeMgm.Service;
using EmplyeMgm.ViewModel;
using EmplyeMgm.StoredProcedure;


namespace EmplyeMgm.Controllers
{
    [Authorize(Roles ="Admin,SuperAdmin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminController> _logger;
        private readonly IAdminSPService _adminSPService;

        public AdminController(IAdminService adminService,ILogger<AdminController>logger,IAdminSPService adminSPService)
        {
            _adminService=adminService;
            _logger=logger;
            _adminSPService=adminSPService;
        }

        // GET: Employees
        [Authorize(Roles ="Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                 //var employees = await _adminService.GetEmployeesAsync();
                var employees = await _adminSPService.GetAlllEmployees();
                return View(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting employee");
                return RedirectToAction("Error", "Home");
            }
        }

        
        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {

                var employee = await _adminService.GetEmployeeByIdAsync(id.Value);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"An error occured while getting employee details.");
                return RedirectToAction("Error", "Home");
            }
        }

       
        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employee)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    var emp = new Employee
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Emial = employee.Emial,
                        DOB = employee.DOB,
                        City = employee.City,
                        IsAdmin = employee.IsAdmin
                    };
                   // await _adminService.CreateEmployeeAsync(emp, employee.Password);
                    await _adminSPService.CreateEmployee(emp, employee.Password);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating an employee.");
                    return RedirectToAction("Error", "Home");
                }
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
            try
            {

                var employee = await _adminService.GetEmployeeByIdAsync(id.Value);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting employee for edit.");
                return RedirectToAction("Error", "Home");
            }
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
                   // await _adminService.UpdateEmployeeAsync(employee);
                    await _adminSPService.UpdateEmployee(employee);
                    TempData["EmployeeEdit"] = true;
                    return View("Edit");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!_adminService.EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "A concurrency error occurred while updating employee.");
                        return RedirectToAction("Error", "Home");
                    }
                }
                catch(Exception ex) {
                    _logger.LogError(ex, "An error occurred while updating employee.");
                    return RedirectToAction("Error", "Home");
                }
            }else
            {
                return View(employee);
            }
           
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var employee = await _adminService.GetEmployeeByIdAsync(id.Value);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting employee for delete.");
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                // await _adminService.DeleteEmployeeAsync(id);
                await _adminSPService.DeleteEmployee(id);
                TempData["EmployeeDeleted"] = true;
                return View("Delete");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting employee.");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
