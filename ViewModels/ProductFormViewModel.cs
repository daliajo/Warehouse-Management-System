using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; //to use Iformfile

namespace WarehouseManagementSystem.ViewModels;

public class ProductFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Range(0.01,double.MaxValue,ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(0,int.MaxValue,ErrorMessage = "Quantity cannot be negative.")]
    public int Quantity { get; set; }

    [Range(1,int.MaxValue,ErrorMessage = "Please select a supplier.")]
    public int SupplierId { get; set; }

    [Display(Name = "Product Image")]
    public IFormFile? ImageFile { get; set; }

    public string? ExistingImageFileName { get; set; } //used on the Edit page
}