using Microsoft.AspNetCore.Mvc.Filters;

namespace jonas.Extensions;

public class ModelValidationFilter : IActionFilter
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            throw new BadHttpRequestException("Model not valid");
        }
    }
}
