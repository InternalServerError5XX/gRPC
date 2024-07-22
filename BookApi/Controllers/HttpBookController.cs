using BookApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using BookApi.Dto;

namespace BookApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HttpBookController(HttpClient httpClient) : ControllerBase
    {
        private readonly Uri httpUrl = new Uri("https://localhost:7273/v1/books");

        [HttpGet]
        public async Task<IActionResult> GetBooks()
        {
            var response = await httpClient.GetAsync(httpUrl);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var books = await response.Content.ReadFromJsonAsync<ResponseBooksDto>();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var response = await httpClient.GetAsync($"{httpUrl}/{id}");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var books = await response.Content.ReadFromJsonAsync<Book>();
            return Ok(books);
        }

        [HttpPost]
        public async Task<IActionResult> AddBook(RequestBookDto bookDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid book data.");

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
            };

            var content = new StringContent(JsonSerializer.Serialize(bookDto, jsonOptions), Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(httpUrl, content);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, await response.Content.ReadFromJsonAsync<grpcErrorDto>());

            var books = await response.Content.ReadFromJsonAsync<Book>();
            return Ok(books);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateBook(Book book)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid book data.");

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
            };

            var content = new StringContent(JsonSerializer.Serialize(book, jsonOptions), Encoding.UTF8, "application/json");
            var response = await httpClient.PatchAsync(httpUrl, content);
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, await response.Content.ReadFromJsonAsync<grpcErrorDto>());

            var books = await response.Content.ReadFromJsonAsync<Book>();
            return Ok(books);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var response = await httpClient.DeleteAsync($"{httpUrl}/{id}");
            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode);

            var books = await response.Content.ReadFromJsonAsync<ResponseDeleteDto>();
            return Ok(books);
        }
    }
}
