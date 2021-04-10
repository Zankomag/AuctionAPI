using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Auction.Application.Models;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization;
using Auction.WebApi.Authorization.Attributes;
using Auction.WebApi.Authorization.Extensions;
using Auction.WebApi.Authorization.Types;
using Auction.WebApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Auction.WebApi.Controllers {

	[Authorize]
	[ApiController]
	[Route(controllerRoute)]
	[Produces("application/json")]
	public class AuctionItemsController : ControllerBase {
		private const string controllerRoute = "api/[controller]";
		private readonly IAuctionItemService auctionItemService;
		private readonly IBidService bidService;

		public AuctionItemsController(IAuctionItemService auctionItemService, IBidService bidService) {
			this.auctionItemService = auctionItemService;
			this.bidService = bidService;
		}

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

		// GET api/AuctionItems/war%20and%20peace
		[HttpGet("search/{name}")]
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
			if(!User.TryGetUserIdentity(out UserIdentity userIdentity))
				return StatusCode(500);
			model.SellerId = userIdentity.Id;
			var result = await auctionItemService.AddAsync(model);
			if(result == null)
				return BadRequest();
			return result;
		}

		// POST api/AuctionItems/5/images
		[Authorize(Requirement.OwnerOfAuctionItemId)]
		[HttpPost("{id:int}/images")]
		public async Task<ActionResult> AddImages(int id, IFormFile image) {
			if(image?.IsImage() == true) {
				await using(var stream = new MemoryStream()) {
					await image.CopyToAsync(stream);
					byte[] imageBytes = stream.ToArray();
					var result = await auctionItemService
						.AddImageAsync(id, imageBytes, Path.GetExtension(image.FileName));
					if(!result)
						return BadRequest();
					return Ok();
				}
			}
			return BadRequest();
		}

		// GET api/AuctionItems/images
		[HttpGet("images/{id:int}")]
		public async Task<IActionResult> GetImage(int id) {
			var result = await auctionItemService.GetImageByIdAsync(id);
			if(result == null)
				return NotFound();
			new FileExtensionContentTypeProvider().TryGetContentType(result.FileExtension, out string contentType);
			contentType ??= "application/octet-stream";
			return new FileContentResult(result.File, contentType) {
				FileDownloadName = $"picture{result.FileExtension}"
			};
		}

		// PUT api/AuctionItems/5
		[Authorize(Requirement.OwnerOfAuctionItemId)]
		[HttpPut("{id:int}")]
		public async Task<ActionResult<AuctionItemInputModel>> Update(int id,
			[FromBody] AuctionItemInputModel model) {

			if(!User.TryGetUserIdentity(out UserIdentity userIdentity))
				return StatusCode(500);
			model.SellerId = userIdentity.Id;
			var result = await auctionItemService.UpdateAsync(id, model);
			if(result == null)
				return BadRequest();
			return result;
		}

		// DELETE api/AuctionItems/5
		[AuthorizeAny(Requirement.Admin, Requirement.OwnerOfAuctionItemId)]
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int id) {
			bool result = await auctionItemService.DeleteByIdAsync(id);
			if(!result)
				return BadRequest();
			return Ok();
		}

		// GET api/AuctionItems/5/bids
		[HttpGet("{id:int}/bids")]
		public async Task<ActionResult<IEnumerable<BidModel>>> GetBidsByAuctionItemId(int id) {
			var result = await bidService.GetByAuctionItemIdWithDetailsAsync(id);
			if(result == null)
				return NotFound();
			return Ok(result);
		}

		// POST api/AuctionItems/5/bids
		[AuthorizeExcept(Requirement.OwnerOfAuctionItemId)]
		[HttpPost("{id:int}/bids")]
		public async Task<ActionResult<BidModel>> AddBid(BidInputModel model, int id) {
			if(!User.TryGetUserIdentity(out UserIdentity userIdentity))
				return StatusCode(500);
			var result = await bidService.AddAsync(model, userIdentity.Id, id);
			if(result == null)
				return BadRequest();
			return result;
		}

		// GET /apiAuctionItems/bids/5
		[HttpGet("bids/{id:int}")]
		public async Task<ActionResult<BidModel>> GetBidById(int id) {
			var result = await bidService.GetByIdWithDetailsAsync(id);
			if(result == null)
				return NotFound();
			return result;
		}

		// DELETE api/images/5
		[AuthorizeAny(Requirement.Admin, Requirement.OwnerOfAuctionItemImageId)]
		[HttpDelete("images/{id:int}")]
		public async Task<IActionResult> DeleteImage(int id) {
			bool result = await auctionItemService.DeleteImageByIdAsync(id);
			if(!result)
				return BadRequest();
			return Ok();
		}
	}

}