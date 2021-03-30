using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuctionAPI.Core.Entities;
using AuctionAPI.Core.Repositories;
using AuctionAPI.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace AuctionAPI.Infrastructure.Repositories {

	public class UserRepository : Repository<User, int>, IUserRepository {
		/// <inheritdoc />
		public UserRepository(DbContext context) : base(context) { }

		/// <inheritdoc />
		public async Task<IEnumerable<User>> GetAllAsync() => await GetAllExceptPasswordHash().ToListAsync();

		/// <inheritdoc />
		public async Task<User> GetByIdAsync(int id)
			=> await GetAllExceptPasswordHash().FirstOrDefaultAsync(x => x.Id == id);

		/// <inheritdoc />
		public async Task<User> GetByEmailAsync(string email)
			=> await GetAllExceptPasswordHash().FirstOrDefaultAsync(x => x.Email == email);

		/// <inheritdoc />
		public async Task<User> GetAuthorizationInfoByEmailAsync(string email)
			=> await DbSet.Select(x => new User {
				Id = x.Id,
				Role = x.Role,
				PasswordHash = x.PasswordHash,
				PasswordSalt = x.PasswordSalt
			}).FirstOrDefaultAsync(x => x.Email == email);

		/// <inheritdoc />
		public async Task AddAsync(User user) => await DbSet.AddAsync(user);

		/// <inheritdoc />
		public void UpdateRoleAsync(int userId, string role) {
			var user = new User {
				Id = userId,
				Role = role
			};
			DbSet.Attach(user).Property(x => x.Role).IsModified = true;
		}

		/// <inheritdoc />
		public async Task<bool> DeleteAsync(int userId) {
			if(await DbSet.AnyAsync(x => x.Id.Equals(userId))) {
				DbSet.Remove(new User {Id = userId});
				return true;
			}
			return false;
		}

		private IQueryable<User> GetAllExceptPasswordHash()
			=> DbSet.Select(x => new User {
				Id = x.Id,
				Role = x.Role,
				Email = x.Email,
				FirstName = x.FirstName,
				LastName = x.LastName
			});
	}

}