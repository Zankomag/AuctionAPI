using Auction.Application.Models.Generic;

namespace Auction.Application.Models {

	public class ImageFileModel : Model<int> {
		public string FileExtension { get; set; }
		public byte[] File { get; set; }
	}

}