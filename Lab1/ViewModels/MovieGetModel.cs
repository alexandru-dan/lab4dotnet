using Lab1.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.ViewModels
{
    public class MovieGetModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReleseYear { get; set; }
        public DateTime DateAdded { get; set; }
        public string Director { get; set; }
        public double Rating { get; set; }
        public List<Comment> Comments { get; set; }

        public static MovieGetModel FromMovie(Movie movie)
        {
            return new MovieGetModel
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleseYear = movie.ReleseYear,
                DateAdded = movie.DateAdded,
                Director = movie.Director,
                Rating = movie.Rating,
                Comments = movie.Comments
                
            };
        }
    }
}
