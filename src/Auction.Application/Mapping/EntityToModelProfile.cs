using System.Linq;
using Auction.Application.Authorization;
using Auction.Application.Models;
using Auction.Core.Entities;
using AutoMapper;

namespace Auction.Application.Mapping {

	public class EntityToModelProfile : Profile {
		public EntityToModelProfile() {
			CreateMap<AuctionItemCategoryInputModel, AuctionItemCategory>()
				.ReverseMap();
			CreateMap<AuctionItemCategory, AuctionItemCategoryDetailedModel>();

			CreateMap<UserInputModel, User>();
			CreateMap<User, UserModel>();
			CreateMap<User, UserDetailedModel>()
				.ForMember(x => x.Roles, c => c.MapFrom(x => 
					x.UserUserRoles.Select(u => Role.GetRoleName(u.UserRoleId))
						.Where(roleName => roleName != null)));

			CreateMap<BidInputModel, Bid>();
			CreateMap<Bid, BidModel>()
				.PreserveReferences();

			CreateMap<AuctionItemInputModel, AuctionItem>()
				.ForMember(x => x.AuctionItemCategoryId, c => c.MapFrom(a => a.CategoryId))
				.ReverseMap();
			CreateMap<AuctionItem, AuctionItemModel>()
				.ForMember(x => x.CategoryId, c => c.MapFrom(a => a.AuctionItemCategoryId))
				.ForMember(x => x.CategoryName, c => c.MapFrom(a => a.AuctionItemCategory.Name))
				.PreserveReferences();

			CreateMap<RoleModel, UserRole>();
		}
	}

}