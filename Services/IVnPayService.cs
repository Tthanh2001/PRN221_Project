using PRN221_Project.PaymentModels;

namespace PRN221_Project.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(int Price, int Id);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
