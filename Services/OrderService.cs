using ProvaPub.Models;
using ProvaPub.Services.Interface;
using System.Drawing;

namespace ProvaPub.Services
{
    public class OrderService
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
                Value = paymentValue,
            });
        }
    }
}
