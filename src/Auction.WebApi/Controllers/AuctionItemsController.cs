using System.Collections.Generic;
using System.Threading.Tasks;
using Auction.Application.Models;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable S125 // Sections of code should not be commented out

namespace Auction.WebApi.Controllers {

	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class AuctionItemsController : ControllerBase {
		private readonly IAuctionItemService auctionItemService;

		public AuctionItemsController(IAuctionItemService auctionItemService)
			=> this.auctionItemService = auctionItemService;

		// GET api/AuctionItems
		[HttpGet]
		public async Task<IEnumerable<AuctionItemModel>> GetAll()
			=> await auctionItemService.GetAllAsync();

		// GET api/AuctionItems/5
		[HttpGet("{id:int}")]
		public async Task<ActionResult<AuctionItemModel>> GetById(int id) {
			var result = await auctionItemService.GetByIdAsync(id);
			if(result == null)
				return NotFound();
			return result;
		}
		
		// GET api/AuctionItems/war%20and%20peace   or api/AuctionItems + body: { name: {"war and peace"} }
		[HttpPost("{name}")]
		public async Task<ActionResult<IEnumerable<AuctionItemModel>>> GetByName(string name) {
			if(name == null)
				return BadRequest();
			var result = await auctionItemService.GetByNameAsync(name);
			if(result == null)
				return NotFound();
			return Ok(result);
		}

		// POST api/AuctionItems
		[HttpPost]
		public async Task<ActionResult<AuctionItemInputModel>> Add([FromBody] AuctionItemInputModel model) {
			var result = await auctionItemService.AddAsync(model);
			if(result == null)
				return BadRequest();
			return result;
		}

		// PUT api/AuctionItems/5
		[Authorize(Requirement.AdminOrOwnerOfAuctionItemId)]
		[HttpPut("{id}")]
		public async Task<ActionResult<AuctionItemInputModel>> Update(int id,
			[FromBody] AuctionItemInputModel model) {

			var result = await auctionItemService.UpdateAsync(id, model);
			if (result == null)
				return BadRequest();
			return result;
		}

		// DELETE api/AuctionItems/5
		[Authorize(Requirement.AdminOrOwnerOfAuctionItemId)]
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id) {
			bool result = await auctionItemService.DeleteByIdAsync(id);
			if (!result)
				return BadRequest();
			return Ok();
		}

	}

}