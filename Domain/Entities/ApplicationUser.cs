using System;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
	public class ApplicationUser : IdentityUser<int>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; set; }
		public string FullName => $"{FirstName} {LastName}";
	}
}
