using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Filters;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.ViewModels;

namespace WarehouseManagementSystem.Controllers;

[SessionAuthorize]
public class StockTransactionsController : Controller
{
    private readonly ApplicationDbContext _context;

    public StockTransactionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    //GET: StockTransactions
    public async Task<IActionResult> Index(int? productId)
    {
        IQueryable<StockTransaction> transactions =
            _context.StockTransactions
                .Include(transaction => transaction.Product);

        if (productId.HasValue)
        {
            transactions = transactions.Where(
                transaction =>
                    transaction.ProductId == productId.Value);
        }

        var transactionList = await transactions
            .OrderByDescending(
                transaction => transaction.TransactionDate)
            .ToListAsync();

        await LoadProductsAsync(productId);

        return View(transactionList);
    }

    //GET: StockTransactions/StockIn
    public async Task<IActionResult> StockIn()
    {
        await LoadProductsAsync(); //retrieves all products from the database and prepares the product dropdown

        return View();
    }

    //POST: StockTransactions/StockIn
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StockIn(
        StockInViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadProductsAsync(model.ProductId);

            return View(model);
        }

        var product = await _context.Products
            .FindAsync(model.ProductId);

        if (product == null)
        {
            return NotFound();
        }

        product.Quantity += model.Quantity;

        var transaction = new StockTransaction
        {
            ProductId = product.Id,
            Quantity = model.Quantity,
            TransactionType = StockTransactionType.In,
            TransactionDate = DateTime.Now,
            Notes = model.Notes
        };

        _context.StockTransactions.Add(
            transaction); //Insert this transaction into the StockTransactions table

        await _context.SaveChangesAsync();

        return RedirectToAction(
            nameof(ProductsController.Index),
            "Products");
    }

    //GET: StockTransactions/StockOut
    public async Task<IActionResult> StockOut()
    {
        await LoadProductsAsync();

        return View();
    }

    //POST: StockTransactions/StockOut
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> StockOut(
        StockOutViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadProductsAsync(model.ProductId);

            return View(model);
        }

        var product = await _context.Products
            .FindAsync(model.ProductId);

        if (product == null)
        {
            return NotFound();
        }

        if (model.Quantity > product.Quantity)
        {
            ModelState.AddModelError(
                nameof(model.Quantity),
                $"Not enough stock. Available quantity: {product.Quantity}.");

            await LoadProductsAsync(model.ProductId);

            return View(model);
        }

        product.Quantity -= model.Quantity;

        var transaction = new StockTransaction
        {
            ProductId = product.Id,
            Quantity = model.Quantity,
            TransactionType = StockTransactionType.Out,
            TransactionDate = DateTime.Now,
            Notes = model.Notes
        };

        _context.StockTransactions.Add(
            transaction); //Insert this transaction into the StockTransactions table

        await _context.SaveChangesAsync();

        return RedirectToAction(
            nameof(ProductsController.Index),
            "Products");
    }

    private async Task LoadProductsAsync(
        int? selectedProductId = null)
    {
        var products = await _context.Products
            .OrderBy(product => product.Name)
            .ToListAsync();

        ViewBag.ProductId = new SelectList(
            products,
            "Id",
            "Name",
            selectedProductId);
    }
}