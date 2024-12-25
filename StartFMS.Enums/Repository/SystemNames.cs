using System.ComponentModel.DataAnnotations;

namespace StartFMS.Enums.Repository
{
    public enum SystemNames
    {
        [Display(Name = "UserRole", Description = "角色清單")]
        UserRole,
        [Display(Name = "UserAccounts", Description = "使用者帳號")]
        UserAccounts,
        [Display(Name = "SystemCatalogItems", Description = "系統目錄")]
        SystemCatalogItems
    }
}
