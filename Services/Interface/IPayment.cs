namespace ProvaPub.Services.Interface
{
    public interface IPayment
    {
        void ProcessPayment(string paymentMethod, decimal paymentValue, int customerId);
    }
}
