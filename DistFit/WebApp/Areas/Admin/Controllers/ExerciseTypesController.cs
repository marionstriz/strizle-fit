using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using App.DAL.EF;
using App.Domain;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExerciseTypesController : Controller
    {
        private readonly AppDbContext _context;

        public ExerciseTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ExerciseTypes
        public async Task<IActionResult> Index()
        {
              return _context.ExerciseTypes != null ? 
                          View(await _context.ExerciseTypes.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.ExerciseTypes'  is null.");
        }

        // GET: Admin/ExerciseTypes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.ExerciseTypes == null)
            {
                return NotFound();
            }

            var exerciseType = await _context.ExerciseTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exerciseType == null)
            {
                return NotFound();
            }

            return View(exerciseType);
        }

        // GET: Admin/ExerciseTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ExerciseTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id")] ExerciseType exerciseType)
        {
            if (ModelState.IsValid)
            {
                exerciseType.Id = Guid.NewGuid();
                _context.Add(exerciseType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(exerciseType);
        }

        // GET: Admin/ExerciseTypes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.ExerciseTypes == null)
            {
                return NotFound();
            }

            var exerciseType = await _context.ExerciseTypes.FindAsync(id);
            if (exerciseType == null)
            {
                return NotFound();
            }
            return View(exerciseType);
        }

        // POST: Admin/ExerciseTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Name,Id")] ExerciseType exerciseType)
        {
            if (id != exerciseType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exerciseType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseTypeExists(exerciseType.Id))
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
            return View(exerciseType);
        }

        // GET: Admin/ExerciseTypes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.ExerciseTypes == null)
            {
                return NotFound();
            }

            var exerciseType = await _context.ExerciseTypes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exerciseType == null)
            {
                return NotFound();
            }

            return View(exerciseType);
        }

        // POST: Admin/ExerciseType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.ExerciseTypes == null)
            {
                return Problem("Entity set 'AppDbContext.ExerciseTypes'  is null.");
            }
            var exerciseType = await _context.ExerciseTypes.FindAsync(id);
            if (exerciseType != null)
            {
                _context.ExerciseTypes.Remove(exerciseType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseTypeExists(Guid id)
        {
          return (_context.ExerciseTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
