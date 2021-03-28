using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionAPI.Core.Entities;
using AuctionAPI.Core.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Infrastructure.Repositories.Generic {

	public class CrudRepository<TEntity, TKey> : Repository<TEntity, TKey>, ICrudRepository<TEntity, TKey>
		where TEntity : Entity<TKey>, new() {

		public CrudRepository(DbContext context) : base(context) { }

		/// <inheritdoc />
		public IQueryable<TEntity> GetAll() => DbSet.AsNoTracking();

		/// <inheritdoc />
		public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await GetAll().ToListAsync();

		/// <inheritdoc />
		public async Task<TEntity> GetByIdAsync(TKey id) => await DbSet.FindAsync(id);

		/// <inheritdoc />
		public async Task AddAsync(TEntity entity) => await DbSet.AddAsync(entity);

		/// <inheritdoc />
		public virtual void Update(TEntity entity) => Context.Entry(entity).State = EntityState.Modified;

		/// <inheritdoc />
		public async Task<bool> DeleteByIdAsync(TKey id) {
			if(await DbSet.AnyAsync(x => x.Id.Equals(id))) {
				DbSet.Remove(new TEntity {Id = id});
				return true;
			}
			return false;
		}
	}

}