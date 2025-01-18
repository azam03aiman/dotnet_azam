using Microsoft.AspNetCore.Mvc;
using Friends_Web_App.Models;
using Friends_Web_App.Data;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection;

namespace Friends_Web_App.Controllers
{


    public class FriendController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public FriendController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var friend = await dbContext.Friends.ToListAsync();
            return View(friend);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(FriendViewModel viewModel)
        {
            var friend = new Friend
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                age = viewModel.age,
                Year = viewModel.Year,
                Phone = viewModel.Phone,
                Gender = viewModel.Gender,
                hometown = viewModel.hometown
            };
        await dbContext.Friends.AddAsync(friend);
        await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Friend");
        
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var friend = await dbContext.Friends.FindAsync(id);
            if (friend == null)
            {
                return NotFound();
            }
            return View(new FriendViewModel
            {
                Name = friend.Name,
                Description = friend.Description,
                age = friend.age,
                Year = friend.Year,
                Phone = friend.Phone,
                Gender = friend.Gender,
                hometown = friend.hometown
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(FriendViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            var friend = await dbContext.Friends.FindAsync(viewModel.Id);
            if (friend == null)
            {
                return NotFound();
            }
            friend.Name = viewModel.Name;
            friend.Description = viewModel.Description;
            friend.age = viewModel.age;
            friend.Year = viewModel.Year;
            friend.Phone = viewModel.Phone;
            friend.Gender = viewModel.Gender;
            friend.hometown = viewModel.hometown;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List", "Friend");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(FriendViewModel viewModel)
        {
            var friend= await dbContext.Friends.FindAsync(viewModel.Id);
            if (friend != null)
            {
                dbContext.Friends.Remove(friend);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List", "Friend");
        }
    }
}
