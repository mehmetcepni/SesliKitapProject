using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using SesliKitapWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using SesliKitapWeb.Data;

namespace SesliKitapWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> MyBooks()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var userBooks = await _userManager.Users
                .Include(u => u.UserBooks)
                .ThenInclude(ub => ub.Book)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            return View(userBooks?.UserBooks ?? new List<UserBook>());
        }

        [Authorize]
        public async Task<IActionResult> Profile(string? id)
        {
            var currentUserId = _userManager.GetUserId(User);
            if (currentUserId == null)
            {
                return Challenge(); // or Unauthorized()
            }

            ApplicationUser? user;
            bool isCurrentUser = false;

            if (string.IsNullOrEmpty(id) || id == currentUserId)
            {
                user = await _userManager.FindByIdAsync(currentUserId);
                isCurrentUser = true;
            }
            else
            {
                user = await _userManager.FindByIdAsync(id);
            }

            if (user == null)
            {
                return NotFound();
            }

            var followersCount = await _context.UserFollows.CountAsync(uf => uf.FollowingId == user.Id && uf.Status == FollowStatus.Accepted);
            var followingCount = await _context.UserFollows.CountAsync(uf => uf.FollowerId == user.Id && uf.Status == FollowStatus.Accepted);
            var followRelation = await _context.UserFollows.FirstOrDefaultAsync(uf => uf.FollowerId == currentUserId && uf.FollowingId == user.Id);
            var isFollowing = followRelation != null && followRelation.Status == FollowStatus.Accepted;

            // Kullanıcının kitaplarını al
            var userBooks = await _context.UserBooks
                .Include(ub => ub.Book)
                .Where(ub => ub.UserId == user.Id)
                .Select(ub => ub.Book)
                .ToListAsync();

            var viewModel = new ProfileViewModel
            {
                User = user,
                FollowersCount = followersCount,
                FollowingCount = followingCount,
                IsFollowing = isFollowing,
                IsCurrentUser = isCurrentUser,
                UserBooks = userBooks
            };

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Followers(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var followers = await _context.UserFollows
                .Where(uf => uf.FollowingId == id)
                .Select(uf => uf.Follower)
                .ToListAsync();

            ViewBag.UserName = user.UserName;
            return View(followers);
        }

        [Authorize]
        public async Task<IActionResult> Following(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var following = await _context.UserFollows
                .Where(uf => uf.FollowerId == id)
                .Select(uf => uf.Following)
                .ToListAsync();
            
            ViewBag.UserName = user.UserName;
            return View(following);
        }

        // POST: Account/ToggleTheme
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleTheme()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            // Toggle the theme
            user.IsDarkMode = !user.IsDarkMode;
            await _userManager.UpdateAsync(user);

            return RedirectToAction(nameof(Profile));
        }

        [Authorize]
        public async Task<IActionResult> Search(string query)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var viewModel = new SearchViewModel { Query = query ?? "" };

            if (!string.IsNullOrWhiteSpace(query))
            {
                // Kullanıcıları ara
                var users = await _userManager.Users
                    .Where(u => u.FirstName.Contains(query) || 
                                u.LastName.Contains(query) || 
                                u.UserName.Contains(query) ||
                                u.Email.Contains(query))
                    .ToListAsync();

                var searchResults = new List<UserSearchResult>();
                
                foreach (var user in users)
                {
                    // Kendini gösterme
                    if (user.Id == currentUser.Id)
                        continue;

                    var followersCount = await _context.UserFollows.CountAsync(uf => uf.FollowingId == user.Id && uf.Status == FollowStatus.Accepted);
                    var followingCount = await _context.UserFollows.CountAsync(uf => uf.FollowerId == user.Id && uf.Status == FollowStatus.Accepted);
                    var booksCount = await _context.UserBooks.CountAsync(ub => ub.UserId == user.Id);
                    var followRelation = await _context.UserFollows.FirstOrDefaultAsync(uf => uf.FollowerId == currentUser.Id && uf.FollowingId == user.Id);
                    var isFollowing = followRelation != null && followRelation.Status == FollowStatus.Accepted;

                    searchResults.Add(new UserSearchResult
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        FollowersCount = followersCount,
                        FollowingCount = followingCount,
                        BooksCount = booksCount,
                        IsFollowing = isFollowing
                    });
                }

                viewModel.Users = searchResults;
            }

            return View(viewModel);
        }
    }
}