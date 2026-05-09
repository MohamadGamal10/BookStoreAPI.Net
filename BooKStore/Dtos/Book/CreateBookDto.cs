namespace BooKStore.Dtos.Book
{
    public class CreateBookDto
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime publishedDate { get; set; } = DateTime.Now;
    }
}
