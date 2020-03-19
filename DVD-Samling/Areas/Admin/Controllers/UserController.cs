using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DVD_Samling.Data;
using DVD_Samling.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVD_Samling.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            return View(await _db.ApplicationUsers.Where(u => u.Id != claim.Value).ToListAsync());
        }

        public async Task<IActionResult> Lock(string id)
        {
            if(id == null)
            {
                NotFound();
            }

            var applicationUser = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);

            if(applicationUser == null)
            {
                NotFound();
            }

            applicationUser.LockoutEnd = DateTime.Now.AddYears(100);

            _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> UnLock(string id)
        {
            if (id == null)
            {
                NotFound();
            }

            var applicationUser = await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == id);

            if (applicationUser == null)
            {
                NotFound();
            }

            applicationUser.LockoutEnd = DateTime.Now;

            _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}