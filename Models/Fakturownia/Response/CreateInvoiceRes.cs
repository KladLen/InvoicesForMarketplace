using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesForMarketplace.Models.Fakturownia.Response
{
    public class CreateInvoiceRes
    {
        public string invoice_id {  get; set; }
        public string token { get; set; }
    }
}
