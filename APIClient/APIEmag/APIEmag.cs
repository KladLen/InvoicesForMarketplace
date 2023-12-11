using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace InvoicesForMarketplace.APIClient.APIEmag
{
    public class APIEmag : IAPIEmag
    {
        private RestClient restClient;
        public const string BASE_URL_EMAG_PL = "https://marketplace-api.emag.pl/api-3";
        public const string BASE_URL_EMAG_RO = "https://marketplace-api.emag.ro/api-3";
        public const string BASE_URL_EMAG_BG = "https://marketplace-api.emag.bg/api-3";
        public const string BASE_URL_EMAG_HU = "https://marketplace-api.emag.hu/api-3";

        public APIEmag()
        {
            InitializeRestClient(BASE_URL_EMAG_PL);
        }

        private void InitializeRestClient(string baseUrl)
        {
            var options = new RestClientOptions(BASE_URL_EMAG_PL);
            restClient = new RestClient(options);
        }

        public void UpdateBaseUrl(string baseUrl)
        {
            InitializeRestClient(baseUrl);
        }

        public async Task<RestResponse> GetOrdersByStatus(int status)
        {
            var request = new RestRequest(EndpointsEmag.GET_ORDERS, Method.Get);
            request.AddQueryParameter("status", status);
            return await restClient.ExecuteAsync(request);
        }

        public async Task<RestResponse> SaveAttachmentToOrder<T>(T attachment)
        {
            var request = new RestRequest(EndpointsEmag.SAVE_ATTACHMENT, Method.Post);
            request.AddBody(attachment);
            return await restClient.ExecuteAsync(request);
        }

        public async Task<RestResponse> GetProductById(int id)
        {
            var request = new RestRequest(EndpointsEmag.GET_PRODUCT_BY_ID);
            request.AddQueryParameter("id", id);
            return await restClient.ExecuteAsync(request);
        }
    }
}
