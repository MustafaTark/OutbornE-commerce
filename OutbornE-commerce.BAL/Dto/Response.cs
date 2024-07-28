using OutbornE_commerce.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto
{
    public class Response<T>
    {
        public int Status { get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
        public string? MessageAr { get; set; }
        public T? Data { get; set; }
        
    }
   
}
