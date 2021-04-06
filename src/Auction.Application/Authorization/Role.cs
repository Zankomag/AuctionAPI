using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Auction.Application.Authorization {
	
	public static class Role {
		//This workaround is used because role strings must be constant due to attribute limitations
		[RoleId(1)] public const string Admin = "Admin";
		[RoleId(2)] public const string User = "User";

		public static List<Core.Entities.UserRole> AllRoles { get; private set; }

		static Role() => InitializeRoles();

		public static bool TryGetRoleId(string roleName, out int roleId) {
			roleId = default;
			var role = AllRoles.FirstOrDefault(x => x.Name == roleName);
			if(role == null)
				return false;
			roleId = role.Id;
			return true;
		}

		public static List<Core.Entities.UserRole> GetDefaultRoles() {
			var defaultRoles = new List<Core.Entities.UserRole>();
			var userRole = AllRoles.FirstOrDefault(x => x.Name == User);
			if(userRole != null) {
				defaultRoles.Add(userRole);
			}
			return defaultRoles;
		}
		
		private static void InitializeRoles() {
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
			AllRoles = roles;
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
				if(role.Id <= 0) {
					throw new ValidationException($"\"{role.Name}\" role Id must be greater then 0");
				}
				if(!distinctRoleIds.Add(role.Id)) {
					throw new ValidationException($"Role \"{role.Name}\" has Id that is already used");
				}
			}
		}
		
	}

}