using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionAPI.Core.Entities;
using AuctionAPI.Core.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Infrastructure.Repositories.Generic {

	public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey>, new() {

		protected DbContext Context;
		protected DbSet<TEntity> DbSet;

		public Repository(DbContext context) {
			Context = context;
			DbSet = context.Set<TEntity>();
		}

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
		public void Delete(TEntity entity) => DbSet.Remove(entity);

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