using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.ViewModels;

public class StockOutViewModel
{
    [Range(1,int.MaxValue,ErrorMessage = "Please select a product.")]
    public int ProductId { get; set; }

    [Range(1,int.MaxValue,ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }

    public string? Notes { get; set; }
}