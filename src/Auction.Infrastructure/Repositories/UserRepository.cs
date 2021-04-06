using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auction.Application.Authorization;
using Auction.Core.Entities;
using Auction.Core.Repositories;
using Auction.Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Auction.Infrastructure.Repositories {

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
			=> await DbSet.Where(x => x.Email == email).Select(x => new User {
				Id = x.Id,
				Roles = x.Roles,
				PasswordHash = x.PasswordHash,
				PasswordSalt = x.PasswordSalt
			}).FirstOrDefaultAsync();

		/// <inheritdoc />
		public async Task AddAsync(User user) => await DbSet.AddAsync(user);
		
		/// <inheritdoc />
		public async Task<bool> AddRoleAsync(int userId, int roleId) {
			var user = await DbSet.Where(x => x.Id == userId)
				.Select(x => new User {
					Id = x.Id,
					Roles = x.Roles
				}).FirstOrDefaultAsync();
			if(user?.Roles.Any(x => x.Id == roleId) != false) {
				return false;
			}
			user.Roles.Add(new UserRole(){Id = roleId});
			return true;
		}

		/// <inheritdoc />
		public async Task<bool> RemoveRoleAsync(int userId, int roleId) {
			var user = await DbSet.Where(x => x.Id == userId)
				.Select(x => new User {
					Id = x.Id,
					Roles = x.Roles
				}).FirstOrDefaultAsync();
			UserRole role = user?.Roles.FirstOrDefault(x => x.Id == roleId);
			if(role == null) {
				return false;
			}
			user.Roles.Remove(role);
			return true;
		}

		/// <inheritdoc />
		public async Task<bool> DeleteAsync(int userId) {
			if(await DbSet.AnyAsync(x => x.Id.Equals(userId))) {
				DbSet.Remove(new User {Id = userId});
				return true;
			}
			return false;
		}

		/// <inheritdoc />
		public async Task<bool> UserExists(int userId) => await DbSet.AnyAsync(x => x.Id == userId);

		private IQueryable<User> GetAllExceptPasswordHash()
			=> DbSet.Select(x => new User {
				Id = x.Id,
				Roles = x.Roles,
				Email = x.Email,
				FirstName = x.FirstName,
				LastName = x.LastName
			});
	}

}