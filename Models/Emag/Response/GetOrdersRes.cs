using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace InvoicesForMarketplace.Models.Emag.Response
{
    public class GetOrdersRes
    {
        public List<Order> orders {  get; set; }
    }
    public class Order
    {
        public int id {  get; set; }
        public int status { get; set; }
        public Customer customer { get; set; }
        public List<Product> products { get; set; }
        public List<OrderAttachment> attachments { get; set; }
    }

    public class Customer
    {
        public int id { get; set; }
        public string billing_name { get; set; }
        public string billing_phone { get; set; }
        public string billing_country { get; set; }
        public string billing_city { get; set; }
        public string billing_street { get; set; }
        public string billing_postal_code { get; set; }
    }

    public class Product
    {
        public int product_id { get; set; }
        public string currency { get; set; }
        public int quantity { get; set; }
        public decimal sale_price { get; set; }
    }
    public class OrderAttachment
    {
        public int order_id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string type { get; set; }
    }
}
