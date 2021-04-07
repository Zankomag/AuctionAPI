using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Core.Entities;
using Auction.Core.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Repositories.Generic {

	public abstract class CrudRepository<TEntity, TKey> : Repository<TEntity, TKey>, ICrudRepository<TEntity, TKey>
		where TEntity : Entity<TKey>, new() {

		protected CrudRepository(DbContext context) : base(context) { }

		/// <inheritdoc />
		public IQueryable<TEntity> GetAll(bool withoutTracking = true) 
			=> withoutTracking ? DbSet.AsNoTracking() : DbSet;

		/// <inheritdoc />
		public virtual async Task<IEnumerable<TEntity>> GetAllAsync() => await GetAll().ToListAsync();

		/// <inheritdoc />
		public virtual async Task<TEntity> GetByIdAsync(TKey id) => await DbSet.FindAsync(id);

		/// <inheritdoc />
		public virtual async Task AddAsync(TEntity entity) => await DbSet.AddAsync(entity);

		/// <inheritdoc />
		public virtual void Update(TEntity entity) => Context.Entry(entity).State = EntityState.Modified;

		/// <inheritdoc />
		public virtual async Task<bool> DeleteByIdAsync(TKey id) {
			if(await DbSet.AnyAsync(x => x.Id.Equals(id))) {
				DbSet.Remove(new TEntity {Id = id});
				return true;
			}
			return false;
		}
	}

}