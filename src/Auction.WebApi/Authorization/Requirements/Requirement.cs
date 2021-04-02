using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.WebApi.Authorization.Requirements {

	/// <summary>
	///     Policy requirement
	/// </summary>
	public static partial class Requirement {

		//Inheritdoc does not inherit from interface
		/// <summary>
		///     Requirement category
		/// </summary>
		public abstract class Category { }


		/// <summary>
		///     Requirement handler
		/// </summary>
		public abstract class HandlerBase<TRequirement> : AuthorizationHandler<TRequirement> where TRequirement :
			IAuthorizationRequirement {

			protected readonly IHttpContextAccessor HttpContextAccessor;
			private RouteData routeData;

			protected RouteData RouteData
				=> routeData ??= HttpContextAccessor.HttpContext!.GetRouteData();

			protected HandlerBase(IHttpContextAccessor httpContextAccessor)
				=> HttpContextAccessor = httpContextAccessor;
		}
	}

}