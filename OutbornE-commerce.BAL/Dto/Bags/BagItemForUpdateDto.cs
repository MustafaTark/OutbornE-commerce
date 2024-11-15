using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Bags
{
    public class BagItemForUpdateDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid? ColorId { get; set; }
        public int Quantity { get; set; }
    }
}
