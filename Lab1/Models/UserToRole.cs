using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.Models
{
    public class UserToRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
