using Auction.Core.Entities;

namespace Auction.Core.Repositories.Generic {

	public interface IRepository<TEntity, in TKey> where TEntity : Entity<TKey> { }

}