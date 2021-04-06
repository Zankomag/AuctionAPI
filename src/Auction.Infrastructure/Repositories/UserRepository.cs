using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
				UserUserRoles = x.UserUserRoles,
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
					UserUserRoles = x.UserUserRoles
				}).FirstOrDefaultAsync();
			return AddRole(user, roleId);
		}

		/// <inheritdoc />
		public bool AddRole(User user, int roleId) {
			if(user == null)
				return false;
			user.UserUserRoles ??= new List<UserUserRole>();
			if(user.UserUserRoles.Any(x => x.UserRoleId == roleId)) {
				return false;
			}

			//Modifying user.UserUserRoles just doesn't work
			Context.Set<UserUserRole>().Add(new UserUserRole {
				UserId = user.Id,
				UserRoleId = roleId
			});
			return true;
		}

		/// <inheritdoc />
		public async Task<bool> RemoveRoleAsync(int userId, int roleId) {
			if(await Context.Set<UserUserRole>()
				.AnyAsync(x => x.UserId == userId && x.UserRoleId == roleId)) {

				//Modifying user.UserUserRoles just doesn't work
				Context.Set<UserUserRole>().Remove(new UserUserRole {
					UserId = userId,
					UserRoleId = roleId
				});
				return true;
			}
			return false;
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
		public async Task<bool> UserExistsAsync(int userId) => await DbSet.AnyAsync(x => x.Id == userId);

		private IQueryable<User> GetAllExceptPasswordHash()
			=> DbSet.Include(x => x.UserUserRoles)
				.ThenInclude(x => x.UserRole)
				.Select(x => new User {
					Id = x.Id,
					UserUserRoles = x.UserUserRoles,
					Email = x.Email,
					FirstName = x.FirstName,
					LastName = x.LastName
				});
	}

}