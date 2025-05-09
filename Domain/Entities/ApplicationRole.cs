// Domain/Entities/ApplicationRole.cs
using System;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        // A human-readable description of this role
        public string Description { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
