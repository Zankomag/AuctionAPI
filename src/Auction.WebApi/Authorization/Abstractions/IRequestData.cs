#nullable enable
using Microsoft.AspNetCore.Routing;

namespace Auction.WebApi.Authorization.Abstractions {

	public interface IRequestData {

		RouteData RouteData { get; }

		string? RouteIdValue { get; }
	}

}