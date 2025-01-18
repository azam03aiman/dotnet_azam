namespace FriendsApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using FriendsApp.Data;
    using FriendsApp.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Security.Claims;
    using Microsoft.EntityFrameworkCore;

    public class PostController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PostController> _logger;

        public PostController(ApplicationDbContext context, ILogger<PostController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Post/Index
        public IActionResult Index()
        {
            var posts = _context.Posts
                .Include(p => p.User) // Include the User entity
                .OrderByDescending(p => p.CreatedAt)  // Display most recent posts first
                .ToList();

            return View(posts);  // Return the list of posts to the view
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var post = await _context.Posts
                .Include(p => p.User)  // Include the User entity
                .FirstOrDefaultAsync(p => p.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            // Fetch associated comments
            var comments = await _context.Comments
                .Where(c => c.PostId == id)  // Filter by PostId
                .Include(c => c.User)         // Optionally, include user info for the comment
                .ToListAsync();

            // Pass comments to the view using ViewBag
            ViewBag.Comments = comments;

            return View(post);  // Return the post details view
        }

        // GET: Post/Create
        [Authorize]  // Ensure that only logged-in users can create posts
        public IActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post model)
        {
            // Skip validation for now, ensure that Title and Content are provided
            if (string.IsNullOrEmpty(model.Title) || string.IsNullOrEmpty(model.Content))
            {
                ModelState.AddModelError("Title", "Both Title and Content are required.");
                return View(model);  // Return the view with the error
            }

            // Retrieve the UserId from the claims
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogWarning(userIdString);

            if (int.TryParse(userIdString, out int userId))
            {
                // Lookup the user by UserId
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
                _logger.LogInformation($"User ID: {user?.UserId}");

                if (user == null)
                {
                    _logger.LogWarning("User not found for post creation.");
                    return NotFound();  // Return NotFound if user does not exist
                }

                // Create a new post and associate it with the current user
                var post = new Post
                {
                    Title = model.Title,
                    Content = model.Content,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null,  // Newly created post has no UpdatedAt yet
                    UserId = user.UserId,
                };

                // Add the post to the database and save changes
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Post created successfully by user {user.Name}.");
                return RedirectToAction("Index", "Home");  // Redirect after post creation
            }
            else
            {
                _logger.LogWarning("UserId is invalid or not found in the claims.");
                return BadRequest("Invalid user ID.");
            }
        }

        // GET: Post/Edit/5
        [Authorize]
        public IActionResult Edit(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.PostId == id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);  // Return the post data to the Edit view
        }

        // POST: Post/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post model)
        {
            if (id != model.PostId)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var post = await _context.Posts.FindAsync(id);
                if (post == null)
                {
                    return NotFound();
                }

                post.Title = model.Title;
                post.Content = model.Content;
                post.UpdatedAt = DateTime.Now; // Update the post's updated timestamp

                _context.Posts.Update(post);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Post updated successfully");

                return RedirectToAction("Index"); // Redirect to the list of posts after update
            }

            return View(model); // If validation fails, return to the edit view
        }

        // GET: Post/Delete/5
        [Authorize]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts
                .FirstOrDefault(p => p.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);  // Return the post delete confirmation view
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Post deleted successfully");

            return RedirectToAction("Index"); // Redirect to the post list after deletion
        }
    }
}
