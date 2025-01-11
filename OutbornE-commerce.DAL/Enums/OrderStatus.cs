using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Enums
{
    public enum OrderStatus
    {
        Placed = 0,
        PlacedWithImages = 1,
        Confirmed = 2,
        Shipped = 3,
        Delivered = 4,
        Canceled = 5,
        Refused = 6
    }
}
