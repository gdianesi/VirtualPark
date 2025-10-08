using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VirtualPark.WebApi.Filters.Exception;

public class ExceptionFilter : IExceptionFilter
{
    private readonly Dictionary<Type, Func<System.Exception, IActionResult>> _errors = new()
    {
        {
            typeof(InvalidOperationException),
            ex => new ObjectResult(new
            {
                InnerCode = "InvalidOperation",
                Message = ex.Message
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(ArgumentException),
            ex => new ObjectResult(new
            {
                InnerCode = "IsNullOrWhiteSpace",
                Message = ex.Message
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(FormatException),
            ex => new ObjectResult(new
            {
                InnerCode = "FormatError",
                Message = ex.Message
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(ArgumentOutOfRangeException),
            ex => new ObjectResult(new
            {
                InnerCode = "OutOfRange",
                Message = ex.Message
            })
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            }
        },
        {
            typeof(KeyNotFoundException),
            ex => new ObjectResult(new
            {
                InnerCode = "NotFound",
                Message = ex.Message
            })
            {
                StatusCode = (int)HttpStatusCode.NotFound
            }
        },
        {
            typeof(UnauthorizedAccessException),
            ex => new ObjectResult(new
            {
                InnerCode = "Unauthorized",
                Message = ex.Message
            })
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            }
        }
    };

    public void OnException(ExceptionContext context)
    {
        var exceptionType = context.Exception.GetType();

        var handler = _errors.GetValueOrDefault(exceptionType);

        if(handler != null)
        {
            context.Result = handler(context.Exception);
        }
        else
        {
            context.Result = new ObjectResult(new
            {
                InnerCode = "InternalError",
                Message = "There was an error when processing the request"
            })
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
