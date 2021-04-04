namespace Auction.WebApi.Authorization.Constants {

	public static class JwtMessage {
		public const string InvalidToken = "Invalid token";
		public const string SubPropertyDoesntExist = InvalidToken + ": 'sub' property doesn't exist";
		public const string SubPropertyIsNotInteger = InvalidToken + ": 'sub' property is not an integer";
	}

}