using Friends_Web_App.Data;
using Friends_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Friends_Web_App.Controllers
{
    public class GroupTaskController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public GroupTaskController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var tasks = await dbContext.GroupTask.ToListAsync();
            return View(tasks);  // The view will automatically look for "List.cshtml" 
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();  // Looks for Add.cshtml by default
        }

        [HttpPost]
        public async Task<IActionResult> Add(GroupTaskViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var groupTask = new GroupTask
                {
                    Name = viewModel.Name,
                    Description = viewModel.Description,
                    Priority = viewModel.Priority,
                    DueDate = viewModel.DueDate,
                    IsComplete = viewModel.IsComplete
                };
                await dbContext.GroupTask.AddAsync(groupTask);
                await dbContext.SaveChangesAsync();

                return RedirectToAction("List");
            }
            return View(viewModel);  // Return to Add page if validation fails
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await dbContext.GroupTask.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(new GroupTaskViewModel
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                Priority = task.Priority,
                DueDate = task.DueDate,
                IsComplete = task.IsComplete
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GroupTaskViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);  // Return to Edit page if validation fails
            }

            var task = await dbContext.GroupTask.FindAsync(viewModel.Id);
            if (task == null)
            {
                return NotFound();
            }
            task.Name = viewModel.Name;
            task.Description = viewModel.Description;
            task.Priority = viewModel.Priority;
            task.DueDate = viewModel.DueDate;
            task.IsComplete = viewModel.IsComplete;

            await dbContext.SaveChangesAsync();

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var task = await dbContext.GroupTask.FindAsync(id);
            if (task != null)
            {
                dbContext.GroupTask.Remove(task);
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("List");
        }
    }
}
