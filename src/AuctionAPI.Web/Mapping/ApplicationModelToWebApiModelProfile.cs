using AuctionAPI.Application.Models;
using AuctionAPI.Web.Authentication;
using AutoMapper;

namespace AuctionAPI.Web.Mapping {

	public class ApplicationModelToWebApiModelProfile : Profile {
		public ApplicationModelToWebApiModelProfile() => CreateMap<UserModel, UserIdentity>();
	}

}