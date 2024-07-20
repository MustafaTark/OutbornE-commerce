using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
	public class Address : BaseEntity
	{
		public string City { get; set; }
		public string? Street { get; set; }
		public string? BuildingNumber { get; set; }
		public string? PostalCode { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
	}
}
