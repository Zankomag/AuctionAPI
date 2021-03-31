using System.Text;

namespace AuctionAPI.Web.Authentication {

	public class JwtSettings {

		public string Secret { get; set; }

		private byte[] secretBytes;

		public byte[] SecretBytes => secretBytes ??= Encoding.UTF8.GetBytes(Secret);
		
	}

}