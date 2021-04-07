using System.Threading.Tasks;
using Auction.Application.Models;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization.Attributes;
using Auction.WebApi.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers {

	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class BidsController : ControllerBase {
		private readonly IBidService bidService;

		public BidsController(IBidService bidService)
			=> this.bidService = bidService;

		// GET /api/Bids/5
		[HttpGet("{id:int}")]
		public async Task<ActionResult<BidModel>> GetById(int id) {
			var result = await bidService.GetByIdWithDetailsAsync(id);
			if(result == null)
				return NotFound();
			return result;
		}

		// POST /api/Bids
		[AuthorizeExcept(Requirement.OwnerOfAuctionItemId)]
		[HttpPost]
		public async Task<ActionResult<BidModel>> Add(BidInputModel model) {
			var result = await bidService.AddAsync(model);
			if(result == null)
				return BadRequest();
			return result;
		}
	}

}