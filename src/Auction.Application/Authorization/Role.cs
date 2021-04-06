using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Auction.Application.Authorization {

	//TODO roles need to be saved in db and one user can have many roles
	//and all roles have to be retrieved from db on authentication
	public static class Role {
		//This workaround is used because role strings must be constant due to attribute limitations
		[RoleId(1)] public const string Admin = "Admin";
		[RoleId(2)] public const string User = "User";

		public static List<Core.Entities.UserRole> GetAll() {
			
			var roles = typeof(Role).GetFields(BindingFlags.Public | BindingFlags.Static)
				.Where(fi => fi.IsLiteral && !fi.IsInitOnly)
				.Select(x => new {
					RoleId = x.GetCustomAttribute<RoleIdAttribute>()?.Id,
					Value = x.GetRawConstantValue() as string
				})
				.Where(x => x.Value != null && x.RoleId != null)
				.Select(x => new Core.Entities.UserRole() {
					Id = x.RoleId.Value,
					Name = x.Value
				}).ToList();
			ValidateRoles(roles);
			return roles;
		}

		/// <summary>
		/// Throws exception if roles aren't distinct
		/// </summary>
		private static void ValidateRoles(IEnumerable<Core.Entities.UserRole> roles) {
			HashSet<string> distinctRoleNames = new HashSet<string>();
			HashSet<int> distinctRoleIds = new HashSet<int>();
			foreach(var role in roles) {
				if(!distinctRoleNames.Add(role.Name)) {
					throw new ValidationException($"Role \"{role.Name}\" is duplicated");
				}
				if(!distinctRoleIds.Add(role.Id)) {
					throw new ValidationException($"Role \"{role.Name}\" has Id that is already used");
				}
			}
		}
	}

}