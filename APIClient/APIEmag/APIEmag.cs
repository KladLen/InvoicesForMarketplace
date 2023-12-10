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
        private readonly RestClient restClient;
        const string BASE_URL_EMAG = "https://marketplace-api.emag.pl/api-3";

        public APIEmag()
        {
            var options = new RestClientOptions(BASE_URL_EMAG);
            restClient = new RestClient(options);
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
    }
}
