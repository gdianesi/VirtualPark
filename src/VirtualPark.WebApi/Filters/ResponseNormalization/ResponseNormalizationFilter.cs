using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VirtualPark.WebApi.Filters.ResponseNormalization;

public class ResponseNormalizationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if(context.Exception != null)
        {
            return;
        }

        var method = context.HttpContext.Request.Method.ToUpper();

        switch(method)
        {
            case "POST":
                HandlePost(context);
                break;

            case "PUT":
            case "DELETE":
                HandlePutDelete(context);
                break;

            case "GET":
                HandleGet(context);
                break;
        }
    }

    private void HandlePost(ActionExecutedContext context)
    {
        if(context.Result is ObjectResult obj)
        {
            if(obj.StatusCode == null || obj.StatusCode == 200)
            {
                obj.StatusCode = StatusCodes.Status201Created;
            }
        }
        else
        {
            context.Result = new StatusCodeResult(StatusCodes.Status204NoContent);
        }
    }

    private void HandlePutDelete(ActionExecutedContext context)
    {
        if(context.Result is EmptyResult ||
            context.Result is ObjectResult { Value: null })
        {
            context.Result = new StatusCodeResult(StatusCodes.Status204NoContent);
        }
        else if(context.Result is ObjectResult obj)
        {
            obj.StatusCode ??= StatusCodes.Status200OK;
        }
    }

    private void HandleGet(ActionExecutedContext context)
    {
        if(context.Result is ObjectResult obj)
        {
            obj.StatusCode ??= StatusCodes.Status200OK;
        }
    }
}
