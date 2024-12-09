using CatalogApi.Contracts;
using CatalogApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductsRepository productsRepository, ILogger<ProductsController> logger) : ControllerBase
    {

        //GET api/products
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await productsRepository.GetAllProducts();
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateNewProduct(Product p)
        {
            var newProduct = await productsRepository.CreateNewProduct(p);
            return Created($"/api/products/{newProduct.Id}",newProduct); //201
        }

        //api/products/1283
        [HttpGet("{id}")]
        [ProducesResponseType<Product>(200)]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var p = await productsRepository.GetProductById(id);
            if (p is null)
            {
                return NotFound();
            }
            return Ok(p);
        }


    }
}
