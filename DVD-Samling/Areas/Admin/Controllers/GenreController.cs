using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DVD_Samling.Data;
using DVD_Samling.Models;
using DVD_Samling.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVD_Samling.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Admin")]
    public class GenreController : Controller
    {
        private readonly ApplicationDbContext _db;
        public GenreController(ApplicationDbContext db)
        {
            _db = db;
        }

        //Get all Genre
        public async Task<IActionResult> Index()
        {

            return View(await _db.Genre.ToListAsync());
        }

        //Get - Create
        public IActionResult Create()
        {
            return View();
        }

        //POST -CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _db.Add(genre);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }


        //EDIT - GET
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var genre = await _db.Genre.FindAsync(id);
            if(genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        //EDIT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Genre genre)
        {
            if (ModelState.IsValid)
            {
                _db.Update(genre);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        //DELETE - GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var genre = await _db.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }

        //DELETE - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var genre = await _db.Genre.FindAsync(id);

            if(genre == null)
            {
                return View();
            }
            _db.Genre.Remove(genre);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var genre = await _db.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            return View(genre);
        }
    }
}