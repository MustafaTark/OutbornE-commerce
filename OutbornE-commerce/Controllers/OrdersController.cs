using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Orders;
using OutbornE_commerce.BAL.PaymentService;
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
        private readonly UserManager<User> _userManager;
        private readonly IPaymentService _paymentService;
        private readonly ICityRepository _cityRepository;
        public OrdersController(IOrderRepository orderRepository, IFilesManager filesManager, IProductRepository productRepository, UserManager<User> userManager, IPaymentService paymentService, ICityRepository cityRepository)
        {
            _orderRepository = orderRepository;
            _filesManager = filesManager;
            _productRepository = productRepository;
            _userManager = userManager;
            _paymentService = paymentService;
            _cityRepository = cityRepository;
        }
        [HttpPost]
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
        [HttpPut("update-status")]
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

            return await _orderRepository.ExecuteInTransactionAsync(async () =>
            {
                var order = await _orderRepository.Find(c => c.Id == orderId);

                order.Status = status;
                order.UpdatedBy = User.GetUserIdAPI();
                order.UpdatedOn = DateTime.UtcNow;
                _orderRepository.Update(order);

                if (order.Status == (int)(OrderStatus.Shipped))
                {
                    var productUpdates = order.OrderItems.Select(item => new
                    {
                        ProductId = item.ProductId,
                        QuantityToReduce = item.Qunatity
                    }).ToList();

                    foreach (var update in productUpdates)
                    {
                        await _productRepository.ExecuteUpdate(
                            p => p.Id == update.ProductId,
                            p => p.SetProperty(p => p.QuantityInStock, p => p.QuantityInStock - update.QuantityToReduce)
                        );
                    }
                }

                return Ok(new Response<string>
                {
                    Data = "",
                    IsError = false,
                    Message = "",
                    Status = (int)StatusCodeEnum.Ok,
                });
            }, cancellationToken);
        }

        [HttpPost("orderWithPayment")]
        [Authorize]
        public async Task<IActionResult> CreateOrderWithPayment([FromBody] OrderForCreateWithoutImage model, CancellationToken cancellationToken)
        {
            try
            {
                string userId = User.GetUserIdAPI();
                var order = model.Adapt<Order>();
                var productIds = model.OrderItems.Select(x => x.ProductId).ToList();
                var products = await _productRepository.FindByCondition(c => productIds.Contains(c.Id));
                order.OrderItems.Clear();
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
                    if (item.IsWholesale && item.Quantity < product.WholesaleMinmumQuntity)
                    {
                        return BadRequest(new Response<string>
                        {
                            Data = "",
                            IsError = true,
                            Message = "الكمية المطلوبة غير كافية لطلب الجملة ",
                            Status = (int)StatusCodeEnum.BadRequest
                        });
                    }
                    var orderItem = new OrderItem
                    {
                        ProductId = item.ProductId,
                        Qunatity = item.Quantity,
                        Price = item.IsWholesale? product.WholesalePrice: product.PricePerUnit,
                        Total = (item.IsWholesale? product.WholesalePrice : product.PricePerUnit)* item.Quantity,
                        CreatedBy = User.GetUserIdAPI()
                    };
                    order.OrderItems.Add(orderItem);
                }

             
                order.Total = order.OrderItems.Sum(x => x.Total);
                if (model.CityId != null)
                {
                    var city = await _cityRepository.Find(c => c.Id == model.CityId);
                    if (city == null)
                    {
                        return BadRequest(new Response<string>
                        {
                            Data = "",
                            IsError = true,
                            Message = "المدينة غير موجودة",
                            Status = (int)StatusCodeEnum.BadRequest
                        });
                    }
                    order.CityId = model.CityId;
                    order.Total += city.ShippingCost;
                }
                order.TotalAfterDiscount = order.Total;
                order.UserId = userId;
                order.Status = (int)OrderStatus.Placed;
                order.CreatedBy = User.GetUserIdAPI();

                var entity = await _orderRepository.Create(order);
                await _orderRepository.SaveAsync(cancellationToken);

                var user = await _userManager.FindByIdAsync(userId);
                var paymentRequest = new PaymentRequest
                {
                    OrderId = entity.Id,
                    Quantity = 1,
                    OrderNotes = order.Notes,
                    PaymentMethod = "visa",
                    Price = order.TotalAfterDiscount,
                    ProductName = "Order",
                    UserName = user.FullName,
                    ShippingAddress = order.FullAddress != null ? order.FullAddress : "Egypt",
                    UserEmail = user.Email != null ? user.Email : "mostaf01230@gmail.com"
                };
                var paymentUrl = _paymentService.InitiatePayment(paymentRequest);
                return Ok(new Response<dynamic>
                {
                    Data = new
                    { 
                        id = order.Id,
                        paymentUrl = paymentUrl,
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
                    IsError = false,
                    Message = "حدث خطأ",
                    Status = (int)StatusCodeEnum.BadRequest,
                });
            }

        }
    }
 }
