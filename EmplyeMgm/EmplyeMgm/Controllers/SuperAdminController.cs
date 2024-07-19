using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmplyeMgm.Models;
using EmplyeMgm.Service;
using Microsoft.AspNetCore.Authorization;
using EmplyeMgm.ViewModel;

namespace EmplyeMgm.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISuperAdminService _superAdminService;
        public SuperAdminController(ApplicationDbContext context, ISuperAdminService superAdminService)
        {
            _context = context;
            _superAdminService = superAdminService;
        }

            // GET: Employees
            public async Task<IActionResult> Index()
            {
                var employees = await _superAdminService.GetEmployeesAsync();
                return View(employees);
            }


            // GET: Employees/Details/5
            public async Task<IActionResult> Details(int? id)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _superAdminService.GetEmployeeByIdAsync(id.Value);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }

        
        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( EmployeeViewModel employee)
        {
            if (ModelState.IsValid)
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

                await _superAdminService.CreateEmployeeAsync(emp, employee.Password);
                return RedirectToAction(nameof(Index));
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

                var employee = await _superAdminService.GetEmployeeByIdAsync(id.Value);
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
                        await _superAdminService.UpdateEmployeeAsync(employee);

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_superAdminService.EmployeeExists(employee.Id))
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

                var employee = await _superAdminService.GetEmployeeByIdAsync(id.Value);
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
                await _superAdminService.DeleteEmployeeAsync(id);
                return RedirectToAction(nameof(Index));
            }
    }
}
