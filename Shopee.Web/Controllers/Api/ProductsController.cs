namespace Shopee.Web.Controllers.Api
{    
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Data;

    [Route("api/[Controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(productRepository.GetAllWithUsers().OrderBy(p => p.Name));
        }
    }
}