using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WarehouseManagementSystem.Filters;

public class SessionAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        int? userId =
            context.HttpContext.Session.GetInt32("UserId");

        if (userId == null)
        {
            context.Result = new RedirectToActionResult(
                "Login",
                "Account",
                null);
        }
    }
}