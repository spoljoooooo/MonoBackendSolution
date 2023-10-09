using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mono.Data;
using Mono.Models;
using Mono.ViewModels;

namespace Mono.Controllers
{
    public class VehicleMakesController : Controller
    {
        private readonly VehicleContext _context;

        public VehicleMakesController(VehicleContext context)
        {
            _context = context;
        }

        // GET: VehicleMakes
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "abrv" ? "abrv_desc" : "abrv";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var makes = from m in _context.vehicleMakes
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                makes = makes.Where(m => m.Name.Contains(searchString)
                                       || m.Abrv.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    makes = makes.OrderByDescending(m => m.Name);
                    break;
                case "abrv":
                    makes = makes.OrderBy(m => m.Abrv);
                    break;
                case "abrv_desc":
                    makes = makes.OrderByDescending(m => m.Abrv);
                    break;
                default:
                    makes = makes.OrderBy(m => m.Name);
                    break;
            }

            int pageSize = 3;
            var paginatedListModel = await PaginatedList<VehicleMake>.CreateAsync(makes, pageNumber ?? 1, pageSize);
            return View(paginatedListModel);
        }

        // GET: VehicleMakes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.vehicleMakes == null)
            {
                return NotFound();
            }

            var vehicleMake = await _context.vehicleMakes.FirstOrDefaultAsync(m => m.Id == id);

            if (vehicleMake == null)
            {
                return NotFound();
            }

            var makeViewModel = new MakeViewModel(vehicleMake.Id, vehicleMake.Name, vehicleMake.Abrv);

            return View(makeViewModel);
        }

        // GET: VehicleMakes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VehicleMakes/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Abrv")] CreateMakeRequest request)
        {
            if (ModelState.IsValid)
            {
                var newMake = new VehicleMake(request.Name, request.Abrv);
                _context.Add(newMake);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var makeViewModel = new MakeViewModel(request.Name, request.Abrv);
            return View(makeViewModel);
        }

        // GET: VehicleMakes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.vehicleMakes == null)
            {
                return NotFound();
            }

            var vehicleMake = await _context.vehicleMakes.FindAsync(id);
            if (vehicleMake == null)
            {
                return NotFound();
            }

            var makeViewModel = new MakeViewModel(vehicleMake.Id, vehicleMake.Name, vehicleMake.Abrv);
            return View(makeViewModel);
        }

        // POST: VehicleMakes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Abrv")] EditMakeRequest request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var editMake = new VehicleMake(request.Id, request.Name, request.Abrv);
                    _context.Update(editMake);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleMakeExists(request.Id))
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

            var makeViewModel = new MakeViewModel(request.Id, request.Name, request.Abrv);
            return View(makeViewModel);
        }

        // GET: VehicleMakes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.vehicleMakes == null)
            {
                return NotFound();
            }

            var vehicleMake = await _context.vehicleMakes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleMake == null)
            {
                return NotFound();
            }

            var makeViewModel = new MakeViewModel(vehicleMake.Id, vehicleMake.Name, vehicleMake.Abrv);
            return View(makeViewModel);
        }

        // POST: VehicleMakes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.vehicleMakes == null)
            {
                return Problem("Entity set 'VehicleContext.vehicleMakes'  is null.");
            }
            var vehicleMake = await _context.vehicleMakes.FindAsync(id);
            if (vehicleMake != null)
            {
                _context.vehicleMakes.Remove(vehicleMake);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleMakeExists(int id)
        {
            return (_context.vehicleMakes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
