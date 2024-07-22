using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace BookApi
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var response = new
            {
                Error = context.Exception.Message,
                Trace = context.Exception.StackTrace
            };

            context.Result = new BadRequestObjectResult(response);
            context.ExceptionHandled = true;
        }
    }
}
