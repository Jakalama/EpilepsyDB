using EpilepsieDB.Authorization;
using EpilepsieDB.Source.Wrapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace EpilepsieDB.Data
{
    public static class SeedUser
    {
        public static async Task SetSystemadmin(IServiceProviderWrapper serviceProvider, string admin, string adminPW)
        {
            await SetUser(serviceProvider, admin, adminPW, Roles.Systemadmin);
        }

        public static async Task SetUser(IServiceProviderWrapper serviceProvider, string user, string userPW)
        {
            await SetUser(serviceProvider, user, userPW, Roles.User);
        }

        private static async Task SetUser(IServiceProviderWrapper serviceProvider, string user, string userPW, string role)
        {
            var adminID = await EnsureUser(serviceProvider, user, userPW);
            await EnsureRole(serviceProvider, adminID, role);
        }

        private static async Task<string> EnsureUser(IServiceProviderWrapper serviceProvider, string username, string password)
        {
            UserManager<IdentityUser> userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            if (userManager == null)
            {
                throw new Exception("UserManager is null");
            }

            IdentityUser user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = username,
                    Email = username,
                    EmailConfirmed = true
                };
                IdentityResult result = await userManager.CreateAsync(user, password);

                if (!result.Succeeded)
                {
                    throw new Exception("The password is probably not strong enough!");
                }
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProviderWrapper serviceProvider, string uid, string role)
        {
            IdentityResult identityResult = null;
            RoleManager<IdentityRole> roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("RoleManager is null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                identityResult = await roleManager.CreateAsync(new IdentityRole(role));
            }

            UserManager<IdentityUser> userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            IdentityUser user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The user must be created before asigning a role!");
            }

            identityResult = await userManager.AddToRoleAsync(user, role);

            return identityResult;
        }
    }
}
