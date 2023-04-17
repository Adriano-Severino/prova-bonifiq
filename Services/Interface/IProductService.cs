using ProvaPub.Models;

namespace ProvaPub.Services.Interface
{
    public interface IProductService
    {
        public ProductList ListProducts(int page);
    }
}
