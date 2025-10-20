using CinemaTickets.Data;
using CinemaTickets.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaTickets.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        AppDbContext _context = new AppDbContext();
        public IActionResult Index()
        {
            var movies = _context.movies.AsNoTracking()
                .AsNoTracking()
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .AsQueryable();

            return View(movies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var moviVmmm = new MoviesVm()
            {
                categories = _context.categories.AsNoTracking().AsEnumerable(),
                cinemas = _context.cinemas.AsNoTracking().AsEnumerable()
            };
            return View(moviVmmm);
        }
        [HttpPost]
        public IActionResult Create(MoviesVm moviesVm, IFormFile MainImg, List<IFormFile> SupImg, List<IFormFile> Actors)
        {
            if (MainImg is not null && MainImg.Length > 0)
            {
                var MainImgName = Guid.NewGuid().ToString()
                    + Path.GetExtension(MainImg.FileName);
                var MainImgPath = Path.Combine(Directory.GetCurrentDirectory()
                    , "wwwroot/assets/img/MovieMainImg", MainImgName);

                using (var system = System.IO.File.Create(MainImgPath))
                {
                    MainImg.CopyTo(system);
                }
                moviesVm.Movie.MainImg = MainImgName;
            }
            var movieCreated = _context.movies.Add(moviesVm.Movie);
            _context.SaveChanges();

            if (SupImg is not null && SupImg.Count() > 0)
            {
                foreach (var supImg in SupImg)
                {
                    var SupImgName = Guid.NewGuid().ToString()
                    + Path.GetExtension(supImg.FileName);
                    var SupImgPath = Path.Combine(Directory.GetCurrentDirectory()
                        , "wwwroot/assets/img/MovieSupImg", SupImgName);
                    using (var system = System.IO.File.Create(SupImgPath))
                    {
                        supImg.CopyTo(system);
                    }
                    _context.supImgs.Add(new()
                    {
                        Img = SupImgName,
                        MovieId = movieCreated.Entity.Id
                    });
                }
                _context.SaveChanges();
            }
            if (Actors is not null && Actors.Count() > 0)
            {
                foreach (var actors in Actors)
                {
                    var actorsImgName = Guid.NewGuid().ToString()
                    + Path.GetExtension(actors.FileName);
                    var actorsImgPath = Path.Combine(Directory.GetCurrentDirectory()
                        , "wwwroot/assets/img/ActorsImg", actorsImgName);
                    using (var system = System.IO.File.Create(actorsImgPath))
                    {
                        actors.CopyTo(system);
                    }
                    _context.Actors.Add(new()
                    {
                        ActorImg = actorsImgName,
                        MovieId = movieCreated.Entity.Id
                    });
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var movie = _context.movies.Find(id);
            if (movie == null)
                return NotFound();


            return View(new MoviesVm()
            {
                Movie = movie,
                categories = _context.categories.AsNoTracking().AsEnumerable(),
                cinemas = _context.cinemas.AsNoTracking().AsEnumerable(),
                Actors = _context.Actors.Where(a => a.MovieId == id),
                SupImgs = _context.supImgs.Where(a => a.MovieId == id)
            });
        }
        [HttpPost]
        public IActionResult Edit(MoviesVm moviesVm, IFormFile? MainImg, List<IFormFile>? SupImg, List<IFormFile>? Actors)
        {
            var EditMovie = _context.movies.AsNoTracking().FirstOrDefault(c => c.Id == moviesVm.Movie.Id);
            if (EditMovie == null)
                return NotFound();
            if (MainImg is not null)
            {
                if (MainImg.Length > 0)
                {

                    if (!string.IsNullOrEmpty(EditMovie.MainImg))
                    {
                        var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot/assets/img/cinemaImg", EditMovie.MainImg);

                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }
                    var MainImgName = Guid.NewGuid().ToString()
                   + Path.GetExtension(MainImg.FileName);
                    var MainImgPath = Path.Combine(Directory.GetCurrentDirectory()
                        , "wwwroot/assets/img/MovieMainImg", MainImgName);
                    using (var system = System.IO.File.Create(MainImgPath))
                    {
                        MainImg.CopyTo(system);
                    }
                    moviesVm.Movie.MainImg = MainImgName;

                }
            }
            else
            {
                moviesVm.Movie.MainImg = EditMovie.MainImg;
            }
            if (SupImg is not null)
            {
                if (SupImg.Count > 0)
                {
                    var OldSupImg = _context.supImgs
                        .Where(s => s.MovieId == moviesVm.Movie.Id).ToList();
                    foreach (var item in OldSupImg)
                    {
                        if (!string.IsNullOrEmpty(item.Img))
                        {
                            var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(),
                                "wwwroot/assets/img/MovieSupImg", item.Img);

                            if (System.IO.File.Exists(oldImgPath))
                            {
                                System.IO.File.Delete(oldImgPath);
                            }
                        }
                    }
                    _context.RemoveRange(OldSupImg);
                    _context.SaveChanges();

                    foreach (var item in SupImg)
                    {

                        var SupImgName = Guid.NewGuid().ToString()
                             + Path.GetExtension(item.FileName);
                        var SupImgPath = Path.Combine(Directory.GetCurrentDirectory()
                            , "wwwroot/assets/img/MovieSupImg", SupImgName);
                        using (var system = System.IO.File.Create(SupImgPath))
                        {
                            item.CopyTo(system);
                        }
                        _context.supImgs.Add(new()
                        {
                            Img = SupImgName,
                            MovieId = moviesVm.Movie.Id
                        });

                    }
                    _context.SaveChanges();
                }
            }

            if (Actors is not null)
            {
                if (Actors.Count > 0)
                {
                    var oldActors = _context.Actors.Where(a => a.MovieId == moviesVm.Movie.Id);
                    foreach (var actor in oldActors)
                    {
                        if (!string.IsNullOrEmpty(actor.ActorImg))
                        {
                            var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(),
                                "wwwroot/assets/img/ActorsImg", actor.ActorImg);

                            if (System.IO.File.Exists(oldImgPath))
                            {
                                System.IO.File.Delete(oldImgPath);
                            }
                        }
                    }
                    _context.RemoveRange(oldActors);
                    _context.SaveChanges();

                    foreach (var item in Actors)
                    {
                        var actorsImgName = Guid.NewGuid().ToString()
                    + Path.GetExtension(item.FileName);
                        var actorsImgPath = Path.Combine(Directory.GetCurrentDirectory()
                            , "wwwroot/assets/img/ActorsImg", actorsImgName);
                        using (var system = System.IO.File.Create(actorsImgPath))
                        {
                            item.CopyTo(system);
                        }
                        _context.Actors.Add(new()
                        {
                            ActorImg = actorsImgName,
                            MovieId = moviesVm.Movie.Id
                        });

                    }
                    _context.SaveChanges();
                }
            }
            _context.movies.Update(moviesVm.Movie);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var movie = _context.movies
                .Include(m => m.Category)
                .Include(m => m.Cinema)
                .FirstOrDefault(c => c.Id == id);

            if (movie == null)
                return NotFound();

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _context.movies
                .Include(m => m.SupImgs)
                .Include(m => m.Actors)
                .FirstOrDefault(c => c.Id == id);

            if (movie == null)
                return NotFound();


            if (!string.IsNullOrEmpty(movie.MainImg))
            {
                var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/assets/img/MovieMainImg", movie.MainImg);

                if (System.IO.File.Exists(oldImgPath))
                    System.IO.File.Delete(oldImgPath);
            }


            if (movie.SupImgs != null && movie.SupImgs.Any())
            {
                foreach (var sup in movie.SupImgs)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot/assets/img/MovieSupImg", sup.Img);

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
                _context.supImgs.RemoveRange(movie.SupImgs);
            }


            if (movie.Actors != null && movie.Actors.Any())
            {
                foreach (var actor in movie.Actors)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot/assets/img/ActorsImg", actor.ActorImg);

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
                _context.Actors.RemoveRange(movie.Actors);
            }


            _context.movies.Remove(movie);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
