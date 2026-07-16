using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Filters;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [SessionAuthorize]
    public async Task<IActionResult> Index()
    {
        ViewBag.TotalProducts =
            await _context.Products.CountAsync(); //counts database records

        ViewBag.TotalSuppliers =
            await _context.Suppliers.CountAsync();

        ViewBag.TotalStockQuantity =
            await _context.Products.SumAsync(
                product => product.Quantity); //Adds the quantity of every product

        ViewBag.LowStockProducts =
            await _context.Products.CountAsync(
                product => product.Quantity < 5); //Counts products whose quantity is less than 5

        var latestTransactions =
            await _context.StockTransactions
                .Include(transaction => transaction.Product)
                .OrderByDescending(
                    transaction => transaction.TransactionDate)
                .Take(10) //takes the latest 10 transactions
                .ToListAsync();

        return View(latestTransactions);
    }
}