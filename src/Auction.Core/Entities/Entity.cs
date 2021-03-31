using System.ComponentModel.DataAnnotations;

namespace Auction.Core.Entities {

	public class Entity<TKey> {
		[Key] public TKey Id { get; set; }
	}

}