using Microsoft.AspNetCore.Mvc;
using WarehouseManagementSystem.ViewModels;
namespace WarehouseManagementSystem.Controllers;

public class AccountController : Controller
{
    //GET: Account/Login shows login form
    public IActionResult Login()
    {   //check if user is already logged in
        if (HttpContext.Session.GetString("Username") != null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    //POST: Account/Login  receives and process the submitted form
    [HttpPost]
    [ValidateAntiForgeryToken]
    //passing Loginviewmodel instead of username, password, to keep the related fields and their validation rules together
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (model.Username == "admin" && model.Password == "admin123")
        {
            HttpContext.Session.SetString("Username", model.Username); //create login session

            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid username or password.");

        return View(model);
    }

    //GET: Account/Logout
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction(nameof(Login));
    }
}