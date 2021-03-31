using Auction.Application.Models;
using Auction.WebApi.Authentication.Models;
using AutoMapper;

namespace Auction.WebApi.Mapping {

	public class ApplicationModelToWebApiModelProfile : Profile {
		public ApplicationModelToWebApiModelProfile() => CreateMap<UserModel, UserIdentity>();
	}

}