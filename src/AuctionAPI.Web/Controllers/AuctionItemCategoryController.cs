using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionAPI.Application.Models;
using AuctionAPI.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AuctionAPI.Web.Controllers {

	[ApiController]
	[Route("api/[controller]")]
	[Produces("application/json")]
	public class AuctionItemCategoryController : ControllerBase {
		private readonly IAuctionItemCategoryService auctionItemCategoryService;

		public AuctionItemCategoryController(IAuctionItemCategoryService auctionItemCategoryService)
			=> this.auctionItemCategoryService = auctionItemCategoryService;


		// GET api/AuctionItemCategory
		[HttpGet]
		public async Task<IEnumerable<AuctionItemCategoryDetailedModel>> GetAll()
			=> await auctionItemCategoryService.GetAllAsync();

		
		// GET api/AuctionItemCategory/5
		//about {id:int}: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-3.1#route-constraint-reference
		[HttpGet("{id:int}")]
		public async Task<AuctionItemCategoryDetailedModel> GetById(int id)
			=> await auctionItemCategoryService.GetByIdAsync(id);

		// GET api/AuctionItemCategory/Books
		[HttpGet("{name}")]
		public async Task<IEnumerable<AuctionItemCategoryDetailedModel>> GetByName(string name)
			=> await auctionItemCategoryService.GetByNameAsync(name);

		// POST api/AuctionItemCategory
		[HttpPost]
		public async Task<AuctionItemCategoryInputModel> Add([FromBody] AuctionItemCategoryInputModel model)
			=> await auctionItemCategoryService.AddAsync(model);

		// PUT api/AuctionItemCategory/5
		[HttpPut("{id}")]
		public async Task<AuctionItemCategoryDetailedModel> Update(int id, [FromBody] AuctionItemCategoryInputModel model)
			=> await auctionItemCategoryService.UpdateAsync(id, model);

		// DELETE api/AuctionItemCategory/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id) {
			await auctionItemCategoryService.DeleteByIdAsync(id);
			return Ok();
		}
	}

}