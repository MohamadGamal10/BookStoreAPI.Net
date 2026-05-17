using BooKStore.Models;

namespace BooKStore.Dtos.Book
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public DateTime publishedDate { get; set; } = DateTime.Now;
    }
}
