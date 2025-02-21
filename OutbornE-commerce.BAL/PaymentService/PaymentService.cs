using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OutbornE_commerce.BAL.Dto;
using OutbornE_commerce.BAL.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _config;
        private readonly string _merchantId;
        private readonly string _apiKey;
        private readonly string _redirectUrl;
        private readonly string _failureUrl;

        public PaymentService(IConfiguration config)
        {
            _config = config;
            _merchantId = _config["Kashier:MerchantId"]!;
            _apiKey = _config["Kashier:ApiKey"]!;
            _redirectUrl = _config["Kashier:RedirectUrl"]!;
            _failureUrl = _config["Kashier:FailureUrl"]!;
        }
        public string InitiatePayment(PaymentRequest request)
        {
            var orderId = request.OrderId != null? request.OrderId.Value.ToString(): Guid.NewGuid().ToString();
            var currency = "EGP";
            var totalAmount = request.Price * request.Quantity;

            var path = $"/?payment={_merchantId}.{orderId}.{totalAmount}.{currency}";
            var hash = GenerateKashierHash(path, _apiKey);

            var paymentUrl = new UriBuilder("https://checkout.kashier.io/")
            {
                Query = new QueryStringBuilder()
                    .Add("merchantId", _merchantId)
                    .Add("mode", "test")
                    .Add("orderId", orderId)
                    .Add("amount", totalAmount.ToString(CultureInfo.InvariantCulture))
                    .Add("currency", currency)
                    .Add("hash", hash)
                    .Add("allowedMethods", "card,wallet,fawry")
                    .Add("merchantRedirect", (_redirectUrl + "Payments/success" + $"?orderId={orderId}"))
                    .Add("failureRedirect", _failureUrl + "Payments/fail" + $"?orderId={orderId}")
                    .Add("redirectMethod", "get")
                    .Add("brandColor", "%2300bcbc")
                    .Add("display", "ar")
                    .ToString()
            }.ToString();

            return paymentUrl;
        }

        private static string GenerateKashierHash(string path, string apiKey)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(apiKey));
            var hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(path));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
