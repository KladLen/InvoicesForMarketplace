﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesForMarketplace.APIClient.APIFakturownia
{
    public interface IAPIFakturownia
    {
        Task<RestResponse> CreateInvoice<T>(T invoice) where T : class;
        Task<RestResponse> GetInvoicePdf(string id);
    }
}