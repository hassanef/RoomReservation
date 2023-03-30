using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Identity.Entities.Identity
{
    public class Role : IdentityRole<int>
    {
        public Guid? TenantId { get; set; }
        
        public Guid? ApplicationId { get; set; } 
        public Role()
        {
        }

        public Role(string name)
            : this()
        {
            Name = name;
        }
        public Role(string name, Guid tenantId)
            : this(name)
        {
            TenantId = tenantId;
        }

        public Role(string name, string description)
            : this(name)
        {
            Description = description;
        }

        public string Description { get; set; }

        public virtual ICollection<UserRole> Users { get; set; }
        public virtual ICollection<RoleClaim> Claims { get; set; }
    }
}