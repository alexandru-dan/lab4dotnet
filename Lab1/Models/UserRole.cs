using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab1.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IEnumerable<UserToRole> UserToRole { get; set; }
    }
}
