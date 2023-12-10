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
        Task<RestResponse> GetOrdersByStatus(int status);
        Task<RestResponse> SaveAttachmentToOrder<T>(T attachment);
    }
}