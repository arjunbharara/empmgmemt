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
        private readonly UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;

        public AdminController(IAdminService adminService,ILogger<AdminController>logger,
            IAdminSPService adminSPService,UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser>signInManager)
        {
            _adminService=adminService;
            _logger=logger;
            _adminSPService=adminSPService;
            _userManager=userManager;
            _signInManager=signInManager;
        }

        // GET: Employees
        [Authorize(Roles ="Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                 var employees = await _adminService.GetEmployeesAsync();
                //var employees = await _adminSPService.GetAlllEmployees();
                return View(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting employee");
                return RedirectToAction("Error", "Home");
            }
        }

        
        // GET: Employees/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {

                var employee = await _adminService.GetEmployeeByIdAsync(id);
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
                    var emp = new ApplicationUser
                    {
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        Email = employee.Emial,
                        DOB = employee.DOB,
                        City = employee.City,
                        IsAdmin = employee.IsAdmin
                    };
                    await _adminService.CreateEmployeeAsync(emp, employee.Password);
                   // await _adminSPService.CreateEmployee(emp, employee.Password);
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
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {

                var employee = await _adminService.GetEmployeeByIdAsync(id);
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

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Email,DOB,City,IsAdmin")] ApplicationUser employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _adminService.UpdateEmployeeAsync(employee);
                    //await _adminSPService.UpdateEmployee(employee);
                    TempData["EmployeeEdit"] = true;
                    return View("Edit");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!await _adminService.EmployeeExists(employee.Id))
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
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var employee = await _adminService.GetEmployeeByIdAsync(id);
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                await _adminService.DeleteEmployeeAsync(id);
                //await _adminSPService.DeleteEmployee(id);
                TempData["EmployeeDeleted"] = true;
                return View("Delete");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting employee.");
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePassViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    //If User does not exists, redirect to the Login Page
                    return RedirectToAction("Login", "Account");
                }
                var result = await _adminService.ChangePass(model, user);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                // changing the password refresh sign-in cookie
                await _signInManager.RefreshSignInAsync(user);

                return RedirectToAction("ChangePasswordConfirmation", "Employees");
            }

            return View(model);
        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePasswordConfirmation()
        {
            return View();
        }

    }
}
