using Microsoft.AspNetCore.Mvc;

namespace Jibini.SharedBase.Middleware;

public class ResultStatusWrapperFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext _e, EndpointFilterDelegate next)
    {
        var e = _e.HttpContext;
        try
        {
            var result = await next.Invoke(_e);
            if (result is IActionResult)
            {
                return result;
            }
            return new
            {
                Success = e.Response.StatusCode < 400,
                Result = result
            };
        } catch (Exception ex)
        {
            e.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return new
            {
                Success = false,
                Exception = ex.GetType().FullName,
                ex.Message,
                Stacktrace = ex.StackTrace
            };
        }
    }
}
