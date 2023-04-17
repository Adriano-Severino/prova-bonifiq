namespace ProvaPub.Models
{
	public class ProductList
	{
        private object customers;

        public ProductList(Pagination pagination, List<Product> products)
        {
            Pagination = pagination;
            Products = products;
        }

        public List<Product> Products { get; set; }
        public Pagination Pagination { get; set; }
    }
}
