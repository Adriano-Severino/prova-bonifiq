using ProvaPub.Models;

namespace ProvaPub.Services.Interface
{
    public interface ICustomerService
    {
        public CustomerList ListCustomers(int page);
        public Task<bool> CanPurchase(int customerId, decimal purchaseValue);
    }
}
