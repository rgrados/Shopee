namespace Shopee.Web.Mappers
{
    using Shopee.Web.Data.Entities;
    using Shopee.Web.Models;

    public static class ProductMapper
    {
        public static Product ToProduct(this ProductViewModel productViewModel, string imageUrl)
        {
            Product product = null;

            if (productViewModel != null)
            {
                product = new Product
                {
                    Id = productViewModel.Id,
                    ImageUrl = imageUrl,
                    IsAvailabe = productViewModel.IsAvailabe,
                    LastPurchase = productViewModel.LastPurchase,
                    LastSale = productViewModel.LastSale,
                    Name = productViewModel.Name,
                    Price = productViewModel.Price,
                    Stock = productViewModel.Stock,
                };
            }

            return product;
        }

        public static ProductViewModel ToProductViewModel(this Product product)
        {
            ProductViewModel productViewModel = null;

            if (product != null)
            {
                productViewModel = new ProductViewModel
                {
                    Id = product.Id,
                    ImageUrl = product.ImageUrl,
                    IsAvailabe = product.IsAvailabe,
                    LastPurchase = product.LastPurchase,
                    LastSale = product.LastSale,
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock,
                    User = product.User,
                };
            }

            return productViewModel;
        }
    }
}
