using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesForMarketplace.APIClient.APIFakturownia
{
    public class APIFakturownia : IAPIFakturownia
    {
        private readonly RestClient restClient;
        const string BASE_URL_FAKTUROWNIA = $"https://{YOUR_DOMAIN}.fakturownia.pl";
        const string API_TOKEN = "dodaj_token";

        public APIFakturownia()
        {
            var options = new RestClientOptions(BASE_URL_FAKTUROWNIA);
            restClient = new RestClient(options);
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
    }
}
