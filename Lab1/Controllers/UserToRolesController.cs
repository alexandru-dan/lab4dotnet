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
    public class UserToRolesController : ControllerBase
    {
        private IUserToRoleServices userToRoleService;

        public UserToRolesController(IUserToRoleServices userToRoleService)
        {
            this.userToRoleService = userToRoleService;
        }


        /// <summary>
        /// Find an userToRoles by the given id.
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /userToRoles
        ///     [
        ///     {  
        ///        id: 3,
        ///        userId = 2,
        ///        UserRoleId = 3,
        ///        UserRole = "Regular",
        ///        StartTime = 2019-06-05,
        ///        EndTime = null
        ///     }
        ///     ]
        /// </remarks>
        /// <param name="id">The id given as parameter</param>
        /// <returns>A list of userUserRole with the given id</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/userToRoles/5
        [HttpGet("{id}", Name = "GetUserUserRole")]
        public IActionResult Get(int id)
        {
            var found = userToRoleService.GetHistoryRoleById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }


        /// <summary>
        /// Add an new userToRoles
        /// </summary>
        ///   /// <remarks>
        /// Sample response:
        ///
        ///     Put /userUserRoles
        ///     {
        ///        userId = 1,
        ///        userRoleName = "UserManager"        
        ///     }
        /// </remarks>
        /// <param name="userToRolePostModel">The input userToRoles to be added</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public void Post([FromBody] UserToRolePostModel userToRolePostModel)
        {
            userToRoleService.Create(userToRolePostModel);
        }
    }
}