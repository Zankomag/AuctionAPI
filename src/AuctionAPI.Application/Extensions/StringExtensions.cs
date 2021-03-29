using System;
using System.Security.Cryptography;
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

		/// <summary>
		/// Creates PBKDF2 Password hash.
		/// <see href="https://stackoverflow.com/a/10402129/11101834">More info</see>
		/// </summary>
		public static string ToPasswordHash(this string password, out byte[] salt) {
			const int iterations = 40000; //100000
			const int saltLength = 32;
			const int passwordHashLength = 20;
			const int totalHashLength = saltLength + passwordHashLength;

			salt = new byte[saltLength];
			new RNGCryptoServiceProvider().GetBytes(salt);

			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
			byte[] hash = pbkdf2.GetBytes(passwordHashLength);

			byte[] hashBytes = new byte[totalHashLength];
			Array.Copy(salt, 0, hashBytes, 0, saltLength);
			Array.Copy(hash, 0, hashBytes, saltLength, passwordHashLength);

			string passwordHash = Convert.ToBase64String(hashBytes);
			return passwordHash;
		}
	}

}