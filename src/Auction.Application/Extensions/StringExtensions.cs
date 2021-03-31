using System;
using System.Security.Cryptography;
using System.Text;
using static Auction.Application.Extensions.StringExtensions.PasswordHashingConstants;

namespace Auction.Application.Extensions {

	internal static class StringExtensions {

		///
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
		///     Escapes string with <see cref="EscapeLikeText" /> and wraps it to % chars
		/// </summary>
		public static string ToLikeString(this string text) {
			string escapedText = text.EscapeLikeText();
			StringBuilder builder = new(escapedText.Length + 2);
			builder.Append('%')
				.Append(escapedText)
				.Append('%');
			return builder.ToString();
		}

		/// <summary>
		///     Creates PBKDF2 Password hash.
		///     <see href="https://stackoverflow.com/a/10402129/11101834">More info</see>
		/// </summary>
		/// <param name="password"></param>
		/// <param name="salt">Salt that will be created during hashing</param>
		public static string ToPasswordHash(this string password, out byte[] salt) {

			salt = new byte[SaltLength];
			new RNGCryptoServiceProvider().GetBytes(salt);

			return password.ToPasswordHashBySalt(salt);
		}

		/// <summary>
		///     Creates PBKDF2 Password hash by its salt.
		///     <see href="https://stackoverflow.com/a/10402129/11101834">More info</see>
		/// </summary>
		/// <param name="password"></param>
		/// <param name="salt">Salt with which password was hashed</param>
		public static string ToPasswordHashBySalt(this string password, byte[] salt) {
			if(salt.Length != SaltLength)
				throw new ArgumentException($"Salt must be {SaltLength} bytes", nameof(salt));

			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
			byte[] hash = pbkdf2.GetBytes(PasswordHashLength);

			byte[] hashBytes = new byte[TotalHashLength];
			Array.Copy(salt, 0, hashBytes, 0, SaltLength);
			Array.Copy(hash, 0, hashBytes, SaltLength, PasswordHashLength);

			string passwordHash = Convert.ToBase64String(hashBytes);
			return passwordHash;
		}


		public static class PasswordHashingConstants {
			public const int Iterations = 105458;
			public const int SaltLength = 32;
			public const int PasswordHashLength = 20;
			public const int TotalHashLength = SaltLength + PasswordHashLength;
		}
	}

}