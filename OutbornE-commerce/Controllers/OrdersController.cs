using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Orders;
using OutbornE_commerce.BAL.Repositories.Orders;
using OutbornE_commerce.Extensions;
using OutbornE_commerce.FilesManager;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private IFilesManager _filesManager;
        public OrdersController(IOrderRepository orderRepository, IFilesManager filesManager)
        {
            _orderRepository = orderRepository;
            _filesManager = filesManager;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] OrderForCreateDto model,CancellationToken cancellationToken)
        {
            try
            {
                string userId = User.GetUserIdAPI();
                var order = model.Adapt<Order>();

                if (model.PaymentImage != null)
                {
                    var fileModel = await _filesManager.UploadFile(model.PaymentImage, "Orders");
                    order.PaymentImageUrl = fileModel.Url;
                }
                if (model.PaymentImage2 != null)
                {
                    var fileModel = await _filesManager.UploadFile(model.PaymentImage2, "Orders");
                    order.PaymentImageUrl2 = fileModel.Url;
                }
                if (model.PaymentImage3 != null)
                {
                    var fileModel = await _filesManager.UploadFile(model.PaymentImage3, "Orders");
                    order.PaymentImageUrl3 = fileModel.Url;
                }
                order.UserId = userId;
                await _orderRepository.Create(order);
                await _orderRepository.SaveAsync(cancellationToken);
                return Ok(new Response<Guid>
                {
                    Data = order.Id,
                    IsError = false,
                    Message = "",
                    Status = (int)StatusCodeEnum.Ok
                });
            }catch(Exception ex)
            {
                return BadRequest(new Response<string>
                {
                    Data = ex.InnerException?.Message,
                    IsError =false,
                    Message = "حدث خطأ",
                    Status = (int)StatusCodeEnum.BadRequest,
                });
            }
         
        }





        //private List<IFormFile> GetPaymentImages(OrderForCreateDto model)
        //{
        //    List<IFormFile> models = [model.PaymentImage];
        //    if(model.PaymentImage2 != null) {
        //        models.Add(model.PaymentImage2);
        //    }
        //    if(model.PaymentImage3 != null) {
        //        models.Add(model.PaymentImage3);
        //    }
        //    return models;

        //}
    }
}
