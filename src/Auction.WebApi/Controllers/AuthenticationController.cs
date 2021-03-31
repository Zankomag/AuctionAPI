using System.Threading.Tasks;
using Auction.Application.Models;
using Auction.WebApi.Authentication.Abstractions;
using Auction.WebApi.Authentication.Models;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers {

	[ApiController]
	[Route("api/token")]
	[Produces("application/json")]
	public class AuthenticationController : ControllerBase {
		private readonly IAuthenticationService authenticationService;

		public AuthenticationController(IAuthenticationService authenticationService)
			=> this.authenticationService = authenticationService;

		// POST /api/token
		[HttpPost]
		public async Task<ActionResult<TokenModel>> Authenticate([FromBody] AuthenticationModel model) {
			var tokenModel = await authenticationService.GetToken(model.Email, model.Password);

			if(tokenModel == null)
				return BadRequest("Wrong username or password");

			return tokenModel;
		}
	}

}