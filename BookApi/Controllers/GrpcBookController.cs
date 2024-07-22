using BookService.Protos;
using BookService.Services;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GrpcBookController(BooksService booksService) : ControllerBase
    {
        private readonly Uri grpcUrl = new Uri("localhost:7273");
    }
}
