using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;

namespace Identity.Entities.Identity
{
    public class UserClaim : IdentityUserClaim<int>
    {
        public override void InitializeFromClaim(Claim claim)
        {
          
        }

        public Guid? TenantId { get; set; }
        public Guid? ApplicationId { get; set; }
        public virtual User User { get; set; }
    }
}