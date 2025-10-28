using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SesliKitapWeb.Data;
using SesliKitapWeb.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SesliKitapWeb.Controllers
{
    [Authorize]
    public class FollowsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FollowsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Follow(string id)
        {
            var userToFollow = await _userManager.FindByIdAsync(id);
            if (userToFollow == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            if (currentUser.Id == id)
            {
                return BadRequest("You cannot follow yourself.");
            }

            var existingFollow = await _context.UserFollows
                .FirstOrDefaultAsync(uf => uf.FollowerId == currentUser.Id && uf.FollowingId == id);

            if (existingFollow == null)
            {
                var follow = new UserFollow
                {
                    FollowerId = currentUser.Id,
                    FollowingId = id,
                    Status = FollowStatus.Pending
                };
                _context.UserFollows.Add(follow);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Takip isteği gönderildi!";
            }
            else if (existingFollow.Status == FollowStatus.Rejected)
            {
                existingFollow.Status = FollowStatus.Pending;
                await _context.SaveChangesAsync();
                TempData["Message"] = "Takip isteği yeniden gönderildi!";
            }

            return RedirectToAction("Profile", "Account", new { id });
        }

        [Authorize]
        public async Task<IActionResult> PendingRequests()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var pendingRequests = await _context.UserFollows
                .Include(uf => uf.Follower)
                .Where(uf => uf.FollowingId == currentUser.Id && uf.Status == FollowStatus.Pending)
                .OrderByDescending(uf => uf.CreatedAt)
                .ToListAsync();

            return View(pendingRequests);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptRequest(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var request = await _context.UserFollows
                .FirstOrDefaultAsync(uf => uf.Id == id && uf.FollowingId == currentUser.Id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = FollowStatus.Accepted;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Takip isteği onaylandı!";
            return RedirectToAction("PendingRequests");
        }

        [HttpPost]
        public async Task<IActionResult> RejectRequest(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var request = await _context.UserFollows
                .FirstOrDefaultAsync(uf => uf.Id == id && uf.FollowingId == currentUser.Id);

            if (request == null)
            {
                return NotFound();
            }

            request.Status = FollowStatus.Rejected;
            await _context.SaveChangesAsync();

            TempData["Message"] = "Takip isteği reddedildi!";
            return RedirectToAction("PendingRequests");
        }

        [HttpPost]
        public async Task<IActionResult> Unfollow(string id)
        {
            var userToUnfollow = await _userManager.FindByIdAsync(id);
            if (userToUnfollow == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            var follow = await _context.UserFollows
                .FirstOrDefaultAsync(uf => uf.FollowerId == currentUser.Id && uf.FollowingId == id);

            if (follow != null)
            {
                _context.UserFollows.Remove(follow);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Profile", "Account", new { id });
        }
    }
}

