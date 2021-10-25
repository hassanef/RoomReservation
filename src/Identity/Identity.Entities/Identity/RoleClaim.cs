using Microsoft.AspNetCore.Identity;

namespace Identity.Entities.Identity
{
    public class RoleClaim : IdentityRoleClaim<int>
    {
        public virtual Role Role { get; set; }
    }
}