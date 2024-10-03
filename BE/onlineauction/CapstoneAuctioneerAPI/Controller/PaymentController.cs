﻿using Azure;
using BusinessObject.Model;
using DataAccess.DTO;
using DataAccess.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        /// The user service
        /// </summary>
        private readonly UserService _userService;
        private readonly PayOS _payOS;
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentController" /> class.
        /// </summary>
        /// <param name="paypalClient">The paypal client.</param>
        /// <param name="auctionService">The auction service.</param>
        /// <param name="userService">The user service.</param>
        public PaymentController(PaypalClient paypalClient, AuctionService auctionService, UserService userService, PayOS payOS)
        {
            _paypalClient = paypalClient;
            _auctionService = auctionService;
            _userService = userService;
            _payOS = payOS;
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
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var payment = await _auctionService.TotalPay(auctionId, userId);
                ItemData item = new ItemData(payment.nameAuction, 1, payment.priceAuction);
                List<ItemData> items = new List<ItemData>();
                items.Add(item);
                PaymentData paymentData = new PaymentData(payment.IdResgiter, payment.priceAuction, "Thanh Toán", items, "https://localhost:3000/cancel", "https://localhost:3000/success");
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                return Ok(createPayment);
            }
            catch (Exception ex)
            {
                throw new Exception("Error while creating payment: " + ex.Message);
            }
        }
        [HttpPost]
        [Route("createPaymentDeposit")]
        [Authorize]
        public async Task<IActionResult> createPaymentDeposit(int auctionId)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var deposit = await _userService.TotalPayDeposit(auctionId, userId);
                ItemData item = new ItemData(deposit.nameAuction, 1, deposit.priceAuction);
                List<ItemData> items = new List<ItemData>();
                items.Add(item);
                PaymentData paymentData = new PaymentData(deposit.IdResgiter, deposit.priceAuction, "Tiền Cọc", items, "https://localhost:3000/cancel", "https://localhost:3000/success");
                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);
                return Ok(createPayment.checkoutUrl);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        [HttpGet]
        [Route("checkorder")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);
                return Ok(paymentLinkInformation);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }

        }
        [HttpPut]
        [Route("cancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.cancelPaymentLink(orderId);
                return Ok(paymentLinkInformation);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }

        }
        [HttpPost("confirm-webhook")]
        public async Task<IActionResult> ConfirmWebhook(ConfirmWebhook body)
        {
            try
            {
                await _payOS.confirmWebhook(body.webhook_url);
                return Ok("Successfully");
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }

        }

        [HttpPost("/capture-paypal-deposit")]
        [Authorize]
        public async Task<IActionResult> CapturePaypalDeposit(string orderID, int auctionId, CancellationToken cancellationToken)
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _paypalClient.CaptureOrder(orderID);
                var result = await _userService.UserRegisterAuction(userId, auctionId);
                var id = await _userService.getIdRegisterAuction(auctionId);
                var deposit = new Deposit
                {
                    DID = orderID,
                    RAID = id,
                    PaymentType = "Deposit",
                    PaymentDate = DateTime.Now.ToString(),
                    status = "paid"
                };
                var pay = await _userService.PaymentForDeposit(deposit);
                if (pay != true)
                {
                    return BadRequest(StatusCodes.Status500InternalServerError);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }
        /// <summary>
        /// Refunds a completed PayPal order.
        /// </summary>
        /// <param name="orderID">The PayPal order ID.</param>
        /// <param name="auctionId">The auction ID related to the order.</param>
        /// <returns></returns>
        [HttpPost("/refund-paypal-order")]
        [Authorize]
        public async Task<IActionResult> RefundPaypalOrder()
        {
            try
            {
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var refundResponse = await _paypalClient.RefundOrder("55V60077YF134424F");

                return Ok(refundResponse);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.GetBaseException().Message });
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
