using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.PaymentService;
using OutbornE_commerce.BAL.Repositories.Orders;
using OutbornE_commerce.BAL.Repositories.Products;
using OutbornE_commerce.DAL.Models;
using OutbornE_commerce.Extensions;
using System.Threading;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        public PaymentsController(IPaymentService paymentService, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _paymentService = paymentService;
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }
        [HttpPost]
        public IActionResult InitiatePayment([FromBody] PaymentRequest request)
        {
            var paymentUrl = _paymentService.InitiatePayment(request);
            return Ok(new Response<string>
            {
                Data = paymentUrl,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,
            });
        }

        [HttpGet("success")]
        public async Task<IActionResult> GetSuccess(string orderId,CancellationToken cancellationToken)
        {
           var res =  await _orderRepository.ExecuteInTransactionAsync(async () =>
            {
                var order = await _orderRepository.Find(c => c.Id == new Guid(orderId));

                order.Status = (int) OrderStatus.Confirmed;
                order.UpdatedBy = "kashir";
                order.UpdatedOn = DateTime.UtcNow;
                _orderRepository.Update(order);

                return Ok(new Response<string>
                {
                    Data = "",
                    IsError = false,
                    Message = "",
                    Status = (int)StatusCodeEnum.Ok,
                });
            }, cancellationToken);

            return res;
        }
        [HttpGet("fail")]
        public async Task<IActionResult> GetFail(string orderId, CancellationToken cancellationToken)
        {
            var res = await _orderRepository.ExecuteInTransactionAsync(async () =>
            {
                var order = await _orderRepository.Find(c => c.Id == new Guid(orderId));
                order.Status = (int)OrderStatus.Failed;
                order.UpdatedBy = User.GetUserIdAPI();
                order.UpdatedOn = DateTime.UtcNow;
                _orderRepository.Update(order);
                return Ok(new Response<string>
                {
                    Data = "",
                    IsError = false,
                    Message = "",
                    Status = (int)StatusCodeEnum.Ok,
                });
            }, cancellationToken);
            return res;
        }

    }
}
