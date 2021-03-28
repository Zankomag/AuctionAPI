using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionAPI.Core.Entities;

namespace AuctionAPI.Core.Repositories.Generic {

	public interface ICrudRepository<TEntity, in TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey> {
		IQueryable<TEntity> GetAll();

		Task<IEnumerable<TEntity>> GetAllAsync();

		Task<TEntity> GetByIdAsync(TKey id);

		Task AddAsync(TEntity entity);

		void Update(TEntity entity);

		void Delete(TEntity entity);
		
		/// <returns>True on success</returns>
		Task<bool> DeleteByIdAsync(TKey id);
	}


}