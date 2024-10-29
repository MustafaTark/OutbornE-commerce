//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity.UI.Services;
//using Microsoft.AspNetCore.Mvc;
//using OutbornE_commerce.BAL.Dto.Newsletters;
//using OutbornE_commerce.BAL.EmailServices;
//using OutbornE_commerce.BAL.Repositories.Newsletters;
//using OutbornE_commerce.BAL.Repositories.NewsletterSubscribers;
//using OutbornE_commerce.BAL.Repositories.SMTP_Server;
//using OutbornE_commerce.DAL.Models;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace OutbornE_commerce.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class NewslettersController : ControllerBase
//    {
//        private readonly INewsletterRepository _newsletterRepository;
//        private readonly INewsletterSubscriberRepository _newsletterSubscriberRepository;
//        private readonly ISMTPRepository _SMTPRepository;
//        private readonly IEmailSenderCustom _emailSender;

//        public NewslettersController(
//            INewsletterRepository newsletterRepository,
//            INewsletterSubscriberRepository newsletterSubscriberRepository,
//            ISMTPRepository sMTPRepository,
//            IEmailSenderCustom emailSender)
//        {
//            _newsletterRepository = newsletterRepository;
//            _newsletterSubscriberRepository = newsletterSubscriberRepository;
//            _SMTPRepository = sMTPRepository;
//            _emailSender = emailSender;
//        }
//        [HttpPost("subscribe")]
//        public async Task<IActionResult> Subscribe(string Email, CancellationToken cancellationToken)
//        {
//            var newsletterSubscriber = new NewsletterSubscriber
//            {
//                CreatedBy = "User",
//                Email = Email,
//                CreatedOn = DateTime.UtcNow,
//            };

//            await _newsletterSubscriberRepository.Create(newsletterSubscriber);
//            await _newsletterSubscriberRepository.SaveAsync(cancellationToken);

//            return Ok(new Response<string>
//            {
//                Data = Email,
//                IsError = false,
//                Status = (int)StatusCodeEnum.Ok
//            });
//        }
//        [HttpGet]
//        public async Task<IActionResult> GetAllNewsletters(int pageNumber = 1,int pageSize = 10,string? searchTerm= null)
//        {
//            var items = new PagainationModel<IEnumerable<Newsletter>>();

//            if (string.IsNullOrEmpty(searchTerm))
//                items = await _newsletterRepository.FindAllAsyncByPagination(null, pageNumber, pageSize);
//            else
//                items = await _newsletterRepository
//                                    .FindAllAsyncByPagination(b => (b.Subject.Contains(searchTerm)
//                                                               || b.Body.Contains(searchTerm))
//                                                               , pageNumber, pageSize);
//            var data = items.Data.Adapt<List<NewsletterDto>>();

//            return Ok(new PaginationResponse<List<NewsletterDto>>
//            {
//                Data = data,
//                IsError = false,
//                Status = (int)StatusCodeEnum.Ok,
//                PageNumber = pageNumber,
//                PageSize = pageSize,
//                TotalCount = items.TotalCount
//            });
//        }
//        [HttpPost]
//        public async Task<IActionResult> CreateNewsletter([FromBody] NewsletterDto model,CancellationToken cancellationToken)
//        {
//           var newsLetter = model.Adapt<Newsletter>();
//            newsLetter.CreatedBy = "admin";

//            await _newsletterRepository.Create(newsLetter);
//            await _newsletterRepository.SaveAsync(cancellationToken);

//            var subscribers = await _newsletterSubscriberRepository.FindAllAsync(null);
//            await _emailSender.SendEmailToListAsync(subscribers.Select(s => s.Email).ToList(),newsLetter.Subject,newsLetter.Body);

//            return Ok(new Response<Guid>
//            {
//                Data = newsLetter.Id,
//                IsError = false,
//                Status = (int)StatusCodeEnum.Ok
//            });
//        }
//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(Guid id)
//        {
//            var newsletter = await _newsletterRepository.Find(n => n.Id == id);
//            var data = newsletter.Adapt<NewsletterDto>();
//            return Ok(new Response<NewsletterDto>
//            {
//                Data = data,
//                IsError = false,
//                Status = (int)StatusCodeEnum.Ok
//            });
//        }
        
//    }
//}
