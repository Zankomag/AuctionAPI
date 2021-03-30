using System.Text;

namespace AuctionAPI.Web.Authentication {

	public class JwtSettings {

		public string Secret { get; set; }

		public byte[] SecretBytes { get; }

		public JwtSettings() => SecretBytes = Encoding.UTF8.GetBytes(Secret);
	}

}