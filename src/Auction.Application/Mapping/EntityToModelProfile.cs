using Auction.Application.Models;
using Auction.Core.Entities;
using AutoMapper;

namespace Auction.Application.Mapping {

	public class EntityToModelProfile : Profile {
		public EntityToModelProfile() {
			CreateMap<AuctionItemCategory, AuctionItemCategoryInputModel>().ReverseMap();
			CreateMap<AuctionItemCategory, AuctionItemCategoryDetailedModel>().ReverseMap();

			CreateMap<User, UserInputModel>().ReverseMap();
			CreateMap<User, UserDetailedModel>().ReverseMap();
		}
	}

}