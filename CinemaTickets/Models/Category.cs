using System.ComponentModel.DataAnnotations;

namespace CinemaTickets.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
