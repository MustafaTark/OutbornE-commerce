using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OutbornE_commerce.BAL.Dto.Bags;
using OutbornE_commerce.BAL.Repositories.BagItems;
using OutbornE_commerce.Extensions;

namespace OutbornE_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BagController : ControllerBase
    {
        private readonly IBagItemRepository _bagItemRepository;

        public BagController(IBagItemRepository bagItemRepository)
        {
            _bagItemRepository = bagItemRepository;
        }
        [HttpPost]
        public async Task<IActionResult> Add(BagItemForCreateDto model,CancellationToken cancellationToken)
        {
            var bag = model.Adapt<BagItem>();
            bag.UserId = User.GetUserIdAPI();
            var result = await _bagItemRepository.Create(bag);
            await _bagItemRepository.SaveAsync(cancellationToken);

            return Ok(new Response<Guid>
            {
                Data = result.Id,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok
            });
        }
        [HttpPut]
        public async Task<IActionResult> Update(BagItemForUpdateDto model)
        {
            var item = await _bagItemRepository.Find(c=>c.Id == model.Id);
            item = model.Adapt<BagItem>();
            item.UserId = User.GetUserIdAPI();
            _bagItemRepository.Update(item);
            return Ok(new Response<Guid>
            {
                Data = item.Id,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,

            });
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string userId = User.GetUserIdAPI();
            var items = await _bagItemRepository.FindByCondition(b=>b.UserId == userId,new [] {"Product","Color"});
            var data = items.Adapt<List<BagItemDtoAPI>>();
            return Ok(new Response<List<BagItemDtoAPI>>
            {
                Data = data,
                IsError = false,
                Status = (int)StatusCodeEnum.Ok,

            });
        }

    }
}
