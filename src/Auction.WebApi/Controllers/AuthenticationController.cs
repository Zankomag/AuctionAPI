using System.Threading.Tasks;
using Auction.Application.Models;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Abstractions;
using Auction.WebApi.Authorization.Constants;
using Auction.WebApi.Authorization.Types;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers {

	[ApiController]
	[Route("api/auth")]
	[Produces("application/json")]
	public class AuthenticationController : ControllerBase {
		private readonly IAuthenticationService authenticationService;
		private readonly IUserService userService;

		public AuthenticationController(IAuthenticationService authenticationService, IUserService userService) {
			this.authenticationService = authenticationService;
			this.userService = userService;
		}

		// POST /api/auth/token
		[HttpPost("token")]
		public async Task<ActionResult<TokenModel>> Authenticate([FromBody] AuthenticationModel model) {
			var tokenModel = await authenticationService.GetToken(model.Email, model.Password);

			if(tokenModel == null)
				return BadRequest(AuthenticationMessage.WrongUsernameOrPassword);

			return tokenModel;
		}

		// POST /api/auth/changePassword
		[HttpPost("changePassword")]
		public async Task<IActionResult> Authenticate([FromBody] PasswordChangeModel model) {
			var result = await userService.UpdatePasswordAsync(model);

			if(!result)
				return BadRequest();
			return Ok();
		}
		
		//TODO allow getting token by other token (extend token) without login credentials, just by auth
	}

}