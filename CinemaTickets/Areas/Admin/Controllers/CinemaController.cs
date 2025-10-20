using CinemaTickets.Data;
using CinemaTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaTickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        AppDbContext _context = new AppDbContext();
        public IActionResult Index()
        {
            var cinema = _context.cinemas.AsNoTracking().AsQueryable();

            return View(cinema.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Cinema cinema, IFormFile Img)
        {

            if (Img is not null && Img.Length > 0)
            {
                var ImgName = Guid.NewGuid().ToString() + Path.GetExtension(Img.FileName);
                var ImgPath = Path.Combine(Directory.GetCurrentDirectory()
                    , "wwwroot/assets/img/cinemaImg", ImgName);
                using (var system = System.IO.File.Create(ImgPath))
                {
                    Img.CopyTo(system);
                }
                cinema.Img = ImgName;
            }
            _context.cinemas.Add(cinema);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var edit = _context.cinemas.AsNoTracking().FirstOrDefault(c => c.Id == id);
            if (edit == null)
                return NotFound();

            return View(edit);
        }
        [HttpPost]
        public IActionResult Edit(Cinema cinema, IFormFile? Img)
        {
            var oldCinema = _context.cinemas.FirstOrDefault(c => c.Id == cinema.Id);
            if (oldCinema == null)
                return NotFound();


            oldCinema.Name = cinema.Name;
            oldCinema.Status = cinema.Status;

            if (Img != null && Img.Length > 0)
            {

                if (!string.IsNullOrEmpty(oldCinema.Img))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot/assets/img/cinemaImg", oldCinema.Img);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }


                var imgName = Guid.NewGuid() + Path.GetExtension(Img.FileName);
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/assets/img/cinemaImg", imgName);

                using (var stream = System.IO.File.Create(imgPath))
                {
                    Img.CopyTo(stream);
                }

                oldCinema.Img = imgName;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var cinema = _context.cinemas.FirstOrDefault(c => c.Id == id);
            if (cinema == null) return NotFound();
            return View(cinema);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var cinema = _context.cinemas.FirstOrDefault(c => c.Id == id);
            if (cinema == null) return NotFound();
            if (!string.IsNullOrEmpty(cinema.Img))
            {
                var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/assets/img/cinemaImg", cinema.Img);

                if (System.IO.File.Exists(oldImgPath))
                {
                    System.IO.File.Delete(oldImgPath);
                }
            }
            _context.cinemas.Remove(cinema);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}
