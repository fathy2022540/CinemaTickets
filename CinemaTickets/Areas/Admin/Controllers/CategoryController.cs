using CinemaTickets.Data;
using CinemaTickets.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaTickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        AppDbContext _context = new AppDbContext();
        public IActionResult Index()
        {
            var allCategories = _context.categories.AsNoTracking().AsQueryable();
            return View(allCategories.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        public IActionResult Create(Category category)
        {
            _context.categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var categoruy = _context.categories.FirstOrDefault(c => c.Id == id);
            if (categoruy == null)
                return NotFound();

            return View(categoruy);
        }
        [HttpPost]
        public IActionResult Edit(Category category)
        {
            _context.categories.Update(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var category = _context.categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _context.categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
                return NotFound();

            _context.categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
