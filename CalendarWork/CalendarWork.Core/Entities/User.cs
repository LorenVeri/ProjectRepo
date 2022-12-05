using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarWork.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int RoleId { get; set; }
        public Role? Roles { get; set; }
    }
}
