using Auction.Core.Entities;
using Auction.Core.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Repositories.Generic {

	public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey>, new() {
		protected DbContext Context;
		protected DbSet<TEntity> DbSet;

		protected Repository(DbContext context) {
			Context = context;
			DbSet = context.Set<TEntity>();
		}
	}

}