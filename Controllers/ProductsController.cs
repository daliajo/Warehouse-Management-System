using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Filters;
using WarehouseManagementSystem.Models;
using WarehouseManagementSystem.ViewModels;

namespace WarehouseManagementSystem.Controllers;

[SessionAuthorize]
public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public ProductsController(
        ApplicationDbContext context,
        IWebHostEnvironment environment) //IWebHostEnvironment gives us the path to wwwroot thru _environment.WebRootPath
    {
        _context = context;
        _environment = environment;
    }

    //GET: Products
    public async Task<IActionResult> Index(
        string? searchTerm,
        string? sortOrder,
        int page = 1)
    {
        const int pageSize = 10;

        if (page < 1)
        {
            page = 1;
        }

        IQueryable<Product> products = _context.Products
            .Include(product => product.Supplier);

        //Search by product name or supplier name
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            products = products.Where(product =>
                product.Name.Contains(searchTerm) ||
                (product.Supplier != null &&
                 product.Supplier.Name.Contains(searchTerm))); //product name contains the search or supplier name contains the search
        }

        //Count the matching products before pagination
        int totalProducts = await products.CountAsync();

        int totalPages = (int)Math.Ceiling(
            totalProducts / (double)pageSize);

        if (totalPages > 0 && page > totalPages)
        {
            page = totalPages;
        }

        //Default sorting is product name ascending
        string currentSort =
            string.IsNullOrWhiteSpace(sortOrder)
                ? "name_asc"
                : sortOrder;

        products = currentSort switch
        {
            "name_desc" => products
                .OrderByDescending(product => product.Name)
                .ThenBy(product => product.Id),

            "price_asc" => products
                .OrderBy(product => product.Price)
                .ThenBy(product => product.Id),

            "price_desc" => products
                .OrderByDescending(product => product.Price)
                .ThenBy(product => product.Id),

            _ => products //default case
                .OrderBy(product => product.Name)
                .ThenBy(product => product.Id)
        };

        //retrieve only the 10 products for the current page
        var productList = await products
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SortOrder = currentSort;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.TotalProducts = totalProducts;

        ViewBag.NameSort =
            currentSort == "name_asc"
                ? "name_desc"
                : "name_asc";

        ViewBag.PriceSort =
            currentSort == "price_asc"
                ? "price_desc"
                : "price_asc";

        return View(productList);
    }

    //GET: Products/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            //return NotFound();
            TempData["ErrorMessage"] = "Invalid request.";
            return RedirectToAction(nameof(Index));
        }

        var product = await _context.Products
            .Include(product => product.Supplier)
            .FirstOrDefaultAsync(product => product.Id == id);

        if (product == null)
        {
            //return NotFound();
            TempData["ErrorMessage"] = "Product not found.";
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    //GET: Products/Create
    public async Task<IActionResult> Create()
    {
        await LoadSuppliersAsync();

        return View();
    }

    //POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        ProductFormViewModel model)
    {
        ValidateImage(model.ImageFile);

        if (!ModelState.IsValid)
        {
            await LoadSuppliersAsync(model.SupplierId);

            return View(model);
        }

        string? imageFileName = null;

        if (model.ImageFile != null)
        {
            imageFileName =
                await SaveImageAsync(model.ImageFile);
        }

        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            Quantity = model.Quantity,
            SupplierId = model.SupplierId,
            ImageFileName = imageFileName
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    //GET: Products/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            //return NotFound();
            TempData["ErrorMessage"] = "Invalid request.";
            return RedirectToAction(nameof(Index));
        }

        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            //return NotFound();
            TempData["ErrorMessage"] = "Product not found.";
            return RedirectToAction(nameof(Index));
        }

        var model = new ProductFormViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity,
            SupplierId = product.SupplierId,
            ExistingImageFileName = product.ImageFileName
        };

        await LoadSuppliersAsync(product.SupplierId);

        return View(model);
    }

    //POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        ProductFormViewModel model)
    {
        if (id != model.Id)
        {
            //return NotFound();
        TempData["ErrorMessage"] = "Invalid product request.";
        return RedirectToAction(nameof(Index));
        }

        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            //return NotFound();
        TempData["ErrorMessage"] = "Product not found.";
        return RedirectToAction(nameof(Index));
        }

        ValidateImage(model.ImageFile);

        if (!ModelState.IsValid)
        {
            model.ExistingImageFileName =
                product.ImageFileName;

            await LoadSuppliersAsync(model.SupplierId);

            return View(model);
        }

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.Quantity = model.Quantity;
        product.SupplierId = model.SupplierId;

        string? oldImageFileName = null;

        if (model.ImageFile != null)
        {
            oldImageFileName = product.ImageFileName;

            product.ImageFileName =
                await SaveImageAsync(model.ImageFile);
        }

        await _context.SaveChangesAsync();

        if (oldImageFileName != null)
        {
            DeleteImage(oldImageFileName);
        }

        return RedirectToAction(nameof(Index));
    }

    //GET: Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            //return NotFound();
        TempData["ErrorMessage"] = "Invalid request.";
        return RedirectToAction(nameof(Index));
        }

        var product = await _context.Products
            .Include(product => product.Supplier)
            .FirstOrDefaultAsync(product => product.Id == id);

        if (product == null)
        {
            //return NotFound();
        TempData["ErrorMessage"] = "Product not found.";
        return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    //POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            //return NotFound();
        TempData["ErrorMessage"] = "Product not found.";
        return RedirectToAction(nameof(Index));
        }

        string? imageFileName = product.ImageFileName;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        if (imageFileName != null)
        {
            DeleteImage(imageFileName);
        }

        return RedirectToAction(nameof(Index));
    }

    private void ValidateImage(IFormFile? imageFile)
    {
        if (imageFile == null)
        {
            return;
        }

        string extension = Path
            .GetExtension(imageFile.FileName)
            .ToLowerInvariant();

        string[] allowedExtensions =
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        if (!allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError(
                nameof(ProductFormViewModel.ImageFile),
                "Only JPG and PNG images are allowed.");
        }

        const long maximumFileSize =
            2 * 1024 * 1024;

        if (imageFile.Length > maximumFileSize)
        {
            ModelState.AddModelError(
                nameof(ProductFormViewModel.ImageFile),
                "The image cannot be larger than 2 MB.");
        }

        if (imageFile.Length == 0)
        {
            ModelState.AddModelError(
                nameof(ProductFormViewModel.ImageFile),
                "The selected image is empty.");
        }
    }

    private async Task<string> SaveImageAsync( //SaveImageAsync saves the physical file and returns its generated filename
        IFormFile imageFile)
    {
        string extension = Path
            .GetExtension(imageFile.FileName)
            .ToLowerInvariant();

        string fileName =
            $"{Guid.NewGuid()}{extension}"; //Guid.NewGuid() generates a nearly unique value, to create a unique filename

        string uploadsFolder = Path.Combine(
            _environment.WebRootPath,
            "uploads"); //Building the folder path, wwwroot/uploads

        Directory.CreateDirectory(
            uploadsFolder); //this creates the folder when it does not exist

        string filePath = Path.Combine(
            uploadsFolder,
            fileName); //full path file, \wwwroot\uploads\image.jpg

        //using var means C# automatically closes the stream when the method finishes
        using var stream = new FileStream( //a stream is the connection through which bytes are written to the file
            filePath,
            FileMode.Create);  //means: create a new file at this path

        await imageFile.CopyToAsync(stream); //This copies the uploaded file’s bytes into the new physical file

        return fileName;
    }

    private void DeleteImage(string imageFileName)
    {
        string filePath = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            imageFileName);

        if (System.IO.File.Exists(filePath))
        {
            System.IO.File.Delete(filePath);
        }
    }

    private async Task LoadSuppliersAsync(
        int? selectedSupplierId = null)
    {
        var suppliers = await _context.Suppliers
            .OrderBy(supplier => supplier.Name)
            .ToListAsync(); //retrieves the suppliers from SQL Server and creates a C# list

        ViewData["SupplierId"] = new SelectList( //dropdown
            suppliers,
            "Id",
            "Name",
            selectedSupplierId);
    }
}