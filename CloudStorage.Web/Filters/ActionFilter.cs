using Microsoft.AspNetCore.Mvc.Filters;

namespace CloudStorage.Web.Filters;

public class ActionFilter : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        base.OnActionExecuted(context);
    }
}