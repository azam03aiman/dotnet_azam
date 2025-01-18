using Friends_Web_App.Data;
using Friends_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friends_Web_App.Controllers
{
    public class FoodController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public FoodController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET: Food/List
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var foods = await dbContext.Food
                                        .Select(f => new FoodViewModel
                                        {
                                            Id = f.Id,
                                            Name = f.Name,
                                            Description = f.Description,
                                            rating = f.rating
                                        })
                                        .ToListAsync();
            return View("List", foods);
        }

        // GET: Food/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        // POST: Food/Add
        [HttpPost]
        public async Task<IActionResult> Add(FoodViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var food = new Food
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    rating = viewModel.rating
                };

                dbContext.Food.Add(food);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return View("Add");
        }

        // GET: Food/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var food = await dbContext.Food.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            var viewModel = new FoodViewModel
            {
                Id = food.Id,
                Name = food.Name,
                Description = food.Description,
                rating = food.rating
            };

            return View("Edit", viewModel);
        }

        // POST: Food/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(FoodViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var food = await dbContext.Food.FindAsync(viewModel.Id);
                if (food == null)
                {
                    return NotFound();
                }

                food.Name = viewModel.Name;
                food.Description = viewModel.Description;
                food.rating = viewModel.rating;

                dbContext.Food.Update(food);
                await dbContext.SaveChangesAsync();
                return RedirectToAction("List");
            }

            return View("Edit", viewModel);
        }

        // POST: Food/Delete/{id}
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var food = await dbContext.Food.FindAsync(id);
            if (food != null)
            {
                dbContext.Food.Remove(food);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List");
        }
    }
}
