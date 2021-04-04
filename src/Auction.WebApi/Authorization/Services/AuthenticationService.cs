using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Types;
using AutoMapper;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Auction.WebApi.Authorization.Services {

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
			var signingKey = new SymmetricSecurityKey(jwtSettings.SecretBytes);

			var token = new JwtSecurityToken(expires: expiration,
				claims: user.GetClaims(),
				signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

			return new TokenModel {
				Expiration = expiration,
				Token = new JwtSecurityTokenHandler().WriteToken(token)
			};


		}
	}

}