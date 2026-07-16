using Microsoft.EntityFrameworkCore;
using WarehouseManagementSystem.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WarehouseManagementSystem.Data;

public class ApplicationDbContext : DbContext
{
    //constructor
    // : base(options), this sends the options to the parent DbContext class
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    //each DbSet represents a table
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<StockTransaction> StockTransactions { get; set; }
    public DbSet<AppUser> Users { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StockTransaction>()
        .Property(transaction => transaction.TransactionType)
        .HasConversion<string>()
        .HasMaxLength(3);

        //to prevent duplicate usernames
        modelBuilder.Entity<AppUser>()
            .HasIndex(user => user.Username)
            .IsUnique();
    }
}
























    // More complex version using addition instructions needs OnModelCreating:
    
    // OnModelCreating method provides additional instructions to EF Core about the database model
    // protected override void OnModelCreating(ModelBuilder modelBuilder)
    // {
    //     base.OnModelCreating(modelBuilder);//allow the parent DbContext to apply its normal configuration first,then you add your custom rules afterward

    //     //store transaction type as In or Out instead of 0 or 1
    //     modelBuilder.Entity<StockTransaction>()
    //         .Property(transaction => transaction.TransactionType)
    //         .HasConversion<string>()
    //         .HasMaxLength(3);

    //     // //Fluent API style:

    //     // //One Supplier → Many Products
    //     // //Each Product → One Supplier
    //     // //FK is SupplierId
    //     // modelBuilder.Entity<Product>()
    //     //     .HasOne(product => product.Supplier)
    //     //     .WithMany(supplier => supplier.Products)
    //     //     .HasForeignKey(product => product.SupplierId);

    //     // //One Product → Many StockTransactions
    //     // //Each StockTransaction → One Product
    //     // //FK is ProductId
    //     // modelBuilder.Entity<StockTransaction>()
    //     //     .HasOne(transaction => transaction.Product)
    //     //     .WithMany(product => product.StockTransactions)
    //     //     .HasForeignKey(transaction => transaction.ProductId);

    //     //     The explicit Fluent API version is only necessary when:
    //     //     Property names do not follow conventions
    //     //     The relationship is unusual
    //     //     You need special delete behavior
    //     //     There are multiple relationships between the same classes
    //     //     None of those applies here.
    // }
