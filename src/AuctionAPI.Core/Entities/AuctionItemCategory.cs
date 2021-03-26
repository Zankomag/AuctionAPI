using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionAPI.Core.Entities {
	public class AuctionItemCategory : Entity<int> {
		[Required]
		[StringLength(30)]
		public string Name { get; set; }
	}
}
