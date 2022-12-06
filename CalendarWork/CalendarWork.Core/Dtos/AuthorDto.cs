using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarWork.Core.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
