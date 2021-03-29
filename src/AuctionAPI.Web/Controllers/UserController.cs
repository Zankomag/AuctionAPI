using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionAPI.Application.Models;
using AuctionAPI.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI.Web.Controllers {

	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class UserController : ControllerBase {
		private readonly IUserService userService;

		public UserController(IUserService userService)
			=> this.userService = userService;


		// GET api/Users
		[HttpGet]
		public async Task<IEnumerable<UserModel>> GetAll()
			=> await userService.GetAllAsync();


		// GET api/Users/5
		[HttpGet("{id:int}")]
		public async Task<ActionResult<UserModel>> GetById(int id) {
			var result = await userService.GetByIdAsync(id);
			if(result == null)
				return NotFound();
			return result;
		}

		// GET api/Users/amanda@gmail.com
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
		[HttpPost]
		public async Task<ActionResult<UserModel>> Add([FromBody] UserInputModel model) {
			var result = await userService.AddAsync(model);
			if(result == null)
				return BadRequest();
			return result;
		}

		// POST api/Users/5?role=Admin
		[HttpPost("{id}")]
		public async Task<IActionResult> UpdateRole(int id, string role) {

			var result = await userService.UpdateRoleAsync(id, role);
			if(!result)
				return BadRequest();
			return Ok();
		}

		// DELETE api/Users/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id) {
			bool result = await userService.DeleteAsync(id);
			if(!result)
				return BadRequest();
			return Ok();
		}
		
	}

}