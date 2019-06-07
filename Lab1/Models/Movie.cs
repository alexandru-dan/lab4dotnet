using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.Models
{
    public enum Watched
    {
        YES, NO
    }

    public enum Genre
    {
        Action, Comedy, Horror, Thriller
    }

    public class Movie
    {
        public int Id { get; set; }
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Movie title must be at least 2 characters!")]
        public string Title { get; set; }
        public string Description { get; set; }
        [EnumDataType(typeof(Genre))]
        public Genre MovieGenre { get; set; }
        public int DurationInMinutes { get; set; }
        public int ReleseYear { get; set; }
        public string Director { get; set; }
        public DateTime DateAdded { get; set; }
        [Range(1,10)]
        public double Rating { get; set; }
        [EnumDataType(typeof(Watched))]
        public Watched WasWatched { get; set; }
        // comments
        public List<Comment> Comments { get; set; }


    }
}
