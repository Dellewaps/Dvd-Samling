using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DVD_Samling.Models;
using DVD_Samling.Models.ViewModels;
using DVD_Samling.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using DVD_Samling.Utility;

namespace DVD_Samling.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        

        public async Task<IActionResult> Index()
        {
            IndexViewModel IndexVM = new IndexViewModel()
            {
                MovieItem = await _db.movieItems.Include(M => M.Genre).ToListAsync(),
                Genre = await _db.Genre.ToListAsync()
            };

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim != null)
            {
                var cnt = _db.RentalCarts.Where(u => u.ApplicationUserId == claim.Value).ToList().Count();
                HttpContext.Session.SetInt32(SD.ssRentalCartCount, cnt);
            }

            return View(IndexVM);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var movieItemFromDb = await _db.movieItems.Include(m => m.Genre).Where(m => m.Id == id).FirstOrDefaultAsync();

            RentalCart cartObj = new RentalCart()
            {
                MovieItem = movieItemFromDb,
                MovieItemId = movieItemFromDb.Id
            };

            return View(cartObj);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(RentalCart CartObject)
        {
            CartObject.Id = 0;
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                CartObject.ApplicationUserId = claim.Value;

                RentalCart cartFromDb = await _db.RentalCarts.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId && c.MovieItemId == CartObject.MovieItemId).FirstOrDefaultAsync();

                if(cartFromDb == null)
                {
                    await _db.RentalCarts.AddAsync(CartObject);
                }

                await _db.SaveChangesAsync();

                var count = _db.RentalCarts.Where(c => c.ApplicationUserId == CartObject.ApplicationUserId).ToList().Count();
                HttpContext.Session.SetInt32(SD.ssRentalCartCount, count);

                return RedirectToAction("Index");
            }
            else
            {
                var movieItemFromDb = await _db.movieItems.Include(m => m.Genre).Where(m => m.Id == CartObject.MovieItemId).FirstOrDefaultAsync();

                RentalCart cartObj = new RentalCart()
                {
                    MovieItem = movieItemFromDb,
                    MovieItemId = movieItemFromDb.Id
                };

                return View(cartObj);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
