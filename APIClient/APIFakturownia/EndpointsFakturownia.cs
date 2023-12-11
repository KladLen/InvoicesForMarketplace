using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesForMarketplace.APIClient.APIFakturownia
{
    public class EndpointsFakturownia
    {
        public static readonly string CREATE_INVOICE = "/invoices.json";
        public static readonly string GET_INVOICE_BY_ID = "/invoices/{id}.json";
    }
}