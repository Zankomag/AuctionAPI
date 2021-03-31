using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AuctionAPI.Application.Authorization;
using AuctionAPI.Application.Models;
using AuctionAPI.Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI.Web.Controllers {

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
		public async Task<IEnumerable<UserModel>> GetAll()
			=> await userService.GetAllAsync();
		
		// GET api/Users/5
		[HttpGet("{id:int}")]
		public async Task<ActionResult<UserModel>> GetById(int id) {
			//TODO move to method block of custom authorization
			// User can get only theirs own account, Admin can get any
			if(User.IsInRole(Role.Admin)
				|| (Int32.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId) && id == userId)) {
				
				var result = await userService.GetByIdAsync(id);
				if(result == null)
					return NotFound();
				return result;
			}
			return Forbid();
		}

		// GET api/Users/amanda@gmail.com
		[Authorize(Roles = Role.Admin)]
		[HttpGet("{email}")]
		public async Task<ActionResult<UserModel>> GetByEmail(string email) {
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
		public async Task<ActionResult<UserModel>> Add([FromBody] UserInputModel model) {
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
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id) {

			//TODO add same authorization as in GetById()
			throw new NotImplementedException();
			bool result = await userService.DeleteAsync(id);
			if(!result)
				return BadRequest();
			return Ok();
		}
		
	}

}