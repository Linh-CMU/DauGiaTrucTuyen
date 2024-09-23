using BusinessObject.Model;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace CapstoneAuctioneerAPI.Controller
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [Route("api/Payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        /// <summary>
        /// The paypal client
        /// </summary>
        private readonly PaypalClient _paypalClient;
        /// <summary>
        /// The auction service
        /// </summary>
        private readonly AuctionService _auctionService;
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController"/> class.
        /// </summary>
        /// <param name="paypalClient">The paypal client.</param>
        /// <param name="auctionService">The auction service.</param>
        public PaymentController(PaypalClient paypalClient, AuctionService auctionService)
        {
            _paypalClient = paypalClient;
            _auctionService = auctionService;
        }
        /// <summary>
        /// Creates the payment.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="auctionId">The auction identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Error while creating payment: " + ex.Message</exception>
        [HttpPost]
        [Route("createPayment")]
        public async Task<IActionResult> createPayment(CancellationToken cancellationToken, int auctionId)
        {
            var totalInVND = _auctionService.TotalPay(auctionId);
            // Set the currency to USD
            var currency = "USD";
            // Create a unique order code using a timestamp
            var orderCode = "DH" + DateTime.Now.Ticks.ToString();

            try
            {
                var total = totalInVND / 24000;
                // Call the PayPal client's CreateOrder method
                var response = await _paypalClient.CreateOrder(FormatDecimal(total), currency, orderCode);

                // Return the response from PayPal
                return Ok(response);
            }
            catch (Exception ex)
            {
                // Handle any errors and throw the exception with a meaningful message
                throw new Exception("Error while creating payment: " + ex.Message);
            }
        }
        /// <summary>
        /// Captures the paypal order.
        /// </summary>
        /// <param name="orderID">The order identifier.</param>
        /// <param name="auctionId">The auction identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPost("/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, int auctionId, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);
                var payment = new Payment
                {
                    PaymentType = "paypal"
                };
                var a = await _auctionService.CheckPayMent(payment, auctionId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        /// <summary>
        /// Formats the decimal.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private string FormatDecimal(decimal value)
        {
            return value.ToString("F2", CultureInfo.InvariantCulture);
        }

    }
}
