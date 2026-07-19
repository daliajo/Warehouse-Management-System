using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Models;

// 1. create the builder
// 2. register services
// 3. build and configure the app

var builder = WebApplication.CreateBuilder(args); //creates builder

//register services
builder.Services.AddControllersWithViews(); //register MVC

builder.Services.AddSession(); //register session, provides the Session feature that uses that storage

builder.Services.AddDbContext<ApplicationDbContext>(options => //This registers ApplicationDbContext with Dependency Injection
    options.UseSqlServer( //this DbContext uses Microsoft SQL Server
        builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<
    IPasswordHasher<AppUser>,
    PasswordHasher<AppUser>>(); //built-in password hasher

var app = builder.Build();

//Middleware,added after Build()
app.UseHttpsRedirection(); //This redirects HTTP requests to HTTPS
app.UseStaticFiles(); //bc uploaded images are created on disk while the application is running
app.UseRouting(); //examines the request URL and determines which endpoint may handle it
app.UseSession(); //use Session while processing requests
app.UseAuthorization();

app.MapStaticAssets(); //maps static assets as application endpoints

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}") //default route pattern
    .WithStaticAssets(); //apply the application’s static-asset support to the MVC endpoint setup

app.Run();