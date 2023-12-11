using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesForMarketplace.APIClient.APIEmag
{
    public interface IAPIEmag
    {
        public void UpdateBaseUrl(string baseUrl);
        Task<RestResponse> GetOrdersByStatus(int status);
        Task<RestResponse> GetProductById(int id);
        Task<RestResponse> SaveAttachmentToOrder<T>(T attachment);
    }
}