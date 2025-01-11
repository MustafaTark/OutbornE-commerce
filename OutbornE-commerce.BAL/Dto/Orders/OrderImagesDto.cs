using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Orders
{
    public class OrderImagesDto
    {
        public Guid OrderId   { get; set; }
       // public string? PaymentImageUrl { get; set; }
        public IFormFile PaymentImage { get; set; }
       // public string? PaymentImageUrl2 { get; }
        public IFormFile? PaymentImage2 { get; set; }
      //  public string? PaymentImageUrl3 { get; }
        public IFormFile? PaymentImage3 { get; set; }
    }
}
