using Microsoft.AspNetCore.Identity;

namespace Identity.Entities.Identity
{
    public class UserLogin : IdentityUserLogin<int>
    {
        public virtual User User { get; set; }
    }
}