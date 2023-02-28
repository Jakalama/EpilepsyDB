using EpilepsieDB.Authorization;
using EpilepsieDB.Source.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace EpilepsieDB.Services.Impl
{
    public class UsersService : IUsersService
    {
        readonly UserManager<IdentityUser> _userManager;
        readonly RoleManager<IdentityRole> _roleManager;

        public UsersService(
            UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> CheckPassword(IdentityUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ConfirmEmail(string userID, string emailConfirmationCode)
        {
            var user = await _userManager.FindByIdAsync(userID);

            if (user == null)
                return IdentityResult.Failed();

            return await _userManager.ConfirmEmailAsync(user, emailConfirmationCode);
        }

        public async Task<UserResult> CreateNewUser(UserInvite invite)
        {
            string randPW = RandomName.Generate(15);

            var user = new IdentityUser { UserName = invite.Email, Email = invite.Email };
            var result = await _userManager.CreateAsync(user, randPW);

            if (result.Succeeded)
            {
                result = await AssignRole(user, invite.IsSystemadmin, invite.IsUser, invite.IsScanCreator, invite.IsScanDownloader, invite.IsScanReader);

                string emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string passwordSetCode = await _userManager.GeneratePasswordResetTokenAsync(user);
                passwordSetCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordSetCode));

                return new UserResult()
                {
                    UserID = user.Id,
                    EmailConfirmationCode = emailConfirmationCode,
                    PasswordSetCode = passwordSetCode
                };
            }

            return null;
        }

        private async Task<IdentityResult> AssignRole(IdentityUser user, bool isAdmin, bool isUser, bool isCreator, bool isDownloader, bool isReader)
        {
            if (isAdmin)
                return await AssignRole(user, Roles.Systemadmin);

            if (isUser)
                return await AssignRole(user, Roles.User);

            if (isCreator)
                return await AssignRole(user, Roles.Creator);

            if (isDownloader)
                return await AssignRole(user, Roles.Downloader);

            return await AssignRole(user, Roles.Reader);
        }

        private async Task<IdentityResult> AssignRole(IdentityUser user, string role)
        {
            IdentityResult identityResult = null;

            if (!await _roleManager.RoleExistsAsync(role))
            {
                identityResult = await _roleManager.CreateAsync(new IdentityRole(role));
            }

            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityUser> FindByName(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }

        public async Task<IdentityUser> GetUser(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<IEnumerable<IdentityUser>> GetUsers()
        {
            return await _userManager.Users.ToListAsync();
        }

        public async Task<List<string>> GetRoles(string id)
        {
            IdentityUser user = await GetUser(id);

            if (user != null)
                return (List<string>) await _userManager.GetRolesAsync(user);

            return new List<string>();
        }

        public async Task ChangeRole(string id, UserPermissions permissions)
        {
            IdentityUser user = await GetUser(id);

            if (user == null)
                return;

            List<string> roles = await GetRoles(id);

            foreach (var role in roles)
            {
                await _userManager.RemoveFromRoleAsync(user, role);
            }

            await AssignRole(user, permissions.IsSystemadmin, permissions.IsUser, permissions.IsScanCreator, permissions.IsScanDownloader, permissions.IsScanReader);
        }

        public async Task<bool> TryDelete(string id)
        {
            IdentityUser user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            // prevent deletion of systemadmin
            if (await _userManager.IsInRoleAsync(user, Roles.Systemadmin))
                return false;

            await _userManager.DeleteAsync(user);
            return true;
        }
    }
}
