﻿
namespace CinemaTickets.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ActorImg { get; set; } = string.Empty;
        public Movie Movie { get; set; }
    }
}
