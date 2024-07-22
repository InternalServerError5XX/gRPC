using BookApi.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using static BookApi.Books;

namespace BookApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [TypeFilter(typeof(ApiExceptionFilter))]
    public class GrpcBookController : ControllerBase
    {
        private BooksClient GetBooksClient()
        {
            var grpcUrl = new Uri("https://localhost:7273");
            var channel = GrpcChannel.ForAddress(grpcUrl);
            return new BooksClient(channel);
        }

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var client = GetBooksClient();
            var response = await client.GetBooksAsync(new GetBooksRequest());

            return Ok(response.Books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var client = GetBooksClient();
            var response = await client.GetBookAsync(new GetBookRequest { Id = id });

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult>AddBook(AddBookRequest bookRequest)
        {
            var client = GetBooksClient();
            var response = await client.AddBookAsync(bookRequest);

            return CreatedAtAction(nameof(GetBook), new { id = response.Id }, response);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateBook(UpdateBookRequest bookRequest)
        {
            var client = GetBooksClient();
            var response = await client.UpdateBookAsync(bookRequest);

            return Ok(response);
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var client = GetBooksClient();
            var response = await client.DeleteBookAsync(new DeleteBookRequest { Id = id });

            return Ok(response);
        }
    }
}
