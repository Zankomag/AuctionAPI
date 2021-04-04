#nullable enable
using Microsoft.AspNetCore.Routing;

namespace Auction.WebApi.Authorization.Abstractions {

	public interface IRequestData {
		/// <summary>
		///     'sub' field of JWT
		/// </summary>
		string? UserIdString { get; set; }

		/// <summary>
		///     'sub' field of JWT
		/// </summary>
		int? UserId { get; set; }

		RouteData RouteData { get; }

		string? RouteIdString { get; }
	}

}