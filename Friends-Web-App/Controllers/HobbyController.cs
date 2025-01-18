using Friends_Web_App.Data;
using Friends_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friends_Web_App.Controllers
{
    public class HobbyController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public HobbyController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var hobby = await dbContext.Hobby.ToListAsync();
            return View("HobbyList",hobby);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View("AddHobby");
        }

        [HttpPost]
        public async Task<IActionResult> Add(HobbyViewModel viewModel)
        {
            var hobby = new Hobby
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                rating = viewModel.rating
            };
            await dbContext.Hobby.AddAsync(hobby);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Hobby");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var hobby = await dbContext.Hobby.FindAsync(id);
            if (hobby == null)
            {
                return NotFound();
            }
            return View("EditHobby",new HobbyViewModel
            {
                HobbyId = hobby.HobbyId,
                Name = hobby.Name,
                Description = hobby.Description,
                rating = hobby.rating
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(HobbyViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var hobby = await dbContext.Hobby.FindAsync(viewModel.HobbyId);
            if (hobby == null)
            {
                return NotFound();
            }
            hobby.HobbyId = viewModel.HobbyId;
            hobby.Name = viewModel.Name;
            hobby.Description = viewModel.Description;
            hobby.rating = viewModel.rating;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Hobby");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var hobby = await dbContext.Hobby.FindAsync(id);
            if (hobby != null)
            {
                dbContext.Hobby.Remove(hobby);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Hobby");
        }
    }
}
