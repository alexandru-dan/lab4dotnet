using System;
using System.Collections.Generic;
using System.Linq;
using Lab1.Models;
using Lab1.Services;
using Lab1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {

        private IMovieService service;

        public MovieController(IMovieService service)
        {
            this.service = service;
        }


        /// <summary>
        /// Gets all the Movies from the application.
        /// </summary>
        /// <param name="from">Optional, filter by minimum DateAdded.</param>
        /// <param name="to">Optional, filter by maximum DateAdded.</param>
        /// <param name="page">Optional, page wanted</param>
        /// <returns>A list of movie objects with/without filters.</returns>
        /// 
        ///
        // GET: api/Movie
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public PaginatedList<MovieGetModel> GetAllMovies([FromQuery]DateTime? from, [FromQuery]DateTime? to, [FromQuery]int page = 1)
        {
            page = Math.Max(page, 1);
            return service.GetAllMovies(page, from, to);
        }

        /// <summary>
        /// Get a single movie
        /// </summary>
        /// <param name="id"> The id of the movie </param>
        /// <returns>A movie if exists, or Not found if not exists</returns>
        // GET: api/Movie/5
        [HttpGet("{id}", Name = "Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(int id)
        {
            var movieFound = service.GetById(id);

            if (movieFound == null)
            {
                return NotFound();
            }
            return Ok(movieFound);
        }

        /// <summary>
        /// Add's a movie in the application
        /// </summary>
        /// Sample request:
        ///
        ///     POST /movies
        ///     {
        ///         "title": "Movie with Service",
        ///         "description": "Service description",
        ///         "movieGenre": "Comedy",
        ///         "durationInMinutes": 100,
        ///         "releseYear": 2019,
        ///         "director": "Jhon Snow",
        ///         "dateAdded": "2019-10-10",
        ///         "rating": 6,
        ///         "wasWatched": "YES"
        ///     }	
        /// <param name="movie">The movie to be added</param>
        /// <returns>OK if movie was added. BadRequest if not</returns>
        // POST: api/Movie
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Post([FromBody] MoviePostModel movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            service.Create(movie);
            return Ok(movie);
        }

        /// <summary>
        /// Updates a movie from the application if exists, if not creates a new movie
        /// </summary>
        /// Sample request:
        ///
        ///     PUT /movies
        ///     {
        ///         "title": "Movie with Service",
        ///         "description": "Service description",
        ///         "movieGenre": "Comedy",
        ///         "durationInMinutes": 100,
        ///         "releseYear": 2019,
        ///         "director": "Jhon Snow",
        ///         "dateAdded": "2019-10-10",
        ///         "rating": 6,
        ///         "wasWatched": "YES"
        ///     }		
        /// <param name="id">The id of the movie to be updated</param>
        /// <param name="movie">The updated movie</param>
        /// <returns>Ok if movie was updated or added, BadRequest if the movie parameters are not valid</returns>
        // PUT: api/Movie/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Put(int id, [FromBody] Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            service.Upsert(id, movie);
            return Ok();
        }

        /// <summary>
        /// Deletes a movie from the application by ID that was given
        /// </summary>
        /// <param name="id">The ID of the movie to be deleted</param>
        /// <returns>Ok if movie was deleted, NotFound if movieId not in database</returns>
        // DELETE: api/Movie/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(int id)
        {
            var result = service.Delete(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
