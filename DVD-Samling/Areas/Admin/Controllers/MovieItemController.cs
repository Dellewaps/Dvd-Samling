using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DVD_Samling.Data;
using DVD_Samling.Models;
using DVD_Samling.Models.ViewModels;
using DVD_Samling.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVD_Samling.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Admin")]
    public class MovieItemController : Controller
    {
        private readonly ApplicationDbContext _db;
        //Dette er til upload af billeder
        private readonly IWebHostEnvironment _webHostEnviroment;

        [BindProperty]
        public MovieItemViewModel MovieItemVM { get; set; }

        public MovieItemController(ApplicationDbContext db, IWebHostEnvironment webHostEnviroment)
        {
            _db = db;
            _webHostEnviroment = webHostEnviroment;
            MovieItemVM = new MovieItemViewModel()
            {
                genre = _db.Genre,
                MovieItem = new Models.MovieItem()
            };
        }
        public async Task<IActionResult> Index()
        {

            var movieItem = await _db.movieItems.Include(m => m.Genre).ToListAsync();
            return View(movieItem);
        }


        public IActionResult Create()
        {
            return View(MovieItemVM);
        }

        [HttpPost, ActionName("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            if (!ModelState.IsValid)
            {
                return View(MovieItemVM);
            }

            _db.movieItems.Add(MovieItemVM.MovieItem);
            await _db.SaveChangesAsync();

            //Saveing image

            string webRootPath = _webHostEnviroment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var MovieItemFromDb = await _db.movieItems.FindAsync(MovieItemVM.MovieItem.Id);

            if(files.Count > 0)
            {
                //If image was uploaded 
                var uploads = Path.Combine(webRootPath, "images");
                var extension = Path.GetExtension(files[0].FileName);

                using (var filesStream = new FileStream(Path.Combine(uploads, MovieItemVM.MovieItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                MovieItemFromDb.Image = @"\images\" + MovieItemVM.MovieItem.Id + extension;

            }
            else
            {
                //If no image was uploaded, so use default image
                var uploads = Path.Combine(webRootPath, @"images\" + SD.DefaultMovieImage);
                System.IO.File.Copy(uploads, webRootPath + @"\images\" + MovieItemVM.MovieItem.Id + ".png");
                MovieItemFromDb.Image = @"\images\" + MovieItemVM.MovieItem.Id + ".png";
            }

            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            MovieItemVM.MovieItem = await _db.movieItems.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);

            if(MovieItemVM.MovieItem == null)
            {
                return NotFound();
            }
            return View(MovieItemVM);
        }

        [HttpPost, ActionName("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPOST(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(MovieItemVM);
            }



            //Saveing image

            string webRootPath = _webHostEnviroment.WebRootPath;
            var files = HttpContext.Request.Form.Files;

            var MovieItemFromDb = await _db.movieItems.FindAsync(MovieItemVM.MovieItem.Id);

            if (files.Count > 0)
            {
                //New image has been uploaded 
                var uploads = Path.Combine(webRootPath, "images");
                var extension_new = Path.GetExtension(files[0].FileName);

                //Delete the original image
                var imagePath = Path.Combine(webRootPath, MovieItemFromDb.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }


                //upload new image
                using (var filesStream = new FileStream(Path.Combine(uploads, MovieItemVM.MovieItem.Id + extension_new), FileMode.Create))
                {
                    files[0].CopyTo(filesStream);
                }
                MovieItemFromDb.Image = @"\images\" + MovieItemVM.MovieItem.Id + extension_new;

            }

            MovieItemFromDb.Name = MovieItemVM.MovieItem.Name;
            MovieItemFromDb.Description = MovieItemVM.MovieItem.Description;
            MovieItemFromDb.Subtitels = MovieItemVM.MovieItem.Subtitels;
            MovieItemFromDb.GenreId = MovieItemVM.MovieItem.GenreId;
            MovieItemFromDb.Agelimit = MovieItemVM.MovieItem.Agelimit;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //Get all details on film
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                NotFound();
            }

            MovieItemVM.MovieItem = await _db.movieItems.Include(g => g.Genre).SingleOrDefaultAsync(m => m.Id == id);

            if(MovieItemVM.MovieItem == null)
            {
                NotFound();
            }

            return View(MovieItemVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                NotFound();
            }

            MovieItemVM.MovieItem = await _db.movieItems.Include(g => g.Genre).SingleOrDefaultAsync(m => m.Id == id);

            if(MovieItemVM.MovieItem == null)
            {
                NotFound();
            }

            return View(MovieItemVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _webHostEnviroment.WebRootPath;

            MovieItem movieItem = await _db.movieItems.FindAsync(id);

            if( movieItem != null)
            {
                var imagePath = Path.Combine(webRootPath, movieItem.Image.TrimStart('\\'));

                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }

                _db.movieItems.Remove(movieItem);
                await _db.SaveChangesAsync();

            }

            return RedirectToAction(nameof(Index));
        }
    }
}