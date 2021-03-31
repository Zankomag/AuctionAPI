using System.Collections.Generic;
using System.Threading.Tasks;
using Auction.Application.Models.Generic;

namespace Auction.Application.Services.Abstractions.Generic {

	public interface ICrudService<TModel, TInputModel, in TKey>
		where TModel : TInputModel 
		where TInputModel : Model<TKey> {

		Task<IEnumerable<TModel>> GetAllAsync();

		Task<TModel> GetByIdAsync(TKey id);
		
		/// <returns>Created model on success or null on validation failure.</returns>
		Task<TInputModel> AddAsync(TInputModel model);

		/// <returns>Updated model on success or null on validation failure.</returns>
		Task<TModel> UpdateAsync(TKey id, TInputModel model);
		
		/// <returns>True on success</returns>
		Task<bool> DeleteByIdAsync(TKey id);
	}

}