using Auction.Application.Models;
using Auction.Core.Entities;
using AutoMapper;

namespace Auction.Application.Mapping {

	public class EntityToModelProfile : Profile {
		public EntityToModelProfile() {
			CreateMap<AuctionItemCategoryInputModel, AuctionItemCategory>();
			CreateMap<AuctionItemCategory, AuctionItemCategoryDetailedModel>();

			CreateMap<UserInputModel, User>();
			CreateMap<User, UserModel>();
			CreateMap<User, UserDetailedModel>();

			CreateMap<BidInputModel, Bid>();
			CreateMap<Bid, BidModel>();

			CreateMap<AuctionItemInputModel, AuctionItem>();
			CreateMap<AuctionItem, AuctionItemModel>();
		}
	}

}