// Domain/Entities/ApplicationUser.cs
using System;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
	public class ApplicationUser : IdentityUser<int>
	{
		// Personal details
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }

		// Audit
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; }

		// Computed/read-only
		public string FullName => $"{FirstName} {LastName}";
	}
}
