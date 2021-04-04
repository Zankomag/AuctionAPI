#nullable enable
using Auction.WebApi.Authorization.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Auction.WebApi.Authorization.Services {

	public class RequestData : IRequestData {
		private readonly IHttpContextAccessor httpContextAccessor;
		private RouteData? routeData;
		
		public RequestData(IHttpContextAccessor httpContextAccessor)
			=> this.httpContextAccessor = httpContextAccessor;

		/// <inheritdoc />
		public string? UserIdString { get; set; }

		/// <inheritdoc />
		public int? UserId { get; set; }

		/// <inheritdoc />
		public RouteData RouteData
			=> routeData ??= httpContextAccessor.HttpContext!.GetRouteData();

		/// <inheritdoc />
		public string? RouteIdString => RouteData.Values["id"] as string;
	}

}