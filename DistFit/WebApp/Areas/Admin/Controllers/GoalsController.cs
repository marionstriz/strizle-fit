using App.Contracts.BLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using App.Domain;
using App.Public.DTO.v1.Identity;
using App.Public.v1.Mappers;
using Base.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class GoalsController : Controller
    {
        private readonly IAppBll _bll;
        private readonly GoalMapper _mapper;

        public GoalsController(IAppBll bll, IMapper mapper)
        {
            _bll = bll;
            _mapper = new GoalMapper(mapper);
        }

        // GET: Admin/Goals
        public async Task<IActionResult> Index()
        {
            var goals = (await _bll.Goals
                .GetAllAsync(User.GetUserId(), true))
                .Select(x => _mapper.Map(x));
            
            return View(goals);
        }

        // GET: Admin/Goals/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goal = await _bll.Goals.FirstOrDefaultAsync(id.Value);
            
            if (goal == null)
            {
                return NotFound();
            }

            return View(_mapper.Map(goal));
        }

        // GET: Admin/Goals/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["ExerciseTypeId"] = new SelectList(await _bll.ExerciseTypes.GetAllAsync(), "Id", "Name");
            ViewData["MeasurementTypeId"] = new SelectList(await _bll.MeasurementTypes.GetAllAsync(), "Id", "Name");
            ViewData["ValueUnitId"] = new SelectList(await _bll.Units.GetAllAsync(), "Id", "Name");
            return View();
        }

        // POST: Admin/Goals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(App.Public.DTO.v1.Goal goal)
        {
            if (ModelState.IsValid)
            {
                goal.Id = Guid.NewGuid();
                goal.AppUserId = User.GetUserId();
                
                _bll.Goals.Add(_mapper.Map(goal)!);
                
                await _bll.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseTypeId"] = new SelectList(await _bll.ExerciseTypes.GetAllAsync(), "Id", "Name");
            ViewData["MeasurementTypeId"] = new SelectList(await _bll.MeasurementTypes.GetAllAsync(), "Id", "Name");
            ViewData["ValueUnitId"] = new SelectList(await _bll.Units.GetAllAsync(), "Id", "Name");
            return View(goal);
        }

        // GET: Admin/Goals/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goal = await _bll.Goals.FirstOrDefaultAsync(id.Value);
            if (goal == null)
            {
                return NotFound();
            }
            ViewData["ExerciseTypeId"] = new SelectList(await _bll.ExerciseTypes.GetAllAsync(), "Id", "Name");
            ViewData["MeasurementTypeId"] = new SelectList(await _bll.MeasurementTypes.GetAllAsync(), "Id", "Name");
            ViewData["ValueUnitId"] = new SelectList(await _bll.Units.GetAllAsync(), "Id", "Name");
            
            return View(_mapper.Map(goal));
        }

        // POST: Admin/Goals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, App.Public.DTO.v1.Goal goal)
        {
            if (id != goal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    goal.AppUserId = User.GetUserId();
                    _bll.Goals.Update(_mapper.Map(goal)!);
                    await _bll.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await GoalExistsAsync(goal.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseTypeId"] = new SelectList(await _bll.ExerciseTypes.GetAllAsync(), "Id", "Name");
            ViewData["MeasurementTypeId"] = new SelectList(await _bll.MeasurementTypes.GetAllAsync(), "Id", "Name");
            ViewData["ValueUnitId"] = new SelectList(await _bll.Units.GetAllAsync(), "Id", "Name");
            
            return View(goal);
        }

        // GET: Admin/Goals/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goal = await _bll.Goals.FirstOrDefaultAsync(id.Value);
            if (goal == null)
            {
                return NotFound();
            }

            return View(_mapper.Map(goal));
        }

        // POST: Admin/Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var goal = await _bll.Goals.FirstOrDefaultAsync(id);
            if (goal != null)
            {
                _bll.Goals.Remove(goal);
            }
            
            await _bll.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> GoalExistsAsync(Guid id)
        {
          return await _bll!.Goals.FirstOrDefaultAsync(id) != null;
        }
    }
}
