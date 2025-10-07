using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SteelShop.Api.Filters;

public sealed class ApiExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = 500,
            Title = "Server error",
            Detail = context.Exception.Message
        };
        context.Result = new ObjectResult(details) { StatusCode = 500 };
        context.ExceptionHandled = true;
    }
}