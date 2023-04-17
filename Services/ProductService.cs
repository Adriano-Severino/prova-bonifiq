using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services.Interface;

namespace ProvaPub.Services
{
    public class ProductService : IProductService
    {
        private readonly TestDbContext _ctx;

        public ProductService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        public ProductList ListProducts(int page)
        {
            if (page <= 0)
                page = 1;

            var pageSize = 10;
            var totalCount = _ctx.Products.Count();
            var products = _ctx.Products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var hasNext = (page * pageSize) < totalCount;

            return new ProductList(new Pagination { HasNext = hasNext, TotalCount = totalCount, Page = page }, products);
        }

    }
}
