using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;

namespace EpilepsieDB.Authorization
{
    public class RoleSet
    {
        public const string AllowRead = $"{Roles.Reader},{Roles.Downloader},{Roles.Creator},{Roles.User},{Roles.Systemadmin}";
        public const string AllowDownload = $"{Roles.Downloader},{Roles.Creator},{Roles.User},{Roles.Systemadmin}";
        public const string AllowCreateScan = $"{Roles.Creator},{Roles.User},{Roles.Systemadmin}";
        public const string AllowCreatePatient = $"{Roles.User},{Roles.Systemadmin}";
        public const string IsSystemadmin = $"{Roles.Systemadmin}";
    }
}
