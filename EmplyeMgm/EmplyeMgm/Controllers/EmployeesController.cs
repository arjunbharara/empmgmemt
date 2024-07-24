using Microsoft.AspNetCore.Mvc;
using EmplyeMgm.Models;
using EmplyeMgm.Services;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using EmplyeMgm.ViewModel;
using EmplyeMgm.StoredProcedure;

namespace EmplyeMgm.Controllers
{

    [Authorize(Roles ="Employee")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<AdminController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmployeeSPService _employeeSPService;
        public EmployeesController(IEmployeeService employeeService, ILogger<AdminController> logger, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,IEmployeeSPService employeeSPService)
        {
            _employeeService = employeeService;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _employeeSPService = employeeSPService;
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
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while getting employee details.");
                return RedirectToAction("Error", "Home");
            }
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
                var employee = await _employeeService.GetEmployeeByIdAsync(id);
                //var employee = await _employeeSPService.GetEmployeeById(id);
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
                    await _employeeService.UpdateEmployeeAsync(employee);
                    //await _employeeSPService.UpdateEmployee(employee);
                    TempData["EmployeeEdit"] = true;
                    return View("Edit");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!await _employeeService.EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError(ex, "A concurrency error occurred while updating employee.");
                        return RedirectToAction("Error", "Home");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating employee.");
                    return RedirectToAction("Error", "Home");
                }
            }
            return RedirectToAction("Index");
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
                var result=await _employeeService.ChangePass(model,user);
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
