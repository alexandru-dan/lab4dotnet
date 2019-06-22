using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lab1.Models;
using Lab1.Services;
using Lab1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab1.Controllers
{
    [Authorize(Roles = "UserManager, Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService userService;
        private IUserToRoleServices usersRole;

        public UsersController(IUsersService userService, IUserToRoleServices usersRole)
        {
            this.userService = userService;
            this.usersRole = usersRole;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginPostModel login)
        {
            var user = userService.Authenticate(login.Username, login.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        //[HttpPost]
        public IActionResult Register([FromBody]RegisterPostModel registerModel)
        {
            var user = userService.Register(registerModel);
            if (user != null)
            {
                return BadRequest(user);
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = userService.GetAll();
            return Ok(users);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        [Route("~/api/users/det/{id}")]     //suprascrie ruta prestabilita [Route("api/[controller]")]
        public IActionResult Get(int id)
        {
            var found = userService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }

        [HttpGet]
        [Route("~/api/users/history/{id}")]     //suprascrie ruta prestabilita [Route("api/[controller]")]
        public IActionResult GetHistoryRole(int id)
        {
            var found = usersRole.GetHistoryRoleById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public void Post([FromBody] RegisterPostModel userPostModel)        //pentru creare de User
        {
            userService.Create(userPostModel);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPost]
        [Route("~/api/users/chrole")]     //suprascrie ruta prestabilita [Route("api/[controller]")]
        public IActionResult Post([FromBody] UserToRolePostModel userUserRolePostModel)        //pentru creare de UserToRole cu legatura manyToMany intre User si UserRole
        {
            User curentUserLogIn = userService.GetCurrentUser(HttpContext);
            string roleNameLoged = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;

            string curentUserRoleName = usersRole.GetUserRoleNameById(userUserRolePostModel.UserId);

            if (roleNameLoged.Equals("UserManager"))
            {
                var anulUserRegistered = curentUserLogIn.DataRegistered;        //data inregistrarii
                var curentMonth = DateTime.Now;                                 //data curenta
                var nrLuni = curentMonth.Subtract(anulUserRegistered).Days / (365.25 / 12);   //diferenta in luni dintre datele transmise

                if (nrLuni >= 6)
                {
                    string activRoleName = usersRole.GetUserRoleNameById(userUserRolePostModel.UserId);

                    if (activRoleName.Equals("Admin"))
                    {
                        return Forbid("Nu ai Rolul necesar pentru aceasta operatie !");
                    }

                    if ((activRoleName.Equals("UserManager") | activRoleName.Equals("Regular")) && userUserRolePostModel.UserRoleName.Equals("Admin"))
                    {
                        return Forbid("Nu ai Rolul necesar pentru aceasta operatie !");
                    }
                }
                else
                {
                    return Forbid("Nu ai Vechimea necesara ca UserManager pentru aceasta operatie !");
                }
            }

            usersRole.Create(userUserRolePostModel);
            return Ok();
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserPostModel userPostModel)
        {
            User curentUserLogIn = userService.GetCurrentUser(HttpContext);
            string roleNameLoged = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;

            string curentUserRoleName = usersRole.GetUserRoleNameById(id);

            if (roleNameLoged.Equals("UserManager"))
            {
                //UserGetModel userToUpdate = userService.GetById(id);

                var anulUserRegistered = curentUserLogIn.DataRegistered;        //data inregistrarii
                var curentMonth = DateTime.Now;                                 //data curenta
                var nrLuni = curentMonth.Subtract(anulUserRegistered).Days / (365.25 / 12);   //diferenta in luni dintre datele transmise

                if (nrLuni < 6)
                {
                    return Forbid("Nu ai Vechimea necesara ca UserManager pentru aceasta operatie !");
                }
            }

            var result = userService.Upsert(id, userPostModel);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string roleNameLoged = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;

            if (roleNameLoged.Equals("UserManager"))
            {
                UserGetModel userToDelete = userService.GetById(id);

                string activRoleName = usersRole.GetUserRoleNameById(userToDelete.Id);

                if (activRoleName.Equals("Admin"))
                {
                    return Forbid("Nu ai Rolul necear pentru aceasta operatie !");
                }
            }
            var result = userService.Delete(id);
            if (result == null)
            {
                return NotFound("User with the given id not fount !");
            }
            return Ok(result);
        }
    }
}