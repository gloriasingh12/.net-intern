/* * PROJECT: Basic E-commerce Application
 * TASK 1: Product Listing & Shopping Cart using ASP.NET MVC
 * TECHNOLOGY: C#, Entity Framework Core, MVC Pattern
 */

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

// 1. MODEL: Product Structure
public class Product {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
}

// 2. CONTROLLER: Managing Store Logic
public class ProductsController : Controller {
    
    // Database Context (Entity Framework)
    private readonly ApplicationDbContext _context;
    
    public ProductsController(ApplicationDbContext context) {
        _context = context;
    }

    // TASK: Display all products
    public IActionResult Index() {
        var products = _context.Products.ToList();
        return View(products);
    }

    // TASK: Add item to Shopping Cart (Session based)
    [HttpPost]
    public IActionResult AddToCart(int productId) {
        var product = _context.Products.Find(productId);
        if (product == null) return NotFound();

        // Cart Logic: Get current cart from Session or create new
        var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>();
        
        cart.Add(product);
        
        // Save updated cart back to Session
        HttpContext.Session.SetObjectAsJson("Cart", cart);

        return RedirectToAction("Index");
    }

    // TASK: View Cart
    public IActionResult ViewCart() {
        var cart = HttpContext.Session.GetObjectFromJson<List<Product>>("Cart") ?? new List<Product>();
        return View(cart);
    }
}
