using EpilepsieDB.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using EpilepsieDB.Services;
using EpilepsieDB.Web.View.Extensions;
using EpilepsieDB.Web.View.ViewModels;

namespace EpilepsieDB.Web.View.Controllers
{
    [Controller]
    [Authorize(Roles = Roles.Systemadmin)]
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]", Order = 0)]
    public class UsersController : Controller
    {
        private readonly IUsersService _userService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public UsersController(
            IUsersService userService,
            IEmailService emailService,
            IConfiguration configuration) 
        {
            _userService = userService;
            _emailService = emailService;
            _configuration = configuration;

        }

        // GET: Users/
        public async Task<IActionResult> Index()
        {
            IEnumerable<IdentityUser> users = await _userService.GetUsers();

            return View(users.ToUserInfo());
        }

        // GET: Users/Invite
        public IActionResult Invite()
        {
            return View();
        }

        // POST: Users/Invite
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Invite([Bind("Email,IsSystemadmin,IsUser,IsScanCreator,IsScanDownloader,IsScanReader")] Invite dto)
        {
            if (ModelState.IsValid)
            {
                UserResult result = await _userService.CreateNewUser(dto.ToUserInvite());

                if (result == null)
                    return RedirectToAction(nameof(Index));

                var callbackUrl = Url.Action("ConfirmEmail", "Users", new { userId = result.UserID, emailConfirmationCode = result.EmailConfirmationCode, passwordSetCode = result.PasswordSetCode });

                string link = _configuration["APPLICATION_URL"] + callbackUrl;
                Console.WriteLine(link);

                await _emailService.SendConfirmationMail(dto.Email, link);

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        // Post: Users/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string emailConfirmationCode, string passwordSetCode = null)
        {
            if (userId == null || emailConfirmationCode == null)
                return View("Error");

            var result = await _userService.ConfirmEmail(userId, emailConfirmationCode);

            if (result.Succeeded && !string.IsNullOrEmpty(passwordSetCode))
            {
                string scheme = Request?.Scheme ?? "https";

                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code = passwordSetCode },
                    protocol: scheme);

                return Redirect(callbackUrl);
            }

            return View("Error");
        }

        // Get: Users/EditRole
        public async Task<IActionResult> EditRole(string id)
        {
            if (String.IsNullOrEmpty(id))
                return NotFound();

            List<string> roles = await _userService.GetRoles(id);

            return View(GetUserPermissions(id, roles));
        }

        // Post: Users/EditRole
        public async Task<IActionResult> EditRoleConfirmed(string userID, [Bind("UserID,IsSystemadmin,IsUser,IsScanCreator, IsScanDownloader,IsScanReader")] Permissions dto)
        {
            if (String.IsNullOrEmpty(userID))
                return NotFound();

            if (ModelState.IsValid)
            {
                UserPermissions userPermissions = dto.ToUserPermissions();

                await _userService.ChangeRole(userID, userPermissions);

                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }


        // Get: Users/Delete
        public async Task<IActionResult> Delete(string id)
        {
            if (String.IsNullOrEmpty(id))
                return NotFound();

            IdentityUser user = await _userService.GetUser(id);

            if (user == null)
                return NotFound();

            return View(user.ToUserInfo());
        }

        // Post: Users/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (String.IsNullOrEmpty(id))
                return NotFound();

            bool success = await _userService.TryDelete(id);

            if (!success)
                return Forbid();

            return RedirectToAction(nameof(Index));
        }

        private Permissions GetUserPermissions(string id, List<string> roles)
        {
            Permissions permissions = new Permissions();

            permissions.UserID = id;
            permissions.IsSystemadmin = roles.Contains(Roles.Systemadmin);
            permissions.IsUser = roles.Contains(Roles.User);
            permissions.IsScanCreator = roles.Contains(Roles.Creator);
            permissions.IsScanDownloader = roles.Contains(Roles.Downloader);
            permissions.IsScanReader = roles.Contains(Roles.Reader);

            return permissions;
        }
    }
}
