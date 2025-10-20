using System.ComponentModel.DataAnnotations;

namespace CinemaTickets.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        public string Img { get; set; } = "defaultImg.png";
        public bool Status { get; set; }
        public List<Movie> Movies { get; set; }

    }
}
