using AutoMapper;
using BooKStore.Dtos.Book;
using BooKStore.Models;

namespace BooKStore.Profiles
{
    public class Mapping: Profile
    {
        public Mapping()
        {
            CreateMap<Book, BookDto>().ReverseMap();
        }
    }
}
