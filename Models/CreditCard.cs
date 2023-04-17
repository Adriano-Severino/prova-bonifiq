using ProvaPub.Services.Interface;

namespace ProvaPub.Models
{
    public class CreditCard : IPayment
    {
        public void ProcessPayment(string paymentMethod, decimal paymentValue, int customerId)
        {
            //Implementação para processar pagamento com boleto
        }
    }
}
