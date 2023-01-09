using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GoalsController : Controller
    {
        private readonly AppDbContext _context;

        public GoalsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Goals
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Goals.Include(g => g.AppUser).Include(g => g.ExerciseType).Include(g => g.MeasurementType).Include(g => g.ValueUnit);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Admin/Goals/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Goals == null)
            {
                return NotFound();
            }

            var goal = await _context.Goals
                .Include(g => g.AppUser)
                .Include(g => g.ExerciseType)
                .Include(g => g.MeasurementType)
                .Include(g => g.ValueUnit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goal == null)
            {
                return NotFound();
            }

            return View(goal);
        }

        // GET: Admin/Goals/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Id");
            ViewData["MeasurementTypeId"] = new SelectList(_context.MeasurementTypes, "Id", "Id");
            ViewData["ValueUnitId"] = new SelectList(_context.Units, "Id", "Id");
            return View();
        }

        // POST: Admin/Goals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Value,ValueUnitId,ReachedAt,AppUserId,MeasurementTypeId,ExerciseTypeId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Comment,Id")] Goal goal)
        {
            if (ModelState.IsValid)
            {
                goal.Id = Guid.NewGuid();
                _context.Add(goal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", goal.AppUserId);
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Id", goal.ExerciseTypeId);
            ViewData["MeasurementTypeId"] = new SelectList(_context.MeasurementTypes, "Id", "Id", goal.MeasurementTypeId);
            ViewData["ValueUnitId"] = new SelectList(_context.Units, "Id", "Id", goal.ValueUnitId);
            return View(goal);
        }

        // GET: Admin/Goals/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Goals == null)
            {
                return NotFound();
            }

            var goal = await _context.Goals.FindAsync(id);
            if (goal == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", goal.AppUserId);
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Id", goal.ExerciseTypeId);
            ViewData["MeasurementTypeId"] = new SelectList(_context.MeasurementTypes, "Id", "Id", goal.MeasurementTypeId);
            ViewData["ValueUnitId"] = new SelectList(_context.Units, "Id", "Id", goal.ValueUnitId);
            return View(goal);
        }

        // POST: Admin/Goals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Value,ValueUnitId,ReachedAt,AppUserId,MeasurementTypeId,ExerciseTypeId,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt,Comment,Id")] Goal goal)
        {
            if (id != goal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoalExists(goal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", goal.AppUserId);
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Id", goal.ExerciseTypeId);
            ViewData["MeasurementTypeId"] = new SelectList(_context.MeasurementTypes, "Id", "Id", goal.MeasurementTypeId);
            ViewData["ValueUnitId"] = new SelectList(_context.Units, "Id", "Id", goal.ValueUnitId);
            return View(goal);
        }

        // GET: Admin/Goals/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Goals == null)
            {
                return NotFound();
            }

            var goal = await _context.Goals
                .Include(g => g.AppUser)
                .Include(g => g.ExerciseType)
                .Include(g => g.MeasurementType)
                .Include(g => g.ValueUnit)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goal == null)
            {
                return NotFound();
            }

            return View(goal);
        }

        // POST: Admin/Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Goals == null)
            {
                return Problem("Entity set 'AppDbContext.Goals'  is null.");
            }
            var goal = await _context.Goals.FindAsync(id);
            if (goal != null)
            {
                _context.Goals.Remove(goal);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoalExists(Guid id)
        {
          return (_context.Goals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
