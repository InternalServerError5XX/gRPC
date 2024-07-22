using System.Text.Json.Nodes;

namespace BookApi.Dto
{
    public class grpcErrorDto
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public JsonArray Details { get; set; } = null!;
    }
}
