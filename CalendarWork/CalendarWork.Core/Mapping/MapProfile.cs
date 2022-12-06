using AutoMapper;
using CalendarWork.Core.Dtos;
using CalendarWork.Core.Entities;

namespace CalendarWork.Core.Mapping
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Author, AuthorDto>();
            CreateMap<AuthorDto, Author>();

            CreateMap<Book, BookDto>();
            CreateMap<BookDto, Book>();
        }
    }
}
