using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Jewellery.Controllers
{
    public abstract class JewelleryControllerBase: AbpController
    {
        protected JewelleryControllerBase()
        {
            LocalizationSourceName = JewelleryConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
