using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Filters;
using WarehouseManagementSystem.ViewModels;

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
        //query 1: calculates all product statistics in one SQL query
        //to calculate multiple aggregate values over the same collection
        var productStatistics = await _context.Products
            .GroupBy(product => 1) //bc every product has the same key, all products are placed into one group
            .Select(group => new
            {
                TotalProducts = group.Count(),

                TotalStockQuantity = group.Sum(product => product.Quantity),

                LowStockProducts = group.Count(product => product.Quantity < 5)
            }).FirstOrDefaultAsync();

        var model = new DashboardViewModel
        {
            TotalProducts = productStatistics?.TotalProducts ?? 0, //use TotalProducts when statistics exist,else use 0

            //query 2: counts all suppliers
            //bc suppliers come from a different table
            TotalSuppliers = await _context.Suppliers.CountAsync(),

            TotalStockQuantity = productStatistics?.TotalStockQuantity ?? 0,

            LowStockProducts = productStatistics?.LowStockProducts ?? 0,

            //query 3: retrieves the latest 10 transactions and their products
            LatestTransactions = await _context.StockTransactions
                    .Include(transaction => transaction.Product)
                    .OrderByDescending(transaction => transaction.TransactionDate)
                    .Take(10)
                    .ToListAsync()
        };

        return View(model);
    }
}