using System;

// ReSharper disable InheritdocConsiderUsage

namespace Auction.Application.Authorization {

	/// <summary>
	///     Defines Id of Role in database
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	internal sealed class RoleIdAttribute : Attribute {
		public int Id { get; }
		public RoleIdAttribute(int id) => Id = id;
	}

}