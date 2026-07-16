using System.ComponentModel.DataAnnotations;

namespace WarehouseManagementSystem.Models;

public class Supplier
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Supplier name is required.")]
    public string Name { get; set; } = string.Empty;

 [RegularExpression(@"^07[789]\d{7}$", ErrorMessage = "Enter a valid Jordanian mobile number, such as 0791234567.")]
    public string? Phone { get; set; }

    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string? Email { get; set; }

    public string? Address { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();//one Supplier can have many Products
    /* ICollection<Product>: This means a collection containing Product objects
    collection Because one supplier can supply multiple products
    = new List<Product>(): this initializes the collection as an empty list*/
}