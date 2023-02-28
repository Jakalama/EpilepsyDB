using EpilepsieDB.Services;
using EpilepsieDB.Web.View.ViewModels;

namespace EpilepsieDB.Web.View.Extensions
{
    public static class InviteExtension
    {
        public static UserInvite ToUserInvite(this Invite invite)
        {
            return new UserInvite()
            {
                Email = invite.Email,
                IsSystemadmin = invite.IsSystemadmin,
                IsUser = invite.IsUser,
                IsScanCreator = invite.IsScanCreator,
                IsScanDownloader = invite.IsScanDownloader,
                IsScanReader = invite.IsScanReader
            };
        }
    }
}
