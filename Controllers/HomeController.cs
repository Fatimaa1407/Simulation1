using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation1.DAL;
using Simulation1.Models;
using System.Runtime.CompilerServices;

namespace Simulation1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Employee> employees = await _db.Employees.Include(p => p.Position).ToListAsync();

            return View(employees);
        }

        public async Task<IActionResult> Details(int? id)
        {
            Employee employee = await _db.Employees.Include(p => p.Position).FirstOrDefaultAsync(p=>p.Id == id);
            return View(employee);
   
        }
    }
}
