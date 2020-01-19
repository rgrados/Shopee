namespace Shopee.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using Shopee.Web.Data;

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
            return Ok(productRepository.GetAll());
        }
    }
}