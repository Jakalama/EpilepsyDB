using EpilepsieDB.Services;
using EpilepsieDB.Web.View.ViewModels;

namespace EpilepsieDB.Web.View.Extensions
{
    public static class PermissionExtension
    {
        public static UserPermissions ToUserPermissions(this Permissions dto)
        {
            return new UserPermissions()
            {
                IsSystemadmin = dto.IsSystemadmin,
                IsUser = dto.IsUser,
                IsScanCreator = dto.IsScanCreator,
                IsScanDownloader = dto.IsScanDownloader,
                IsScanReader = dto.IsScanReader,
            };
        }
    }
}
