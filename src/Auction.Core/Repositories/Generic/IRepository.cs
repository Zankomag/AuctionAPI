using AuctionAPI.Core.Entities;

namespace AuctionAPI.Core.Repositories.Generic {

	public interface IRepository<TEntity, in TKey> where TEntity : Entity<TKey> { }

}