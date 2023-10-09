using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mono.Data;
using Mono.Models;
using Mono.ViewModels;

namespace Mono.Controllers
{
    public class VehicleModelsController : Controller
    {
        private readonly VehicleContext _context;

        public VehicleModelsController(VehicleContext context)
        {
            _context = context;
        }

        // GET: VehicleModels
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

            var models = from m in _context.vehicleModels
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                models = models.Where(m => m.Name.Contains(searchString)
                                       || m.Abrv.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    models = models.OrderByDescending(m => m.Name);
                    break;
                case "makeId":
                    models = models.OrderBy(m => m.MakeId);
                    break;
                case "makeId_desc":
                    models = models.OrderByDescending(m => m.MakeId);
                    break;
                case "abrv":
                    models = models.OrderBy(m => m.Abrv);
                    break;
                case "abrv_desc":
                    models = models.OrderByDescending(m => m.Abrv);
                    break;
                default:
                    models = models.OrderBy(m => m.Name);
                    break;
            }

            int pageSize = 3;
            var paginatedListModel = await PaginatedList<VehicleModel>.CreateAsync(models, pageNumber ?? 1, pageSize);
            return View(paginatedListModel);
        }

        // GET: VehicleModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.vehicleModels == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.vehicleModels
                .Include(v => v.Make)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            var modelViewModel = new ModelViewModel(vehicleModel.Id, vehicleModel.MakeId, vehicleModel.Name, vehicleModel.Abrv);
            return View(modelViewModel);
        }

        // GET: VehicleModels/Create
        public IActionResult Create()
        {
            ViewData["MakeId"] = new SelectList(_context.vehicleMakes, "Id", "Id");
            return View();
        }

        // POST: VehicleModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MakeId,Name,Abrv")] CreateModelRequest request)
        {
            if (ModelState.IsValid)
            {
                var newModel = new VehicleModel(request.MakeId, request.Name, request.Abrv);
                _context.Add(newModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MakeId"] = new SelectList(_context.vehicleMakes, "Id", "Id", request.MakeId);
            var modelViewModel = new ModelViewModel(request.MakeId, request.Name, request.Abrv);
            return View(modelViewModel);
        }

        // GET: VehicleModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.vehicleModels == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.vehicleModels.FindAsync(id);
            if (vehicleModel == null)
            {
                return NotFound();
            }
            ViewData["MakeId"] = new SelectList(_context.vehicleMakes, "Id", "Id", vehicleModel.MakeId);
            var modelViewModel = new ModelViewModel(vehicleModel.Id, vehicleModel.MakeId, vehicleModel.Name, vehicleModel.Abrv);
            return View(modelViewModel);
        }

        // POST: VehicleModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MakeId,Name,Abrv")] EditModelRequest request)
        {
            if (id != request.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var editModel = new VehicleModel(request.Id, request.MakeId, request.Name, request.Abrv);
                    _context.Update(editModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VehicleModelExists(request.Id))
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
            ViewData["MakeId"] = new SelectList(_context.vehicleMakes, "Id", "Id", request.MakeId);
            var modelViewModel = new ModelViewModel(request.Id, request.MakeId, request.Name, request.Abrv);
            return View(modelViewModel);
        }

        // GET: VehicleModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.vehicleModels == null)
            {
                return NotFound();
            }

            var vehicleModel = await _context.vehicleModels
                .Include(v => v.Make)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vehicleModel == null)
            {
                return NotFound();
            }

            var modelViewModel = new ModelViewModel(vehicleModel.Id, vehicleModel.MakeId, vehicleModel.Name, vehicleModel.Abrv);
            return View(modelViewModel);
        }

        // POST: VehicleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.vehicleModels == null)
            {
                return Problem("Entity set 'VehicleContext.vehicleModels'  is null.");
            }
            var vehicleModel = await _context.vehicleModels.FindAsync(id);
            if (vehicleModel != null)
            {
                _context.vehicleModels.Remove(vehicleModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VehicleModelExists(int id)
        {
            return (_context.vehicleModels?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
