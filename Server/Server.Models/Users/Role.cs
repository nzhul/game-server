using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Server.Models.Users
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
