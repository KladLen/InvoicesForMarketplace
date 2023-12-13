# Marketplace API / Online invoicing API

## Description
This Azure Function is a TimerTrigger function that integrates with the marketplace API to automatically handle orders with a "prepared" status every 30 minutes. The function generates an invoice using an API for online invoicing and then attaches that invoice as a PDF document to the corresponding order in the marketplace.

### Environment Variables
To run the function, the following environment variables must be provided:

- **EMAG_USERNAME**: The username for authentication in the marketplace API.
- **EMAG_PASSWORD**: The password for authentication in the marketplace API.
- **FAKTUROWNIA_DOMAIN**: The domain name for authentication in the API for online invoicing.
- **FAKTUROWNIA_API_TOKEN**: The token for authentication in the API for online invoicing.
- **SELLER_NAME**: The seller's name, which will appear on the invoice.
- **SELLER_TAX_NO**: The seller's tax identification number.

### Installation
Before running, install the required libraries using the following command:

```bash
dotnet add package RestSharp
dotnet add package Newtonsoft.Json