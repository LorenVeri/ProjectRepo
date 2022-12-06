using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CalendarWork.Core.Entities
{
    [Table("Books")]
    public class Book
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Price { get; set; }

        public int? AuthorID { get; set; }

        public Author? Authors { get; set; }
    }
}
