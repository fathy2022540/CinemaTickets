using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace CinemaTickets.Models
{
    [PrimaryKey("Id", "Img")]

    public class MovieSupImg
    {
 
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Img { get; set; } = string.Empty;
        public Movie Movies { get; set; }
    }
}
