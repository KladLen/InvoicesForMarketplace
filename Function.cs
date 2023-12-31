using InvoicesForMarketplace.APIClient.APIEmag;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net;
using InvoicesForMarketplace.Models.Emag.Response;
using Newtonsoft.Json;
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
        public async Task Run([TimerTrigger("0 */30 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            
            var countryUrls = new List<string>()
            { APIEmag.BASE_URL_EMAG_RO, APIEmag.BASE_URL_EMAG_BG, APIEmag.BASE_URL_EMAG_HU };
  
            foreach (var url in countryUrls)
            {
                _apiEmag.UpdateBaseUrl(url);

                // Get all orders with status prepared
                var getOrdersResponse = await _apiEmag.GetOrdersByStatus(3);
                if (getOrdersResponse.StatusCode == HttpStatusCode.OK)
                {
                    GetOrdersRes getOrdersRes = null;
                    try
                    { 
                        getOrdersRes = JsonConvert.DeserializeObject<GetOrdersRes>(getOrdersResponse.Content);
                    }
                    catch (JsonException e)
                    {
                        _logger.LogError($"Error deserializnig JSON when deserializing Order: {e.Message}");
                    }

                    if (getOrdersRes != null)
                    {
                        // Get all orders without an invoice
                        List<Order> ordersWithoutInvoice = getOrdersRes.orders
                            .Where(order => order.attachments.All(attachment => attachment.type != "1")).ToList();

                        foreach (var order in ordersWithoutInvoice)
                        {
                            // Create invoice
                            CreateInvoiceReq createInvoiceReq = new CreateInvoiceReq
                            {
                                api_token = Environment.GetEnvironmentVariable("FAKTUROWNIA_API_TOKEN"),
                                invoice = new Invoice
                                {
                                    kind = "vat",
                                    number = null,
                                    sell_date = order.date,
                                    issue_date = DateTime.Now.ToString(),
                                    seller_name = Environment.GetEnvironmentVariable("SELLER_NAME"),
                                    seller_tax_no = Environment.GetEnvironmentVariable("SELLER_TAX_NO"),
                                    buyer_name = order.customer.billing_name,
                                    lang = _apiEmag.GetInvoiceLanguageFromUrl(url),
                                    show_discount = order.products.Any(p => p.product_voucher_split != null && p.product_voucher_split.Any()) ? "1" : "0", // checks if product has any voucher
                                    positions = await GetPositionsFromOrderAsync(order.products)
                                }
                            };

                            var createInvoiceResponse = await _apiFakturownia.CreateInvoice<CreateInvoiceReq>(createInvoiceReq);
                            if (createInvoiceResponse.StatusCode == HttpStatusCode.Created)
                            {
                                CreateInvoiceRes createInvoiceRes = null;
                                try
                                {
                                    // Get id of created invoice
                                    createInvoiceRes = JsonConvert.DeserializeObject<CreateInvoiceRes>(createInvoiceResponse.Content);
                                }
                                catch (JsonException e)
                                {
                                    _logger.LogError($"Error deserializnig JSON when deserializing Invoice: {e.Message}");
                                }
                                if (createInvoiceRes != null)
                                {
                                    // Get invoice pdf
                                    string invoiceUrl = _apiFakturownia.GetPdfUrl(createInvoiceRes.token);

                                    // Attach invoice to order
                                    OrderAttachment orderAttachment = new OrderAttachment
                                    {
                                        order_id = order.id,
                                        name = "Invoice",
                                        url = invoiceUrl,
                                        type = "1"
                                    };
                                    var responseOrderAttachment = await _apiEmag.SaveAttachmentToOrder<OrderAttachment>(orderAttachment);
                                    _logger.LogInformation($"Order attachment saving status: {responseOrderAttachment.StatusCode}");
                                }
                                else
                                {
                                    _logger.LogError($"Adding order attachment failed");
                                }
                            }
                            else
                            {
                                _logger.LogError($"Error whlie creating invoice. Response: {createInvoiceResponse.StatusCode}");
                            }
                        }
                    }
                    else
                    {
                        _logger.LogInformation("There was no order with status PREPARED");
                    }
                }
                else
                {
                    _logger.LogError($"Error while reading orders. Response: {getOrdersResponse.StatusCode}");
                }
            }
        }

        // Creating Positions for Invoice (Fakturownia) based on Products from Order (Emag)
        private async Task<List<Position>> GetPositionsFromOrderAsync(List<Product> product)
        {
            List<Position> positions = new List<Position>();
            foreach (var item in product)
            {
                Position position = new Position
                {
                    total_price_gross = item.sale_price,
                    quantity = item.quantity,
                    discount = item.product_voucher_split.Select(voucher => voucher.value).Sum().ToString()
                };

                var getProductResponse = await _apiEmag.GetProductById(item.product_id);
                if (getProductResponse.StatusCode == HttpStatusCode.OK)
                {
                    Product getProductRes = null;
                    try
                    {
                        getProductRes = JsonConvert.DeserializeObject<Product>(getProductResponse.Content);
                    }
                    catch (JsonException e)
                    {
                        _logger.LogError($"Error deserializnig JSON when deserializing Product: {e.Message}");
                    }
                    if (getProductRes != null)
                    {
                        position.name = getProductRes.name;
                    }
                }
                positions.Add(position);
            }
            return positions;
        }
    }
}
