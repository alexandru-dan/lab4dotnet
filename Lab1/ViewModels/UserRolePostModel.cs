using Lab1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.ViewModels
{
    public class UserRolePostModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public static UserRole ToUserRole(UserRolePostModel userRolePostModel)
        {
            return new UserRole
            {
                Name = userRolePostModel.Name,
                Description = userRolePostModel.Description
            };
        }


    }
}
