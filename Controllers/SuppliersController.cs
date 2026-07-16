using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Data;
using WarehouseManagementSystem.Filters;
using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.Controllers;

[SessionAuthorize]
public class SuppliersController : Controller
{
    private readonly ApplicationDbContext _context;

    //DI
    public SuppliersController(ApplicationDbContext context)
    {
        _context = context;
    }

    //GET: Suppliers
    public async Task<IActionResult> Index()
    {
        var suppliers = await _context.Suppliers.ToListAsync(); //this reads all supplier rows

        return View(suppliers);
    }

    //GET: Suppliers/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
    }

    //GET: Suppliers/Create
    //only shows the form
    public IActionResult Create()
    {
        return View();
    }

    //POST: Suppliers/Create
    //receives the submitted form
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Supplier supplier)
    {
        if (!ModelState.IsValid)
        {
            return View(supplier);
        }

        _context.Suppliers.Add(supplier);//add the supplier
        await _context.SaveChangesAsync(); //executes the SQL INSERT

        return RedirectToAction(nameof(Index));
    }

    //GET: Suppliers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var supplier = await _context.Suppliers.FindAsync(id); //existingSupplier is the actual database entity being tracked by EF Core

        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
    }

    //POST: Suppliers/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Supplier supplier)
    {
        if (id != supplier.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(supplier);
        }

        var existingSupplier = await _context.Suppliers.FindAsync(id);

        if (existingSupplier == null)
        {
            return NotFound();
        }

        existingSupplier.Name = supplier.Name;
        existingSupplier.Phone = supplier.Phone;
        existingSupplier.Email = supplier.Email;
        existingSupplier.Address = supplier.Address;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    //GET: Suppliers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
 
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
        {
            return NotFound();
        }

        return View(supplier);
    }

    //POST: Suppliers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var supplier = await _context.Suppliers.FindAsync(id);

        if (supplier == null)
        {
            return NotFound();
        }

        _context.Suppliers.Remove(supplier);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}