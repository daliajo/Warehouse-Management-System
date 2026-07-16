namespace WarehouseManagementSystem.Models;

public class StockTransaction
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public StockTransactionType TransactionType { get; set; } //In or Out

    public DateTime TransactionDate { get; set; }

    public string? Notes { get; set; }

    public Product Product { get; set; } = null!;
}