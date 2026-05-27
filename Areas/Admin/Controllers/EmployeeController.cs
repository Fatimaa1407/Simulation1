using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation1.Areas.Admin.ViewModels.Employee;
using Simulation1.DAL;
using Simulation1.Models;
using Simulation1.Utilities.Images;

namespace Simulation1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env= env;
        }


        

        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _db.Employees.Include(e => e.Position).ToListAsync();

            return View(employees);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateEmployeeVM employeeVM)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            if (!ModelState.IsValid) return View(employeeVM);

            if (employeeVM.ImageFile is null)
            {

                ModelState.AddModelError("ImageFile", "Image is required");
                return View(employeeVM);
            }

            if (!employeeVM.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "File must be an image");
                return View(employeeVM);
            }

            if (employeeVM.ImageFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "File can not exceed 2MB");
                return View(employeeVM);
            }


            Employee employee = new Employee()
            {
                FullName = employeeVM.FullName,
                PositionId = employeeVM.PositionId,
                ImageUrl = employeeVM.ImageFile.SaveImage(_env, "uploads/employees")
            };

            await _db.Employees.AddAsync(employee);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            Employee? employee = _db.Employees.Include(e => e.Position).FirstOrDefault(e => e.Id == id);

            UpdateEmployeeVM employeeVM = new UpdateEmployeeVM()
            {
                FullName = employee.FullName,
                PositionId = employee.PositionId,
                ImageUrl = employee.ImageUrl
            };
            return View(employeeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateEmployeeVM employeeVM)
        {
            ViewBag.Positions = await _db.Positions.ToListAsync();
            if (!ModelState.IsValid) return View(employeeVM);
            if (employeeVM.ImageFile is null)
            {

                ModelState.AddModelError("ImageFile", "Image is required");
                return View(employeeVM);
            }

            if (!employeeVM.ImageFile.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("ImageFile", "File must be an image");
                return View(employeeVM);
            }

            if (employeeVM.ImageFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "File can not exceed 2MB");
                return View(employeeVM);
            }

            Employee? oldEmployee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == employeeVM.Id);

            oldEmployee.FullName = employeeVM.FullName;
            oldEmployee.PositionId = employeeVM.PositionId;

            if (employeeVM.ImageFile is not null)
            {
                oldEmployee.ImageUrl = employeeVM.ImageFile.SaveImage(_env, "uploads/employees");
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]

        public async Task<IActionResult> Delete(int? id)
        {
            Employee? employee =await  _db.Employees.FindAsync(id);
            employee.IsDeleted = true;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]

        public async Task<IActionResult> Restore(int? id)
        {
            Employee? employee = await _db.Employees.FindAsync(id);
            employee.IsDeleted = false;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
