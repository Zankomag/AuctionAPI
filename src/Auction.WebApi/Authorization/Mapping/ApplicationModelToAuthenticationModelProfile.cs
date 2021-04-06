using System.Collections.Generic;
using Auction.Application.Models;
using Auction.WebApi.Authorization.Types;
using AutoMapper;

namespace Auction.WebApi.Authorization.Mapping {

	public class ApplicationModelToAuthenticationModelProfile : Profile {
		public ApplicationModelToAuthenticationModelProfile()
			=> CreateMap<UserDetailedModel, UserIdentity>();
	}

}