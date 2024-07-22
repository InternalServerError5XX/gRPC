using BookService.Models;
using BookService.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace BookService.Services
{
    public class BooksService : Books.BooksBase
    {
        private static readonly List<Book> Books = new List<Book>
        {
            new Book { Id = 1, Title = "Book 1", Author = "Author 1" },
            new Book { Id = 2, Title = "Book 2", Author = "Author 2" },
        };

        public override async Task<GetBookResponse> GetBook(GetBookRequest request, ServerCallContext context)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var response = Books.FirstOrDefault(x => x.Id == request.Id);
            if (response == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));

            return await Task.FromResult(new GetBookResponse
            {
                Id = response.Id,
                Title = response.Title,
                Author = response.Author,
            });
        }

        public override async Task<GetBooksResponse> GetBooks(GetBooksRequest request, ServerCallContext context)
        {
            var response = new GetBooksResponse();

            foreach (var book in Books)
            {
                response.Books.Add(new GetBookResponse
                {
                    Id = book.Id,
                    Title = book.Title,
                    Author = book.Author,
                });
            }

            return await Task.FromResult(response);
        }

        public override async Task<GetBookResponse> AddBook(AddBookRequest request, ServerCallContext context)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Title == null || request.Author == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Enter the info required"));

            var book = new Book
            {
                Id = Books.Max(x => x.Id) + 1,
                Title = request.Title,
                Author = request.Author,
            };

            Books.Add(book);
            return await Task.FromResult(new GetBookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
            });
        }

        public override async Task<GetBookResponse> UpdateBook(UpdateBookRequest request, ServerCallContext context)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (request.Title == null || request.Author == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Enter the info required"));

            var response = Books.FirstOrDefault(x => x.Id == request.Id);
            if (response == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));

            response.Title = request.Title;
            response.Author = request.Author;

            return await Task.FromResult(new GetBookResponse
            {
                Id = response.Id,
                Title = response.Title,
                Author = response.Author,
            });
        }

        public override async Task<DeleteBookResponse> DeleteBook(DeleteBookRequest request, ServerCallContext context)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var response = Books.FirstOrDefault(x => x.Id == request.Id);
            if (response == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Book not found"));

            Books.Remove(response);

            return await Task.FromResult(new DeleteBookResponse
            {
                IsDeleted = true,
            });
        }
    }
}
