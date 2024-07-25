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
        public T? Data { get; set; }
        public int Status {  get; set; }
        public bool IsError { get; set; }
        public string Message { get; set; }
    }
   
}
