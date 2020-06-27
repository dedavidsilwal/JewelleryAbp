using Abp.Authorization;
using Jewellery.Authorization.Roles;
using Jewellery.Authorization.Users;

namespace Jewellery.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
