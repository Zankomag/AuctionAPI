using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionAPI.Application.Models.Generic;

namespace AuctionAPI.Application.Services.Abstractions.Generic {

	public interface ICrudService<TModel, in TKey> where TModel : Model<TKey> {

		Task<IEnumerable<TModel>> GetAllAsync();

		Task<TModel> GetByIdAsync(TKey id);

		Task<TModel> AddAsync(TModel model);

		Task UpdateAsync(TModel model);

		Task DeleteByIdAsync(TKey id);
	}

}