using System;
using InvoicesForMarketplace.APIClient.APIEmag;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using InvoicesForMarketplace.Models.Emag.Response;
using Newtonsoft.Json;
using Azure;
using InvoicesForMarketplace.APIClient.APIFakturownia;
using InvoicesForMarketplace.Models.Fakturownia.Request;
using InvoicesForMarketplace.Models.Fakturownia.Response;

namespace InvoicesForMarketplace
{
    public class Function
    {
        private readonly ILogger _logger;
        private readonly IAPIEmag _apiEmag;
        private readonly IAPIFakturownia _apiFakturownia;

        public Function(ILoggerFactory loggerFactory, IAPIEmag apiEmag, IAPIFakturownia apiFakturownia)
        {
            _logger = loggerFactory.CreateLogger<Function>();
            _apiEmag = apiEmag;
            _apiFakturownia = apiFakturownia;
        }

        [Function("TriggerFunction")]
        public async Task Run([TimerTrigger("*/20 * * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // Get all orders with status prepared
            var preparedOrders = await _apiEmag.GetOrdersByStatus(3);
            if (preparedOrders.StatusCode == HttpStatusCode.OK)
            {
                GetOrdersRes getOrdersRes = JsonConvert.DeserializeObject<GetOrdersRes>(preparedOrders.Content);

                // Get all orders without an invoice
                List<Order> ordersWithoutInvoice = getOrdersRes.orders
                    .Where(order => order.attachments.All(attachment => attachment.type != "1")).ToList();

                foreach (var order in ordersWithoutInvoice)
                {
                    // Create invoice
                    CreateInvoiceReq createInvoiceReq = new CreateInvoiceReq
                    {
                        api_token = "token ze zmiennych œrodowiskowych?",
                        invoice = new Invoice
                        {
                            kind = "vat",
                            number = null,
                            //sell_date =
                            seller_name = "zmienne œrodowiskowe",
                            buyer_name = order.customer.billing_name,
                            positions = GetPositionsFromOrder(order.products),
                        }
                    };

                    var responseInvoice = await _apiFakturownia.CreateInvoice<CreateInvoiceReq>(createInvoiceReq);
                    if (responseInvoice.StatusCode == HttpStatusCode.Created)
                    {
                        // Get id of created invoice?
                        CreateInvoiceRes createInvoiceRes = JsonConvert.DeserializeObject<CreateInvoiceRes>(responseInvoice.Content);
                        var invoiceId = createInvoiceRes.invoice_id;

                        // Get invoice pdf


                        // Attach invoice to order
                        OrderAttachment orderAttachment = new OrderAttachment
                        {
                            order_id = order.id,
                            name = "Invoice",
                            url = "",
                            type = "1"
                        };
                        var responseOrderAttachment = await _apiEmag.SaveAttachmentToOrder<OrderAttachment>(orderAttachment);
                        _logger.LogInformation($"Order attachment saving status: {responseOrderAttachment.StatusCode}");
                    }
                    else
                    {
                        _logger.LogWarning($"Error whlie creating invoice. Response: {responseInvoice.StatusCode}");
                    }                    
                }
            }
            else
            {
                _logger.LogWarning($"Response: {preparedOrders.StatusCode}");
            }


            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }

        private List<Position> GetPositionsFromOrder(List<Product> product)
        {
            List<Position> positions = new List<Position>();
            foreach (var item in product)
            {
                Position position = new Position
                {
                    total_price_gross = item.sale_price,
                    quantity = item.quantity
                };
                positions.Add(position);
            }
            return positions;
        }
    }
}
