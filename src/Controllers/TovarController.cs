using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Database;
using TaskManager.Database.Models;
using TaskManager.Schemas;

namespace TaskManager.Controllers
{
    [SwaggerTag("products")]
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly TaskManagerContext _context;

        public ProductController(TaskManagerContext context)
        {
            _context = context;
        }

        [HttpPost(Name = "create-product")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] CreateProductScheme model)
        {
            var File = await _context.FileModels.FirstOrDefaultAsync(x => x.Id == model.Fileid);

            // Проверяем, что File не равен null перед использованием
            if (File == null)
            {
                return BadRequest("File not found");
            }

            var product = new Product()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Photo = File
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        [HttpGet("{id}", Name = "get-product")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Product>> GetProduct(Guid id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound(new JsonResult("Product not found") { StatusCode = 404 });
            }

            return Ok(product);
        }

        [HttpGet(Name = "get-products")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<List<Product>>> GetProducts([FromQuery] GetProductsScheme model)
        {
            var products = await _context.Products
                .Skip(model.Start)
                .Take(model.End)
                .ToListAsync();

            return Ok(products);
        }

        [HttpDelete("{id}", Name = "delete-product")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<ActionResult<Product>> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new JsonResult("Product not found") { StatusCode = 404 });
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }
    }
}
