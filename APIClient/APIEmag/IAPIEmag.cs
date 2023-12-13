using RestSharp;

namespace InvoicesForMarketplace.APIClient.APIEmag
{
    public interface IAPIEmag
    {
        public void UpdateBaseUrl(string baseUrl);
        Task<RestResponse> GetOrdersByStatus(int status);
        Task<RestResponse> GetProductById(int id);
        Task<RestResponse> SaveAttachmentToOrder<T>(T attachment);
        public string GetInvoiceLanguageFromUrl(string baseUrl);
    }
}