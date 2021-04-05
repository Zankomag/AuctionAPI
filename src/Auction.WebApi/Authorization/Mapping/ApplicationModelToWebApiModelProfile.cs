using Auction.Application.Models;
using Auction.WebApi.Authorization.Types;
using AutoMapper;

namespace Auction.WebApi.Authorization.Mapping {

	public class ApplicationModelToWebApiModelProfile : Profile {
		public ApplicationModelToWebApiModelProfile() => CreateMap<UserDetailedModel, UserIdentity>();
	}

}