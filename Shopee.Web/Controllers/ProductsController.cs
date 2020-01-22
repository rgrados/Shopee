
namespace Shopee.Web.Controllers
{
    using System.Threading.Tasks;
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Data.Entities;
    using Helpers;
    using Models;
    using Mappers;

    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;

        private readonly IUserHelper userHelper;

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper)
        {
            this.productRepository = productRepository;
            this.userHelper = userHelper;
        }

        public IActionResult Index()
        {
            return View(productRepository.GetAll());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                string path = string.Empty;

                if (productViewModel.ImageFile != null && productViewModel.ImageFile.Length > 0)
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products", productViewModel.ImageFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await productViewModel.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/products/{productViewModel.ImageFile.FileName}";
                }

                Product product = productViewModel.ToProduct(path);

                // TODO: Pending to change to: this.User.Identity.Name
                product.User = await userHelper.GetUserByEmailAsync("grados_2008@hotmail.com");
                await productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(productViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await productRepository.GetByIdAsync(id.Value);

            if (product == null)
            {
                return NotFound();
            }

            ProductViewModel productViewModel = product.ToProductViewModel();

            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string path = productViewModel.ImageUrl;

                    if (productViewModel.ImageFile != null && productViewModel.ImageFile.Length > 0)
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products", productViewModel.ImageFile.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await productViewModel.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/products/{productViewModel.ImageFile.FileName}";
                    }

                    Product product = productViewModel.ToProduct(path);

                    // TODO: Pending to change to: this.User.Identity.Name
                    product.User = await userHelper.GetUserByEmailAsync("grados_2008@hotmail.com");
                    await this.productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await productRepository.ExistAsync(productViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(productViewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await productRepository.GetByIdAsync(id);
            await this.productRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}