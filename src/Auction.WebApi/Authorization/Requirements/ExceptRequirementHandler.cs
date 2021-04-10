using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     This requirement fails if requirement other handlers succeed requirement inherited from this
	/// </summary>
	public interface IExceptRequirement : IAuthorizationRequirement { }
	
	/// <summary>
	///     This handler must be registered after or Handlers of combined with Not policies
	/// </summary>
	public class ExceptRequirementHandler : AuthorizationHandler<IExceptRequirement> {

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
			IExceptRequirement requirement) {

			if(!context.HasFailed) {
				if(context.PendingRequirements.Any(x => x is IExceptRequirement)) {
					context.Succeed(requirement);
				} else {
					context.Fail();
				}
			}
			return Task.CompletedTask;
		}
	}

}