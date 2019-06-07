using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab1.Services;
using Lab1.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ICommentService service;

        public CommentController(ICommentService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Get all the comments from the database
        /// </summary>
        /// <param name="filter">Optional, filter by a specific text</param>
        /// <returns>A list of comments</returns>
        // GET: api/Movie
        [HttpGet]
        public IEnumerable<CommentGetModel> GetAllComments([FromQuery] string filter)
        {
            return service.GetAllComments(filter);
        }
    }
}