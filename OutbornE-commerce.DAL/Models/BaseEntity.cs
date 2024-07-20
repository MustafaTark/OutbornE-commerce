﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutbornE_commerce.DAL.Models
{
	public class BaseEntity
	{
		public Guid Id { get; set; }

		public string CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		public string? UpdatedBy { get; set; }
		public DateTime? UpdatedOn { get; set; }

		public bool IsDeleted { get; set; }
	}
}
