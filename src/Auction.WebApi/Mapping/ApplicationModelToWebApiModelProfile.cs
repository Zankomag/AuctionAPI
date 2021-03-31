using Auction.Application.Models;
using Auction.WebApi.Authorization.Models;
using AutoMapper;

namespace Auction.WebApi.Mapping {

	public class ApplicationModelToWebApiModelProfile : Profile {
		public ApplicationModelToWebApiModelProfile() => CreateMap<UserModel, UserIdentity>();
	}

}