using BooKStore.Dtos.Book;

namespace BooKStore.Dtos.Author
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        //public List<BookDto> Books
        //{
        //    get; set;
        //}
    }
}
