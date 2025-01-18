using Friends_Web_App.Data;
using Friends_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friends_Web_App.Controllers
{
    public class BucketListController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public BucketListController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // List action to display all Bucket List items
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var items = await dbContext.BucketList.ToListAsync();
            return View("List", items);
        }

        // Add action to show the form for adding a new item
        [HttpGet]
        public IActionResult Add()
        {
            return View("Add");
        }

        // Post action to handle adding a new Bucket List item
        [HttpPost]
        public async Task<IActionResult> Add(BucketListViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Add", viewModel);
            }

            var bucketListItem = new BucketList
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                rating = viewModel.rating,
                status = viewModel.status
            };

            await dbContext.BucketList.AddAsync(bucketListItem);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        // Edit action to show the form for editing an existing item
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await dbContext.BucketList.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            var viewModel = new BucketListViewModel
            {
                BucketListId = item.BucketListId,
                Name = item.Name,
                Description = item.Description,
                rating = item.rating,
                status = item.status
            };

            return View("Edit", viewModel);
        }

        // Post action to handle editing an existing Bucket List item
        [HttpPost]
        public async Task<IActionResult> Edit(BucketListViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", viewModel);
            }

            var item = await dbContext.BucketList.FindAsync(viewModel.BucketListId);
            if (item == null)
            {
                return NotFound();
            }

            item.Name = viewModel.Name;
            item.Description = viewModel.Description;
            item.rating = viewModel.rating;
            item.status = viewModel.status;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        // Post action to handle deleting an item
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await dbContext.BucketList.FindAsync(id);
            if (item != null)
            {
                dbContext.BucketList.Remove(item);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List");
        }
    }
}
