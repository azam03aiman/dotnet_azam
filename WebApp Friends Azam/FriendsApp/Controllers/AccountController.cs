namespace FriendsApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using BCrypt.Net; // To compare passwords
    using Microsoft.Extensions.Logging;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using FriendsApp.Data;
    using FriendsApp.Models;
    using Microsoft.AspNetCore.Authorization;
    using System.Security.Claims;
    using System.Collections.Generic; // Make sure this is included for List<Claim>

    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Email = model.Email,
                    Name = model.Name,
                    PasswordHash = BCrypt.HashPassword(model.Password)
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // You may want to log the user in right after registration
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()) // Add the UserId as a claim
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Sign in the user immediately after registration
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                _logger.LogInformation("User registered and logged in successfully");

                return RedirectToAction("Index", "Home"); // Redirect to home page after successful registration
            }

            return View(model);
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the user exists by email
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

                if (user != null && BCrypt.Verify(model.Password, user.PasswordHash))
                {
                    // If login is successful, create authentication cookie
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Name),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()) // Add the UserId as a claim
                    };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Sign in the user
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    _logger.LogInformation("User logged in successfully");

                    return RedirectToAction("Index", "Home"); // Redirect to home page after successful login
                }
                else
                {
                    // Add error to the model if email/password is incorrect
                    ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email and password.");
                }
            }

            return View(model);
        }

        // GET: /Account/Profile
        [Authorize] // Ensure that only authenticated users can access the profile page
        public IActionResult Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID from claims

            // Ensure the userId is a valid integer before querying the database
            if (!int.TryParse(userId, out int parsedUserId))
            {
                return BadRequest("Invalid user ID.");
            }

            // Find the user in the database by UserId (now correctly parsed as an int)
            var user = _context.Users.FirstOrDefault(u => u.UserId == parsedUserId);

            if (user == null)
            {
                return NotFound();
            }

            var profileViewModel = new ProfileViewModel
            {
                Name = user.Name,
                Email = user.Email
                // Add other properties if needed (e.g., Profile Picture, etc.)
            };

            return View(profileViewModel); // Return the ProfileViewModel to the view
        }

        // POST: /Account/Profile
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims

                // Ensure the userId is a valid integer before querying the database
                if (!int.TryParse(userId, out int parsedUserId))
                {
                    return BadRequest("Invalid user ID.");
                }

                var user = await _context.Users.FindAsync(parsedUserId); // Find the user by UserId (using the parsed ID)

                if (user == null)
                {
                    return NotFound();
                }

                user.Name = model.Name;  // Update the user's name
                user.Email = model.Email; // Update the user's email (optional)

                // Save changes to the database
                await _context.SaveChangesAsync();

                _logger.LogInformation("User profile updated successfully");

                return RedirectToAction("Profile"); // Redirect to the profile page after updating
            }

            return View(model); // If validation fails, return the model to the view to show errors
        }


        // Logout method
        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            // Sign out the user by clearing the authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            _logger.LogInformation("User logged out successfully");

            // Redirect to the login page after logout
            return RedirectToAction("Login", "Account");
        }
    }
}
