/* * PROJECT: Inventory Management System
 * TASK 4: Real-time Stock Tracking using Entity Framework
 * TECHNOLOGY: C#, LINQ, SQL Server
 */

using Microsoft.AspNetCore.Mvc;
using System.Linq;

public class InventoryController : Controller {
    private readonly ApplicationDbContext _context;

    public InventoryController(ApplicationDbContext context) {
        _context = context;
    }

    // TASK: Update stock after a successful payment
    [HttpPost]
    public IActionResult ProcessOrder(int productId, int quantityRequested) {
        var product = _context.Products.Find(productId);

        if (product == null) return NotFound("Product not found");

        // Check if enough stock is available
        if (product.StockQuantity >= quantityRequested) {
            // Deduct from inventory
            product.StockQuantity -= quantityRequested;
            
            // Save changes to Database
            _context.SaveChanges();
            
            return Ok(new { 
                Message = "Order processed successfully!", 
                RemainingStock = product.StockQuantity 
            });
        } else {
            return BadRequest("Insufficient stock available.");
        }
    }
}
