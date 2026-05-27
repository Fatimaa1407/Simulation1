using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Simulation1.Areas.Admin.ViewModels.Position;
using Simulation1.DAL;
using Simulation1.Models;

namespace Simulation1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PositionController : Controller
    {
        private readonly AppDbContext _db;

        public PositionController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Position> positions = await _db.Positions.Include(p => p.Employees).ToListAsync();
            return View(positions);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreatePositionVM positionVM)
        {
            if (!ModelState.IsValid) return View(positionVM);
            Position position = new Position()
            {
                Name = positionVM.Name,
            };

            await _db.Positions.AddAsync(position);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Update(int? id)
        {

            if (id is null) return NotFound();
            Position position = await _db.Positions.FindAsync(id);
            if (position is null) return NotFound();

            UpdatePositionVM positionVM = new UpdatePositionVM()
            {
                Name = position.Name,
                Id = position.Id
            };
            return View(positionVM);
        }

        [HttpPost]

        public async Task<IActionResult> Update(UpdatePositionVM positionVM)
        {
            if (!ModelState.IsValid) return View(positionVM);
            Position oldPosition = await _db.Positions.FindAsync(positionVM.Id);
            oldPosition.Name = positionVM.Name;

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return NotFound();
            Position position = await _db.Positions.FindAsync(id);
            position.IsDeleted = true;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id is null) return NotFound();
            Position position = await _db.Positions.FindAsync(id);
            position.IsDeleted = false;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

    }
