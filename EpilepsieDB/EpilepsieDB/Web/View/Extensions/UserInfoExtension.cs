using EpilepsieDB.Web.View.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace EpilepsieDB.Web.View.Extensions
{
    public static class UserInfoExtension
    {
        public static UserInfo ToUserInfo(this IdentityUser user)
        {
            return new UserInfo()
            {
                ID = user.Id,
                Email = user.Email
            };
        }

        public static IEnumerable<UserInfo> ToUserInfo(this IEnumerable<IdentityUser> users)
        {
            List<UserInfo> infos = new List<UserInfo>();

            foreach (IdentityUser user in users)
            {
                infos.Add(user.ToUserInfo());
            }

            return infos;
        }
    }
}
