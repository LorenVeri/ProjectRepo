using CalendarWork.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarWork.Core.Constants
{
    public class DbInitializer
    {
        private readonly CalendarWorkDbContext _context;

        public DbInitializer(CalendarWorkDbContext context)
        {
            _context = context;
        }

        public async Task Seed()
        {
            if (!_context.Permissions.Any())
            {
                var functions = _context.Functions;
                var roles = await _context.Roles.ToListAsync();


                foreach (var function in functions)
                {
                    foreach(var role in roles)
                    {
                        _context.Permissions.Add(new Permission(function.Id, role.Id.ToString(), "CREATE"));
                        _context.Permissions.Add(new Permission(function.Id, role.Id.ToString(), "EDIT"));
                        _context.Permissions.Add(new Permission(function.Id, role.Id.ToString(), "DELETE"));
                        _context.Permissions.Add(new Permission(function.Id, role.Id.ToString(), "VIEW"));
                    }
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
