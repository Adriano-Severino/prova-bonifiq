using ProvaPub.Models;
using ProvaPub.Services.Interface;

namespace ProvaPub.Services
{
    public class OrderService : IOrderService
    {
        private readonly IPayment _payment;
        public OrderService(IPayment payment)
        {
            _payment = payment;
        }
        public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
        {
            //Faz pagamento...
            _payment.ProcessPayment(paymentMethod, paymentValue, customerId);

            return await Task.FromResult(new Order()
            {
                CustomerId = customerId,
                OrderDate = DateTime.Now,
                Value = paymentValue,
                Customer = new Customer()
                {
                    Id = customerId,
                    Name = "Teste",
                    Orders = new List<Order>()
                    {
                        new Order()
                        {
                            CustomerId = customerId,
                            OrderDate = DateTime.Now,
                            Value = paymentValue
                        }
                    }
                }
            });
        }
    }
}
