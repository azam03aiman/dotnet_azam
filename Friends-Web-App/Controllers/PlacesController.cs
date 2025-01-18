using Friends_Web_App.Data;
using Friends_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friends_Web_App.Controllers
{
    public class PlacesController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public PlacesController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var places = await dbContext.Places.ToListAsync();
            return View("List", places);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        [HttpPost]
        public async Task<IActionResult> Add(PlacesViewModel viewModel)
        {
            var place = new Places
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Negeri = viewModel.Negeri
            };
            await dbContext.Places.AddAsync(place);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Places");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var place = await dbContext.Places.FindAsync(id);
            if (place == null)
            {
                return NotFound();
            }
            return View("Edit", new PlacesViewModel
            {
                PlacesId = place.PlacesId,
                Name = place.Name,
                Description = place.Description,
                Negeri = place.Negeri
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlacesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var place = await dbContext.Places.FindAsync(viewModel.PlacesId);
            if (place == null)
            {
                return NotFound();
            }
            place.Name = viewModel.Name;
            place.Description = viewModel.Description;
            place.Negeri = viewModel.Negeri;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Places");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var place = await dbContext.Places.FindAsync(id);
            if (place != null)
            {
                dbContext.Places.Remove(place);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Places");
        }
    }
}
