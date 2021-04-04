using Microsoft.AspNetCore.Authorization;

namespace Auction.WebApi.Authorization.Extensions {

	public static class AuthorizationExtensions {

		/// <summary>
		///     Checks whether <paramref name="context" /> has failed/succeeded or is still undetermined
		/// </summary>
		public static bool IsAlreadyDetermined(this AuthorizationHandlerContext context)
			=> context.HasFailed || context.HasSucceeded;
	}

}