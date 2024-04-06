using AutoMapper;
using BookStore.Domain.Core;
using BookStore.Services.Interfaces.Dto;

namespace BookStoreApp.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<BookWithoutIdDto, Book>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author));

        }
    }
}
