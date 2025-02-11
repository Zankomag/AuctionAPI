﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Application.Models;
using Auction.Application.Services.Abstractions;
using Auction.WebApi.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auction.WebApi.Controllers {

	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class AuctionItemCategoriesController : ControllerBase {
		private readonly IAuctionItemCategoryService auctionItemCategoryService;

		public AuctionItemCategoriesController(IAuctionItemCategoryService auctionItemCategoryService)
			=> this.auctionItemCategoryService = auctionItemCategoryService;


		// GET api/AuctionItemCategories
		[HttpGet]
		public async Task<IEnumerable<AuctionItemCategoryDetailedModel>> GetAll()
			=> await auctionItemCategoryService.GetAllAsync();


		// GET api/AuctionItemCategories/5
		//about {id:int}: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-3.1#route-constraint-reference
		[HttpGet("{id:int}")]
		public async Task<ActionResult<AuctionItemCategoryDetailedModel>> GetById(int id) {
			var result = await auctionItemCategoryService.GetByIdAsync(id);
			if(result == null)
				return NotFound();
			return result;
		}

		// GET api/AuctionItemCategories/Books
		[HttpGet("{name}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AuctionItemCategoryDetailedModel>))]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetByName(string name) {
			if(name == null)
				return BadRequest();
			IEnumerable<AuctionItemCategoryDetailedModel>
				result = await auctionItemCategoryService.GetByNameAsync(name);
			if(result?.Any() != true)
				return NotFound();
			return Ok(result);
		}

		// POST api/AuctionItemCategories
		[Authorize(Requirement.Admin)]
		[HttpPost]
		public async Task<ActionResult<AuctionItemCategoryInputModel>> Add(
			[FromBody] AuctionItemCategoryInputModel model) {

			var result = await auctionItemCategoryService.AddAsync(model);
			if(result == null)
				return BadRequest();
			return result;
		}

		// PUT api/AuctionItemCategory/5
		[Authorize(Requirement.Admin)]
		[HttpPut("{id}")]
		public async Task<ActionResult<AuctionItemCategoryInputModel>> Update(int id,
			[FromBody] AuctionItemCategoryInputModel model) {

			var result = await auctionItemCategoryService.UpdateAsync(id, model);
			if(result == null)
				return BadRequest();
			return result;
		}

		// DELETE api/AuctionItemCategories/5
		[HttpDelete("{id}")]
		[Authorize(Requirement.Admin)]
		public async Task<IActionResult> Delete(int id) {
			bool result = await auctionItemCategoryService.DeleteByIdAsync(id);
			if(!result)
				return BadRequest();
			return Ok();
		}
	}

}