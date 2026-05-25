using BooKStore.Dtos.Book;

namespace BooKStore.Dtos.Author
{
    public class UpdateAuthorDto
    {
        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        //public List<int> BookIds { get; set; } = [];

        //public List<BookDto> Books
        //{
        //    get; set;
        //}
    }
}
