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
    public class UserRolesController : ControllerBase
    {
        private IUserRoleService userRoleService;

        public UserRolesController(IUserRoleService userRoleService)
        {
            this.userRoleService = userRoleService;
        }

        /// <summary>
        /// Get all userRoles
        /// </summary>
        /// <returns>A list of all userRoles</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<UserRoleGetModel> GetAll()
        {
            return userRoleService.GetAll();
        }


        /// <summary>
        /// Find an userRole by the given id.
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /userRoles
        ///     {  
        ///        id: 3,
        ///        name = "Regular",
        ///        description = "Default role for new user"
        ///     }
        /// </remarks>
        /// <param name="id">The id given as parameter</param>
        /// <returns>The userRole with the given id</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/UserRoles/5
        [HttpGet("{id}", Name = "GetUserRole")]
        public IActionResult Get(int id)
        {
            var found = userRoleService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }


        /// <summary>
        /// Add an new UserRole
        /// </summary>
        ///   /// <remarks>
        /// Sample response:
        ///
        ///     Post /userRoles
        ///     {
        ///        name = "Regular",
        ///        description = "Default role for new user"
        ///     }
        /// </remarks>
        /// <param name="userRolePostModel">The input userRole to be added</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public void Post([FromBody] UserRolePostModel userRolePostModel)
        {
            userRoleService.Create(userRolePostModel);
        }


        /// <summary>
        /// Modify an userRole if exists in dbSet , or add if not exist
        /// </summary>
        /// <param name="id">id-ul userRole to update</param>
        /// <param name="userRolePostModel">obiect userRolePostModel to update</param>
        /// Sample request:
        ///     <remarks>
        ///     Put /userRoles/id
        ///     {
        ///        name = "Regular",
        ///        description = "Default role for new user"
        ///     }
        /// </remarks>
        /// <returns>Status 200 daca a fost modificat</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserRolePostModel userRolePostModel)
        {
            var result = userRoleService.Upsert(id, userRolePostModel);
            return Ok(result);
        }


        /// <summary>
        /// Delete an userRole
        /// </summary>
        /// <param name="id">UserRole id to delete</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = userRoleService.Delete(id);
            if (result == null)
            {
                return NotFound("User with the given id not fount !");
            }
            return Ok(result);
        }




    }
}