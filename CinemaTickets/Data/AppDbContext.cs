using CinemaTickets.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaTickets.Data
{
    public class AppDbContext:DbContext
    {
        public DbSet<Movie> movies { get; set; }
        public DbSet<Cinema> cinemas { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<MovieSupImg> supImgs { get; set; }
        public DbSet<Actor> Actors { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=localhost;Integrated Security=True;DataBase=Cinema;Encrypt=True;Trust Server Certificate=True");
        }
    }
}
