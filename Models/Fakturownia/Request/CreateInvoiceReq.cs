namespace InvoicesForMarketplace.Models.Fakturownia.Request
{
    public class CreateInvoiceReq
    {
        public string api_token { get; set; }
        public Invoice invoice { get; set; }
    }
    public class Invoice
    {
        public string kind { get; set; }
        public object number { get; set; }
        public string sell_date { get; set; }
        public string issue_date { get; set; }
        public string seller_name { get; set; }
        public string seller_tax_no { get; set; }
        public string buyer_name { get; set; }
        public string lang { get; set; }
        public string show_discount { get; set; }
        public List<Position> positions { get; set; }
    }

    public class Position
    {
        public string name { get; set; }
        public string discount { get; set; }
        public decimal total_price_gross { get; set; }
        public int quantity { get; set; }
    }
}
