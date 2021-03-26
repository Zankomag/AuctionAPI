using System.ComponentModel.DataAnnotations;

namespace AuctionAPI.Core.Entities {

	public class Entity<TKey> {
		[Key] public TKey Id { get; set; }
	}

}