using AuctionAPI.Core.Entities;
using AuctionAPI.Core.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Infrastructure.Repositories.Generic {

	public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : Entity<TKey>, new() {
		protected DbContext Context;
		protected DbSet<TEntity> DbSet;

		protected Repository(DbContext context) {
			Context = context;
			DbSet = context.Set<TEntity>();
		}
	}

}