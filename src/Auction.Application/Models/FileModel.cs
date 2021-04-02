using Auction.Application.Models.Generic;

namespace Auction.Application.Models {

	public class FileModel : Model<int> {

		public int? FileSize { get; set; }
	}

}