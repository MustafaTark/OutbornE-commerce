using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Orders;
using OutbornE_commerce.BAL.Repositories.Orders;
using OutbornE_commerce.BAL.Repositories.Products;
using OutbornE_commerce.Extensions;
using OutbornE_commerce.FilesManager;
using System.Linq.Expressions;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private IFilesManager _filesManager;
        private readonly IProductRepository _productRepository;
        public OrdersController(IOrderRepository orderRepository, IFilesManager filesManager, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _filesManager = filesManager;
            _productRepository = productRepository;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] OrderForCreateDto model, CancellationToken cancellationToken)
        {
            try
            {
                string userId = User.GetUserIdAPI();
                var order = model.Adapt<Order>();
                var productIds = model.OrderItems.Select(x => x.ProductId).ToList();
                var products = await _productRepository.FindByCondition(c => productIds.Contains(c.Id));
                foreach (var item in model.OrderItems)
                {
                    var product = products.FirstOrDefault(x => x.Id == item.ProductId);

                    if (product.QuantityInStock < item.Quantity)
                    {
                        return BadRequest(new Response<string>
                        {
                            Data = "",
                            IsError = true,
                            Message = "الكمية المطلوبة غير متوفرة",
                            Status = (int)StatusCodeEnum.BadRequest
                        });
                    }
                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Qunatity = item.Quantity,
                        Price = product.PricePerUnit,
                        Total = product.PricePerUnit * item.Quantity
                    };
                    order.OrderItems.Add(orderItem);
                }
                order.Total = order.OrderItems.Sum(x => x.Total);
                order.TotalAfterDiscount = order.Total;
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
                order.Status = (int)OrderStatus.PlacedWithImages;
                await _orderRepository.Create(order);
                await _orderRepository.SaveAsync(cancellationToken);
                return Ok(new Response<Guid>
                {
                    Data = order.Id,
                    IsError = false,
                    Message = "",
                    Status = (int)StatusCodeEnum.Ok
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>
                {
                    Data = ex.InnerException?.Message,
                    IsError = false,
                    Message = "حدث خطأ",
                    Status = (int)StatusCodeEnum.BadRequest,
                });
            }

        }

        [HttpPost("withoutPaymentImages")]
        [Authorize]
        public async Task<IActionResult> CreateWithoutPaymentImages([FromForm] OrderForCreateWithoutImage model, CancellationToken cancellationToken)
        {
            try
            {
                string userId = User.GetUserIdAPI();
                var order = model.Adapt<Order>();
                var productIds = model.OrderItems.Select(x => x.ProductId).ToList();
                var products = await _productRepository.FindByCondition(c => productIds.Contains(c.Id));
                foreach (var item in model.OrderItems)
                {
                    var product = products.FirstOrDefault(x => x.Id == item.ProductId);

                    if (product.QuantityInStock < item.Quantity)
                    {
                        return BadRequest(new Response<string>
                        {
                            Data = "",
                            IsError = true,
                            Message = "الكمية المطلوبة غير متوفرة",
                            Status = (int)StatusCodeEnum.BadRequest
                        });
                    }
                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Qunatity = item.Quantity,
                        Price = product.PricePerUnit,
                        Total = product.PricePerUnit * item.Quantity
                    };
                    order.OrderItems.Add(orderItem);
                }
                order.Total = order.OrderItems.Sum(x => x.Total);
                order.TotalAfterDiscount = order.Total;
                order.UserId = userId;
                order.Status = (int)OrderStatus.Placed;
                await _orderRepository.Create(order);
                await _orderRepository.SaveAsync(cancellationToken);
                return Ok(new Response<Guid>
                {
                    Data = order.Id,
                    IsError = false,
                    Message = "",
                    Status = (int)StatusCodeEnum.Ok
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>
                {
                    Data = ex.InnerException?.Message,
                    IsError = false,
                    Message = "حدث خطأ",
                    Status = (int)StatusCodeEnum.BadRequest,
                });
            }

        }
        [HttpPost("addPaymentImages")]
        public async Task<IActionResult> AddPaymentImages([FromForm] OrderImagesDto model, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderRepository.Find(c => c.Id == model.OrderId);
                if (order == null)
                {
                    return BadRequest(new Response<string>
                    {
                        Data = "",
                        IsError = true,
                        Message = "الطلب غير موجود",
                        Status = (int)StatusCodeEnum.NotFound
                    });
                }
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
                _orderRepository.Update(order);
                await _orderRepository.SaveAsync(cancellationToken);
                return Ok(new Response<string>
                {
                    Data = "",
                    IsError = false,
                    Message = "",
                    Status = (int)StatusCodeEnum.Ok
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>
                {
                    Data = ex.InnerException?.Message,
                    IsError = false,
                    Message = "حدث خطأ",
                    Status = (int)StatusCodeEnum.BadRequest,
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, DateTime? from = null, DateTime? to = null, int? status = null)
        {
            try
            {
                Expression<Func<Order, bool>>? filter = null;

                if (from.HasValue || to.HasValue || status.HasValue)
                {
                    filter = o => (!from.HasValue || o.CreatedOn.Date >= from.Value.Date) &&
                                  (!to.HasValue || o.CreatedOn.Date <= to.Value.Date) &&
                                  (!status.HasValue || o.Status == status.Value);
                }

                var items = await _orderRepository.FindAllAsyncByPagination(filter, pageNumber, pageSize,
                                                                        new string[] {
                                                                         "OrderItems.Product"
                                                                        ,"OrderItems.Color",
                                                                        "OrderItems.Size",
                                                                        "User"
                                                                        });

                var orderDtos = items.Data.Select(order => new OrderDto
                {
                    Id = order.Id,
                    Total = order.Total,
                    Discount = order.Discount,
                    CouponCode = order.CouponCode,
                    TotalAfterDiscount = order.TotalAfterDiscount,
                    PaymentImageUrl = order.PaymentImageUrl,
                    PaymentImageUrl2 = order.PaymentImageUrl2,
                    PaymentImageUrl3 = order.PaymentImageUrl3,
                    Longitude = order.Longitude,
                    Latitude = order.Latitude,
                    PhoneNumber = order.PhoneNumber,
                    FullAddress = order.FullAddress,
                    Notes = order.Notes,
                    User = order.User != null ? new UserShowDto
                    {
                        FullName = order.User.FullName,
                        ProfilePicture = order.User.ProfilePicture,
                        Email = order.User.Email,
                        PhoneNumber = order.User.PhoneNumber
                    } : null,
                    Status = order.Status,
                    CreatedOn = order.CreatedOn,
                    OrderItems = order.OrderItems.Select(item => new OrderItemDto
                    {
                        ProductNameEn = item.Product.NameEn,
                        ProductNameAr = item.Product.NameAr,
                        ProductImageUrl = item.Product.ImageUrl,
                        Price = item.Price,
                        Quantity = item.Qunatity,
                        Total = item.Total,
                        ColorNameEn = item.Color?.NameEn,
                        ColorNameAr = item.Color?.NameAr,
                        SizeName = item.Size?.Name,
                    }).ToList()
                }).ToList();

                return Ok(new Response<PagainationModel<List<OrderDto>>>
                {
                    Data = new PagainationModel<List<OrderDto>>
                    {
                        Data = orderDtos,
                        PageNumber = items.PageNumber,
                        PageSize = items.PageSize,
                        TotalCount = items.TotalCount
                    },
                    IsError = false,
                    Message = "",
                    Status = (int)StatusCodeEnum.Ok
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>
                {
                    Data = ex.InnerException?.Message,
                    IsError = true,
                    Message = "حدث خطأ",
                    Status = (int)StatusCodeEnum.BadRequest,
                });
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderRepository.Find(o => o.Id == id, true, new string[] {
                "OrderItems.Product",
                "OrderItems.Color",
                "OrderItems.Size",
                "User"
            });
            if (order == null)
            {
                return Ok(new Response<string>
                {
                    Data = "",
                    IsError = true,
                    Message = "الطلب غير موجود",
                    Status = (int)StatusCodeEnum.NotFound
                });
            }
            var orderDto = new OrderDto
            {
                Id = order.Id,
                Total = order.Total,
                Discount = order.Discount,
                CouponCode = order.CouponCode,
                TotalAfterDiscount = order.TotalAfterDiscount,
                PaymentImageUrl = order.PaymentImageUrl,
                PaymentImageUrl2 = order.PaymentImageUrl2,
                PaymentImageUrl3 = order.PaymentImageUrl3,
                Longitude = order.Longitude,
                Latitude = order.Latitude,
                PhoneNumber = order.PhoneNumber,
                FullAddress = order.FullAddress,
                Notes = order.Notes,
                User = order.User != null ? new UserShowDto
                {
                    FullName = order.User.FullName,
                    ProfilePicture = order.User.ProfilePicture,
                    Email = order.User.Email,
                    PhoneNumber = order.User.PhoneNumber
                } : null,
                Status = order.Status,
                CreatedOn = order.CreatedOn,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductNameEn = item.Product.NameEn,
                    ProductNameAr = item.Product.NameAr,
                    ProductImageUrl = item.Product.ImageUrl,
                    Price = item.Price,
                    Quantity = item.Qunatity,
                    Total = item.Total,
                    ColorNameEn = item.Color?.NameEn,
                    ColorNameAr = item.Color?.NameAr,
                    SizeName = item.Size?.Name,
                }).ToList()
            };
            return Ok(new Response<OrderDto>
            {
                Data = orderDto,
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpPut("updateStatus")]
        public async Task<IActionResult> UpdateOrderStatus([FromQuery] Guid orderId, [FromQuery] int status, CancellationToken cancellationToken)
        {
            if (!(Enum.GetValues<OrderStatus>()).Any(c => (int)c == status))
            {
                return BadRequest(new Response<string>
                {
                    Data = "",
                    IsError = true,
                    Message = "حدث خطأ",
                    Status = (int)StatusCodeEnum.BadRequest,
                });
            }
            var order = await _orderRepository.Find(c => c.Id == orderId);
            if (order == null)
            {
                return BadRequest(new Response<string>
                {
                    Data = "",
                    IsError = true,
                    Message = "الطلب غير موجود",
                    Status = (int)StatusCodeEnum.NotFound
                });
            }
            order.Status = status;
            order.UpdatedBy = User.GetUserIdAPI();
            order.UpdatedOn = DateTime.UtcNow;
            _orderRepository.Update(order);
            await _orderRepository.SaveAsync(cancellationToken);
            return Ok(new Response<string>
            {
                Data = "",
                IsError = false,
                Message = "",
                Status = (int)StatusCodeEnum.Ok,
            });

        }
}
 }
