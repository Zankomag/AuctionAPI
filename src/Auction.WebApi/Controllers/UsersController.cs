using System.Collections.Generic;
using System.Threading.Tasks;
using Auction.Application.Authorization;
using Auction.Application.Models;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers {

	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class UsersController : ControllerBase {
		private readonly IUserService userService;

		public UsersController(IUserService userService)
			=> this.userService = userService;

		// GET api/Users
		[Authorize(Roles = Role.Admin)]
		[HttpGet]
		public async Task<IEnumerable<UserDetailedModel>> GetAll()
			=> await userService.GetAllAsync();

		// GET api/Users/5
		[Authorize(Requirement.IsAdminOrOwnerOf.User.Policy)]
		[HttpGet("{id:int}")]
		public async Task<ActionResult<UserDetailedModel>> GetById(int id) {
			var result = await userService.GetByIdAsync(id);
			if(result == null)
				return NotFound();
			return result;
		}

		// GET api/Users/amanda@gmail.com
		[Authorize(Roles = Role.Admin)]
		[HttpGet("{email}")]
		public async Task<ActionResult<UserDetailedModel>> GetByEmail(string email) {
			if(email == null)
				return BadRequest();
			var result = await userService.GetByEmailAsync(email);
			if(result == null)
				return NotFound();
			return result;
		}

		// POST api/Users
		[AllowAnonymous]
		[HttpPost]
		public async Task<ActionResult<UserDetailedModel>> Add([FromBody] UserInputModel model) {
			var result = await userService.AddAsync(model);
			if(result == null)
				return BadRequest();
			return result;
		}

		// POST api/Users/5/promote
		[Authorize(Roles = Role.Admin)]
		[HttpPost("{id}/promote")]
		public async Task<IActionResult> UpdateRoleToAdmin(int id) {
			var result = await userService.UpdateRoleToAdminAsync(id);
			if(!result)
				return BadRequest();
			return Ok();
		}

		// POST api/Users/5/demote
		[Authorize(Roles = Role.Admin)]
		[HttpPost("{id}/demote")]
		public async Task<IActionResult> UpdateRoleToUser(int id) {
			var result = await userService.UpdateRoleToUserAsync(id);
			if(!result)
				return BadRequest();
			return Ok();
		}

		// DELETE api/Users/5
		[Authorize(Requirement.IsAdminOrOwnerOf.User.Policy)]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id) {
			bool result = await userService.DeleteAsync(id);
			if(!result)
				return BadRequest();
			return Ok();
		}

		//TODO add password changing
	}

}