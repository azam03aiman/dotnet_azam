using FriendsApp.Data;
using FriendsApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

public class CommentController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CommentController> _logger;

    public CommentController(ApplicationDbContext context, ILogger<CommentController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Comment/Create/5
    [Authorize]
    public IActionResult Create(int postId)
    {
        ViewBag.PostId = postId;  // Passing the PostId to the view (can be used in the form)
        return View();
    }

    // POST: Comment/Create
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(int postId, string content)
    {
        // Retrieve the UserId from the claims
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(userIdString, out int userId))
        {
            // Find the post that the comment is for
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                _logger.LogWarning($"Post with ID {postId} not found.");
                return RedirectToAction("Index", "Post");  // Redirect back to the list of posts
            }

            // Create the comment object and set properties directly
            var comment = new Comment
            {
                Content = string.IsNullOrEmpty(content) ? "No content provided" : content, // Default to "No content provided" if empty
                CreatedAt = DateTime.Now,  // Use current timestamp
                PostId = postId,           // Linking the comment to the correct post
                UserId = userId            // Linking the comment to the current user
            };

            // Add the comment to the database
            _context.Comments.Add(comment);

            try
            {
                // Save the changes to the database
                await _context.SaveChangesAsync();
                _logger.LogInformation("Comment saved successfully.");
                return RedirectToAction("Details", "Post", new { id = postId });  // Redirect to post details page
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Error occurred while saving comment: {ex.Message}");
                return View();  // Return to the view on error
            }
        }

        return BadRequest("Invalid user ID.");
    }

    // Edit GET
    [Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(userIdString);

        // Check if the current user is the owner of the comment
        if (comment.UserId != userId)
        {
            return Forbid(); // User is not authorized to edit this comment
        }

        return View(comment);
    }

    // Edit POST
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Comment model)
    {
        if (id != model.CommentId)
        {
            return NotFound(); // This ensures the comment ID is valid
        }

        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound(); // If the comment does not exist
        }

        // Check if the current user is the owner of the comment
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(userIdString);

        if (comment.UserId != userId)
        {
            return Forbid(); // User is not authorized to edit this comment
        }

        // Update the content of the comment
        comment.Content = model.Content;
        comment.CreatedAt = DateTime.Now; // Set to the current date if required

        _context.Comments.Update(comment);
        await _context.SaveChangesAsync();

        // After successful edit, redirect to the post details
        return RedirectToAction("Details", "Post", new { id = comment.PostId });
    }

    // Delete GET
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(userIdString);

        // Check if the current user is the owner of the comment
        if (comment.UserId != userId)
        {
            return Forbid(); // User is not authorized to delete this comment
        }

        return View(comment);
    }

    // Delete POST
    [HttpPost, ActionName("Delete")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var comment = await _context.Comments.FindAsync(id);
        if (comment == null)
        {
            return NotFound();
        }

        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = int.Parse(userIdString);

        // Check if the current user is the owner of the comment
        if (comment.UserId != userId)
        {
            return Forbid(); // User is not authorized to delete this comment
        }

        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("Details", "Post", new { id = comment.PostId });
    }

}
