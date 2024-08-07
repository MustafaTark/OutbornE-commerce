using OutbornE_commerce.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.BAL.Dto.Address
{
    public class AddressDto
    {
        public Guid Id { get; set; }
        public Guid CityId { get; set; }
        public string? Street { get; set; }
        public string? BuildingNumber { get; set; }
        public string? PostalCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool? IsDeafult { get; set; }
        public Guid UserId { get; set; }
    }
}
