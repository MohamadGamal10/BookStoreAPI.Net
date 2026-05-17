using AutoMapper;
using BooKStore.Dtos.Author;
using BooKStore.Dtos.Book;
using BooKStore.Models;

namespace BooKStore.Profiles
{
    public class Mapping: Profile
    {
        public Mapping()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Book, CreateBookDto>().ReverseMap();
            CreateMap<Book, UpdateBookDto>().ReverseMap();

            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<Author, CreateAuthorDto>().ReverseMap();
            CreateMap<Author, UpdateAuthorDto>().ReverseMap();
        }
    }
}
