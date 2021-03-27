using System.Text;

namespace AuctionAPI.Application.Extensions {

	internal static class StringExtensions {
		
		///<summary>Escapes %, _, [, ^ and ~ chars with ~</summary>
		public static string EscapeLikeText(this string text) {

			if(!text.Contains('%') && !text.Contains('_') && !text.Contains('[') && !text.Contains('^')) {
				return text;
			}
			StringBuilder builder = new(text.Length);
			foreach(char ch in text) {
				switch(ch) {
					case '%':
					case '_':
					case '[':
					case '^':
					case '~':
						builder.Append('~');
						break;
				}
				builder.Append(ch);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Escapes string with <see cref="EscapeLikeText"/> and wraps it to % chars 
		/// </summary>
		public static string ToLikeString(this string text) {
			string escapedText = text.EscapeLikeText();
			StringBuilder builder = new(escapedText.Length+2);
			builder.Append('%')
				.Append(escapedText)
				.Append('%');
			return builder.ToString();
		}
	}

}