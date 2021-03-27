using System.Text.Json.Serialization;

namespace AuctionAPI.Application.Models.Generic {

	public class Model<TKey> {
		[JsonIgnore]
		public TKey Id { get; set; }
		
		//This kludge is used to prevent Id field be Deserialized when posting input model
		[JsonPropertyName(nameof(Id))]
		private TKey id => Id;
	}

}