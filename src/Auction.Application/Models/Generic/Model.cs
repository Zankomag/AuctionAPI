using Newtonsoft.Json;

// ReSharper disable InconsistentNaming

namespace Auction.Application.Models.Generic {

	public class Model<TKey> {
		[JsonIgnore]
		public TKey Id { get; set; }

		//This kludge is used to prevent Id field be Deserialized when posting input model
		//Id field should appear first, so it should have order -2 (https://stackoverflow.com/a/14035431/11101834, https://www.newtonsoft.com/json/help/html/JsonPropertyOrder.htm)
		[JsonProperty(Order = -2)]
		private TKey id => Id;
	}

}