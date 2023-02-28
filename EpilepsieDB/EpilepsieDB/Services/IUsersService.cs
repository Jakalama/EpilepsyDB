using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EpilepsieDB.Services
{

    public class UserResult
    {
        public string UserID { get; set; }
        public string EmailConfirmationCode { get; set; }
        public string PasswordSetCode { get; set; }
    }

    public class UserInvite
    {
        public string Email { get; set; }
        public bool IsSystemadmin { get; set; }
        public bool IsUser { get; set; }
        public bool IsScanCreator { get; set; }
        public bool IsScanDownloader { get; set; }
        public bool IsScanReader { get; set; }
    }

    public class UserPermissions
    {
        public bool IsSystemadmin { get; set; }
        public bool IsUser { get; set; }
        public bool IsScanCreator { get; set; }
        public bool IsScanDownloader { get; set; }
        public bool IsScanReader { get; set; }
    }

    public interface IUsersService
    {
        Task<IdentityUser> GetUser(string id);
        Task<IEnumerable<IdentityUser>> GetUsers();
        Task<List<string>> GetRoles(string id);
        Task<IdentityUser> FindByName(string username);
        Task<bool> TryDelete(string id);
        Task<UserResult> CreateNewUser(UserInvite invite);
        Task ChangeRole(string id, UserPermissions permissions);
        Task<IdentityResult> ConfirmEmail(string userID, string emailConfirmationCode);
        Task<bool> CheckPassword(IdentityUser user, string password);
    }
}
