using BookApi.Models;

namespace BookApi.Dto
{
    public class ResponseBooksDto
    {
        public IEnumerable<Book> Books { get; set; } = null!;
    }
}
