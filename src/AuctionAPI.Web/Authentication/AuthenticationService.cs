using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using AuctionAPI.Application.Services.Abstractions;
using AuctionAPI.Web.Authentication.Abstractions;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AuctionAPI.Web.Authentication {

	public class AuthenticationService : IAuthenticationService {
		private readonly IUserService userService;
		private readonly IMapper mapper;
		private readonly JwtSettings jwtSettings;

		public AuthenticationService(IUserService userService, IMapper mapper,
			IOptions<JwtSettings> jwtSettingsOptions) {
			this.userService = userService;
			this.mapper = mapper;
			jwtSettings = jwtSettingsOptions.Value;
		}

		/// <inheritdoc />
		public async Task<TokenModel> GetToken(string email, string password) {
			var userModel = await userService.GetAuthorizationInfoByEmailAndPasswordAsync(email, password);
			if(userModel == null)
				return null;
			var user = mapper.Map<UserIdentity>(userModel);

			var expiration = DateTime.Now.AddDays(1);
			
			var claims = new List<Claim> {
				new("role", user.Role),
				new("sub", user.Id.ToString())
			};

			var signingKey = new SymmetricSecurityKey(jwtSettings.SecretBytes);

			var token = new JwtSecurityToken(expires: expiration,
				claims: claims,
				signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
			
			return new TokenModel() {
				Expiration = expiration,
				Token = new JwtSecurityTokenHandler().WriteToken(token)
			};


		}
	}

}