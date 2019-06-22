using Lab1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.ViewModels
{
    public class UserGetModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public static UserGetModel FromUser(User user)
        {
            return new UserGetModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}
