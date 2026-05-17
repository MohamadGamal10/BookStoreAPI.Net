using BooKStore.Models;

namespace BooKStore.Dtos.Book
{
    public class UpdateBookDto
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public DateTime publishedDate { get; set; } = DateTime.Now;
    }
}
