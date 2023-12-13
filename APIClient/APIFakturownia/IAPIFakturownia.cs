using RestSharp;

namespace InvoicesForMarketplace.APIClient.APIFakturownia
{
    public interface IAPIFakturownia
    {
        Task<RestResponse> CreateInvoice<T>(T invoice) where T : class;
        Task<RestResponse> GetInvoiceById(string id);
        public string GetPdfUrl(string token);
    }
}