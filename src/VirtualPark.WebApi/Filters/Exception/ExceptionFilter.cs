using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VirtualPark.WebApi.Filters.Exception;

public class ExceptionFilter : IExceptionFilter
{
    private readonly Dictionary<Type, IActionResult> _errors = new()
    {
        {
            typeof(ArgumentNullException),
            new ObjectResult(new
            {
                InnerCode = "InvalidArgument",
                Message = "Argument can not be null or empty"
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        }
    };

    public void OnException(ExceptionContext context)
    {
        var response = _errors.GetValueOrDefault(context.Exception.GetType());

        if (response == null)
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "InternalError",
                Message = "There was an error when processing the request"
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
            return;
        }

        context.Result = response;
    }
}
