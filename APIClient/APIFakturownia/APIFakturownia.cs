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
        const string BASE_URL_FAKTUROWNIA = "https://YOUR_DOMAIN.fakturownia.pl";

        public APIFakturownia()
        {
            var options = new RestClientOptions(BASE_URL_FAKTUROWNIA);
            restClient = new RestClient(options);
        }
        public async Task<RestResponse> CreateInvoice<T>(T invoice) where T : class
        {
            var request = new RestRequest(EndpointsFakturownia.CREATE_INVOICE, Method.Post);
            request.AddBody(invoice);
            return await restClient.ExecuteAsync(request);
        }

        public Task<RestResponse> GetInvoicePdf(string id)
        {
            throw new NotImplementedException();
        }
    }
}
