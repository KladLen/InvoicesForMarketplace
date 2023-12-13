using RestSharp;

namespace InvoicesForMarketplace.APIClient.APIFakturownia
{
    public class APIFakturownia : IAPIFakturownia
    {
        private readonly RestClient restClient;
        private readonly string BASE_URL_FAKTUROWNIA;
        private readonly string API_TOKEN;

        public APIFakturownia()
        {
            BASE_URL_FAKTUROWNIA = $"https://{Environment.GetEnvironmentVariable("FAKTUROWNIA_DOMAIN")}.fakturownia.pl";
            var options = new RestClientOptions(BASE_URL_FAKTUROWNIA);
            restClient = new RestClient(options);
            API_TOKEN = Environment.GetEnvironmentVariable("API_TOKEN");
        }
        public async Task<RestResponse> CreateInvoice<T>(T invoice) where T : class
        {
            var request = new RestRequest(EndpointsFakturownia.CREATE_INVOICE, Method.Post);
            request.AddQueryParameter("api_token", API_TOKEN);
            request.AddBody(invoice);
            return await restClient.ExecuteAsync(request);
        }

        public async Task<RestResponse> GetInvoiceById(string id)
        {
            var request = new RestRequest(EndpointsFakturownia.GET_INVOICE_BY_ID, Method.Get);
            request.AddUrlSegment("id", id);
            request.AddQueryParameter("api_token", API_TOKEN);
            return await restClient.ExecuteAsync(request);
        }

        public string GetPdfUrl(string token)
        {
            return BASE_URL_FAKTUROWNIA + "/invoice/" + token + ".pdf";
        }
    }
}