using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Jewellery.Authorization
{
    public class JewelleryAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
            context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
            context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);

            context.CreatePermission(PermissionNames.Pages_Customers, L("Customers"));
            context.CreatePermission(PermissionNames.Pages_MetalTypes, L("MetalTypes"));
            context.CreatePermission(PermissionNames.Pages_Orders, L("Orders"));
            context.CreatePermission(PermissionNames.Pages_Sales, L("Sales"));
            context.CreatePermission(PermissionNames.Pages_Invoice, L("Invoices"));
            context.CreatePermission(PermissionNames.Pages_Products, L("Products"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, JewelleryConsts.LocalizationSourceName);
        }
    }
}
