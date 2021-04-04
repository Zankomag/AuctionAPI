using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Auction.WebApi.Authorization.Extensions {

	public static class AuthorizationExtensions {

		/// <summary>
		///     Checks whether <paramref name="context" /> has failed/succeeded or is still undetermined
		/// </summary>
		public static bool IsAlreadyDetermined(this AuthorizationHandlerContext context)
			=> context.HasFailed || context.HasSucceeded;

		/// <summary>
		///     Checks whether <paramref name="context" /> has failed/succeeded
		///     or is still undetermined/have pending <typeparamref name="TRequirement" />
		/// </summary>
		public static bool IsAlreadyDetermined<TRequirement>(this AuthorizationHandlerContext context)
			where TRequirement : IAuthorizationRequirement
			=> context.IsAlreadyDetermined() || !context.PendingRequirements.Any(x => x is TRequirement);
	}

}