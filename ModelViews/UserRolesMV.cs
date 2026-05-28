using Microsoft.AspNetCore.Identity;

namespace Kids_Toys.ModelViews
{
    public class UserRolesMV
    {
        public UserRolesMV()
        {
            userRoles = new List<string>();
        }

        public IdentityUser user { get; set; }

        public List<string> userRoles { get; set; }

    }
}
