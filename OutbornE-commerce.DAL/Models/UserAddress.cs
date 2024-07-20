using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
	public class UserAddress : BaseEntity
	{
		public string UserId { get; set; }
		public User? User { get; set; }
		public Guid AddressId { get; set; }
		public Address? Address { get; set; }
		public bool IsDefault { get; set; }
	}
}
