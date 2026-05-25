namespace BooKStore.Dtos.Book
{
    public class BookForSpecificAuthorDto
    {
        public string Title { get; set; }
        public DateTime publishedDate { get; set; } = DateTime.Now;
    }
}
