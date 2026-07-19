using WarehouseManagementSystem.Models;

namespace WarehouseManagementSystem.ViewModels;

public class DashboardViewModel
{
    public int TotalProducts { get; set; }

    public int TotalSuppliers { get; set; }

    public int TotalStockQuantity { get; set; }

    public int LowStockProducts { get; set; }

    public List<StockTransaction> LatestTransactions { get; set; }= new();
}