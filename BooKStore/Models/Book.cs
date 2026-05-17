namespace BooKStore.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime publishedDate { get; set; } = DateTime.Now;
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}
