using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace WarehouseManagementSystem.Filters;

public class SessionAuthorizeAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        string? username = context.HttpContext.Session.GetString("Username");

        //to check if user is logged in
        if (username == null)
        {
            context.Result = new RedirectToActionResult(
                "Login",
                "Account",
                null); //redirect to AccountController.Login()
        } 
    }
}