namespace ProvaPub.Models
{
	public class CustomerList
	{
        public CustomerList(Pagination pagination, List<Customer> customers)
        {
            Customers = customers;
            Pagination = pagination;
        }
        public List<Customer> Customers { get; set; }
        public Pagination Pagination { get; set; }
    }
}
