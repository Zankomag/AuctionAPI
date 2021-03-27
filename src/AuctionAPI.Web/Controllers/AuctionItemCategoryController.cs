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
		public async Task<IEnumerable<AuctionItemCategoryModel>> GetAll()
			=> await auctionItemCategoryService.GetAllAsync();

		// GET api/AuctionItemCategory/5
		[HttpGet("{id}")]
		public async Task<AuctionItemCategoryModel> GetById(int id)
			=> await auctionItemCategoryService.GetByIdAsync(id);

		// GET api/AuctionItemCategory/Books
		[HttpGet("{name}")]
		public async Task<IEnumerable<AuctionItemCategoryModel>> GetByName(string name)
			=> await auctionItemCategoryService.GetByNameAsync(name);

		// POST api/AuctionItemCategory
		[HttpPost]
		public async Task<AuctionItemCategoryModel> Add([FromBody] AuctionItemCategoryModel model)
			=> await auctionItemCategoryService.AddAsync(model);

		// PUT api/AuctionItemCategory/5
		[HttpPut("{id}")]
		public async Task<AuctionItemCategoryModel> Update(int id, [FromBody] AuctionItemCategoryModel model)
			=> await auctionItemCategoryService.UpdateAsync(id, model);

		// DELETE api/AuctionItemCategory/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id) {
			await auctionItemCategoryService.DeleteByIdAsync(id);
			return Ok();
		}
	}

}