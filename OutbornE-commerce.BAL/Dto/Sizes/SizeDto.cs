using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Sizes
{
    public class SizeDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } // Clothing {S,M,L,Xl,XXL , etc ..} , Shoes {32 , 33 , 34 ,35 , ... 46}
        public Type Type { get; set; }
    }
}
