using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WarehouseManagementSystem.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Precision(18, 2)]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
    public int Quantity { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Please select a supplier.")]
    public int SupplierId { get; set; }

    public string? ImageFileName { get; set; }

    public Supplier? Supplier { get; set; }

    public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>(); //one Product can have many stock transactions
}