using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.ViewModels;

namespace WarehouseManagementSystem.Controllers;

public class AccountController : Controller
{
    private readonly ApplicationDbContext _context; //to use Users table from the databse
    private readonly IPasswordHasher<AppUser> _passwordHasher;

    public AccountController(ApplicationDbContext context, IPasswordHasher<AppUser> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    //GET: Account/Login shows login form
    public IActionResult Login()
    {
        //check if user is already logged in
        if (HttpContext.Session.GetInt32("UserId") != null)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    //POST: Account/Login receives and processes the submitted form
    [HttpPost]
    [ValidateAntiForgeryToken]
    //passing LoginViewModel instead of username and password to keep the related fields and their validation rules together
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _context.Users.FirstOrDefaultAsync(user =>user.Username == model.Username);//to search the users table

        if (user != null)
        {

            var result = //verifying the pass
                _passwordHasher.VerifyHashedPassword(
                    user, //user in db
                    user.PasswordHash, //hash stored in db
                    model.Password); //plain pass entered in the form
            
            // if (user != null &&user.Password == model.Password)
            if (result != PasswordVerificationResult.Failed)
            {
                HttpContext.Session.SetInt32(
                    "UserId",
                    user.Id); //create login session using the database user id

                HttpContext.Session.SetString(
                    "Username",
                    user.Username); //used to display the username when needed

                return RedirectToAction("Index", "Home");
            }
        }

        ModelState.AddModelError(
            "",
            "Invalid username or password.");

        return View(model);
    }

    //POST: Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        //remove only login-related session values
        HttpContext.Session.Remove("UserId");
        HttpContext.Session.Remove("Username");

        return RedirectToAction(nameof(Login));
    }
}