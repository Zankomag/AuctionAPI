using System.Threading.Tasks;
using Auction.Application.Models;
using Auction.Application.Services.Abstractions;
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
	}

}