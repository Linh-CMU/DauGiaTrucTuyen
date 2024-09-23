using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccess.Service
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PaypalClient
    {
        /// <summary>
        /// Gets the mode.
        /// </summary>
        /// <value>
        /// The mode.
        /// </value>
        public string Mode { get; }
        /// <summary>
        /// Gets the client identifier.
        /// </summary>
        /// <value>
        /// The client identifier.
        /// </value>
        public string ClientId { get; }
        /// <summary>
        /// Gets the client secret.
        /// </summary>
        /// <value>
        /// The client secret.
        /// </value>
        public string ClientSecret { get; }

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <value>
        /// The base URL.
        /// </value>
        public string BaseUrl => Mode == "Live"
            ? "https://api-m.paypal.com"
            : "https://api-m.sandbox.paypal.com";

        /// <summary>
        /// Initializes a new instance of the <see cref="PaypalClient"/> class.
        /// </summary>
        /// <param name="clientId">The client identifier.</param>
        /// <param name="clientSecret">The client secret.</param>
        /// <param name="mode">The mode.</param>
        public PaypalClient(string clientId, string clientSecret, string mode)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            Mode = mode;
        }

        /// <summary>
        /// Authenticates this instance.
        /// </summary>
        /// <returns></returns>
        private async Task<AuthResponse> Authenticate()
        {
            var auth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{ClientId}:{ClientSecret}"));

            var content = new List<KeyValuePair<string, string>>
            {
                new("grant_type", "client_credentials")
            };

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri($"{BaseUrl}/v1/oauth2/token"),
                Method = HttpMethod.Post,
                Headers =
                {
                    { "Authorization", $"Basic {auth}" }
                },
                Content = new FormUrlEncodedContent(content)
            };

            var httpClient = new HttpClient();
            var httpResponse = await httpClient.SendAsync(request);
            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<AuthResponse>(jsonResponse);

            return response;
        }

        /// <summary>
        /// Creates the order.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="currency">The currency.</param>
        /// <param name="reference">The reference.</param>
        /// <returns></returns>
        public async Task<CreateOrderResponse> CreateOrder(string value, string currency, string reference)
        {
            var auth = await Authenticate();

            var request = new CreateOrderRequest
            {
                intent = "CAPTURE",
                purchase_units = new List<PurchaseUnit>
                {
                    new()
                    {
                        reference_id = reference,
                        amount = new Amount
                        {
                            currency_code = currency,
                            value = value
                        }
                    }
                }
            };

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth.access_token}");

            var httpResponse = await httpClient.PostAsJsonAsync($"{BaseUrl}/v2/checkout/orders", request);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<CreateOrderResponse>(jsonResponse);

            return response;
        }

        /// <summary>
        /// Captures the order.
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        /// <returns></returns>
        public async Task<CaptureOrderResponse> CaptureOrder(string orderId)
        {
            var auth = await Authenticate();

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse($"Bearer {auth.access_token}");

            var httpContent = new StringContent("", Encoding.Default, "application/json");

            var httpResponse = await httpClient.PostAsync($"{BaseUrl}/v2/checkout/orders/{orderId}/capture", httpContent);

            var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<CaptureOrderResponse>(jsonResponse);

            return response;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class AuthResponse
    {
        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        public string scope { get; set; }
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        public string access_token { get; set; }
        /// <summary>
        /// Gets or sets the type of the token.
        /// </summary>
        /// <value>
        /// The type of the token.
        /// </value>
        public string token_type { get; set; }
        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        public string app_id { get; set; }
        /// <summary>
        /// Gets or sets the expires in.
        /// </summary>
        /// <value>
        /// The expires in.
        /// </value>
        public int expires_in { get; set; }
        /// <summary>
        /// Gets or sets the nonce.
        /// </summary>
        /// <value>
        /// The nonce.
        /// </value>
        public string nonce { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class CreateOrderRequest
    {
        /// <summary>
        /// Gets or sets the intent.
        /// </summary>
        /// <value>
        /// The intent.
        /// </value>
        public string intent { get; set; }
        /// <summary>
        /// Gets or sets the purchase units.
        /// </summary>
        /// <value>
        /// The purchase units.
        /// </value>
        public List<PurchaseUnit> purchase_units { get; set; } = new();
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class CreateOrderResponse
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string id { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string status { get; set; }
        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public List<Link> links { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class CaptureOrderResponse
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string id { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string status { get; set; }
        /// <summary>
        /// Gets or sets the payment source.
        /// </summary>
        /// <value>
        /// The payment source.
        /// </value>
        public PaymentSource payment_source { get; set; }
        /// <summary>
        /// Gets or sets the purchase units.
        /// </summary>
        /// <value>
        /// The purchase units.
        /// </value>
        public List<PurchaseUnit> purchase_units { get; set; }
        /// <summary>
        /// Gets or sets the payer.
        /// </summary>
        /// <value>
        /// The payer.
        /// </value>
        public Payer payer { get; set; }
        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public List<Link> links { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PurchaseUnit
    {
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public Amount amount { get; set; }
        /// <summary>
        /// Gets or sets the reference identifier.
        /// </summary>
        /// <value>
        /// The reference identifier.
        /// </value>
        public string reference_id { get; set; }
        /// <summary>
        /// Gets or sets the shipping.
        /// </summary>
        /// <value>
        /// The shipping.
        /// </value>
        public Shipping shipping { get; set; }
        /// <summary>
        /// Gets or sets the payments.
        /// </summary>
        /// <value>
        /// The payments.
        /// </value>
        public Payments payments { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class Payments
    {
        /// <summary>
        /// Gets or sets the captures.
        /// </summary>
        /// <value>
        /// The captures.
        /// </value>
        public List<Capture> captures { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class Shipping
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public Address address { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Capture
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string id { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string status { get; set; }
        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        public Amount amount { get; set; }
        /// <summary>
        /// Gets or sets the seller protection.
        /// </summary>
        /// <value>
        /// The seller protection.
        /// </value>
        public SellerProtection seller_protection { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [final capture].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [final capture]; otherwise, <c>false</c>.
        /// </value>
        public bool final_capture { get; set; }
        /// <summary>
        /// Gets or sets the disbursement mode.
        /// </summary>
        /// <value>
        /// The disbursement mode.
        /// </value>
        public string disbursement_mode { get; set; }
        /// <summary>
        /// Gets or sets the seller receivable breakdown.
        /// </summary>
        /// <value>
        /// The seller receivable breakdown.
        /// </value>
        public SellerReceivableBreakdown seller_receivable_breakdown { get; set; }
        /// <summary>
        /// Gets or sets the create time.
        /// </summary>
        /// <value>
        /// The create time.
        /// </value>
        public DateTime create_time { get; set; }
        /// <summary>
        /// Gets or sets the update time.
        /// </summary>
        /// <value>
        /// The update time.
        /// </value>
        public DateTime update_time { get; set; }
        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public List<Link> links { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Amount
    {
        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        /// <value>
        /// The currency code.
        /// </value>
        public string currency_code { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class Link
    {
        /// <summary>
        /// Gets or sets the href.
        /// </summary>
        /// <value>
        /// The href.
        /// </value>
        public string href { get; set; }
        /// <summary>
        /// Gets or sets the relative.
        /// </summary>
        /// <value>
        /// The relative.
        /// </value>
        public string rel { get; set; }
        /// <summary>
        /// Gets or sets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public string method { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class Name
    {
        /// <summary>
        /// Gets or sets the name of the given.
        /// </summary>
        /// <value>
        /// The name of the given.
        /// </value>
        public string given_name { get; set; }
        /// <summary>
        /// Gets or sets the surname.
        /// </summary>
        /// <value>
        /// The surname.
        /// </value>
        public string surname { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SellerProtection
    {
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public string status { get; set; }
        /// <summary>
        /// Gets or sets the dispute categories.
        /// </summary>
        /// <value>
        /// The dispute categories.
        /// </value>
        public List<string> dispute_categories { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class SellerReceivableBreakdown
    {
        /// <summary>
        /// Gets or sets the gross amount.
        /// </summary>
        /// <value>
        /// The gross amount.
        /// </value>
        public Amount gross_amount { get; set; }
        /// <summary>
        /// Gets or sets the paypal fee.
        /// </summary>
        /// <value>
        /// The paypal fee.
        /// </value>
        public PaypalFee paypal_fee { get; set; }
        /// <summary>
        /// Gets or sets the net amount.
        /// </summary>
        /// <value>
        /// The net amount.
        /// </value>
        public Amount net_amount { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class Paypal
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public Name name { get; set; }
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public string email_address { get; set; }
        /// <summary>
        /// Gets or sets the account identifier.
        /// </summary>
        /// <value>
        /// The account identifier.
        /// </value>
        public string account_id { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PaypalFee
    {
        /// <summary>
        /// Gets or sets the currency code.
        /// </summary>
        /// <value>
        /// The currency code.
        /// </value>
        public string currency_code { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string value { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Address
    {
        /// <summary>
        /// Gets or sets the address line 1.
        /// </summary>
        /// <value>
        /// The address line 1.
        /// </value>
        public string address_line_1 { get; set; }
        /// <summary>
        /// Gets or sets the address line 2.
        /// </summary>
        /// <value>
        /// The address line 2.
        /// </value>
        public string address_line_2 { get; set; }
        /// <summary>
        /// Gets or sets the admin area 2.
        /// </summary>
        /// <value>
        /// The admin area 2.
        /// </value>
        public string admin_area_2 { get; set; }
        /// <summary>
        /// Gets or sets the admin area 1.
        /// </summary>
        /// <value>
        /// The admin area 1.
        /// </value>
        public string admin_area_1 { get; set; }
        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        /// <value>
        /// The postal code.
        /// </value>
        public string postal_code { get; set; }
        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        /// <value>
        /// The country code.
        /// </value>
        public string country_code { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class Payer
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public Name name { get; set; }
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        /// <value>
        /// The email address.
        /// </value>
        public string email_address { get; set; }
        /// <summary>
        /// Gets or sets the payer identifier.
        /// </summary>
        /// <value>
        /// The payer identifier.
        /// </value>
        public string payer_id { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PaymentSource
    {
        /// <summary>
        /// Gets or sets the paypal.
        /// </summary>
        /// <value>
        /// The paypal.
        /// </value>
        public Paypal paypal { get; set; }
    }

}

