using System.Collections.Generic;
using System.Threading.Tasks;
using AuctionAPI.Application.Models.Generic;

namespace AuctionAPI.Application.Services.Abstractions.Generic {

	public interface ICrudService<TModel, in TKey> where TModel : Model<TKey> {

		Task<IEnumerable<TModel>> GetAllAsync();

		Task<TModel> GetByIdAsync(TKey id);
		
		/// <returns>Created model on success or null on validation failure.</returns>
		Task<TModel> AddAsync(TModel model);

		/// <returns>Updated model on success or null on validation failure.</returns>
		Task<TModel> UpdateAsync(TKey id, TModel model);

		Task DeleteByIdAsync(TKey id);
	}

}