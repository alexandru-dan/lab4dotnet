using Lab1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.ViewModels
{
    public class UserToRoleGetModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UserRoleId { get; set; }

        public string UserRoleName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }


        public static UserToRoleGetModel FromUserToRole(UserToRole userToRole)
        {
            return new UserToRoleGetModel
            {
                Id = userToRole.Id,
                UserId = userToRole.UserId,
                UserRoleId = userToRole.UserRoleId,
                UserRoleName = userToRole.UserRole.Name,
                StartTime = userToRole.StartTime,
                EndTime = userToRole.EndTime
            };
        }
    }
}
