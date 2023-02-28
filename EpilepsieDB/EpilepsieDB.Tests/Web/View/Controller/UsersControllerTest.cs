using EpilepsieDB.Authorization;
using EpilepsieDB.Services;
using EpilepsieDB.Web.View.Controllers;
using EpilepsieDB.Web.View.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.View.Controller
{
    public class UsersControllerTest : AbstractTest
    {
        public UsersControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Index_ReturnsIActionResult_WithUserInfos()
        {
            // set
            var list = new List<IdentityUser>()
            {
                new IdentityUser(),
                new IdentityUser(),
            };

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(m => m.GetUsers())
                .ReturnsAsync(list);
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);
            
            // act
            var result = await controller.Index();

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<UserInfo>>(viewResult.ViewData.Model);
            Assert.Equal(list.Count(), model.Count());
        }

        [Fact]
        public async Task InviteGET_ReturnsIActionResult()
        {
            // set
            var usersServiceMock = new Mock<IUsersService>();
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = controller.Invite();

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Invite_ReturnsRedirectToIndex_IfSuccessfull()
        {
            // set
            var dto = new Invite()
            {
                Email = "test",
            };
            var userResult = new UserResult()
            {
                UserID = "test",
                EmailConfirmationCode = "test",
                PasswordSetCode = "test",
            };

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(m => m.CreateNewUser(It.IsAny<UserInvite>()))
                .ReturnsAsync(userResult);

            var emailServiceMock = new Mock<IEmailService>();
            emailServiceMock.Setup(m => m.SendConfirmationMail(It.IsAny<string>(), It.IsAny<string>()));

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(m => m["APPLICATION_URL"])
                .Returns("url");

            var urlHelperMock = new Mock<IUrlHelper>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);
            controller.Url = urlHelperMock.Object;

            // act
            var result = await controller.Invite(dto);

            // assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);
            Assert.Equal(nameof(controller.Index), actionName);

            usersServiceMock.Verify(m => m.CreateNewUser(It.IsAny<UserInvite>()), Times.Once());
            emailServiceMock.Verify(m => m.SendConfirmationMail(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Invite_ReturnsRedirectToIndex_IfUserCantbeCreated()
        {
            // set
            var dto = new Invite()
            {
                Email = "test",
            };

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(m => m.CreateNewUser(It.IsAny<UserInvite>()))
                .ReturnsAsync((UserResult) null);

            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.Invite(dto);

            // assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);
            Assert.Equal(nameof(controller.Index), actionName);
        }

        [Fact]
        public async Task Invite_ReturnsRedirectToIndex_IfNotSuccessfull()
        {
            // set
            var dto = new Invite();

            var usersServiceMock = new Mock<IUsersService>();
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);
            controller.ModelState.AddModelError("Email", "Email field is needed!");

            // act
            var result = await controller.Invite(dto);

            // assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);
            Assert.Equal(nameof(controller.Index), actionName);
        }

        [Fact]
        public async Task ConfirmEmail_ReturnsRedirect_WithCallbackUrl()
        {
            // set
            var userID = "userID";
            var emailCode = "email";
            var passwordCode = "password";

            var identityResult = IdentityResult.Success;

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(m => m.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(identityResult);

            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();
            var urlHelperMock = CreateUrlHelperMock();
            urlHelperMock.Setup(m => m.RouteUrl(It.IsAny<UrlRouteContext>()))
                .Returns("blub");

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };
            controller.Url = urlHelperMock.Object;

            // act
            var result = await controller.ConfirmEmail(userID, emailCode, passwordCode);

            // assert
            var viewResult = Assert.IsType<RedirectResult>(result);
            var callbackUrl = Assert.IsAssignableFrom<string>(viewResult.Url);
            Assert.NotNull(callbackUrl);
        }

        [Fact]
        public async Task ConfirmEmail_ReturnsError_IfUserIdIsNull()
        {
            // set
            string userID = null;
            var emailCode = "email";
            var passwordCode = "password";

            var identityResult = IdentityResult.Success;

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(m => m.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(identityResult);

            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();
            var urlHelperMock = new Mock<IUrlHelper>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);
            controller.Url = urlHelperMock.Object;

            // act
            var result = await controller.ConfirmEmail(userID, emailCode, passwordCode);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
        }

        [Fact]
        public async Task ConfirmEmail_ReturnsError_IfEmailCodeIsNull()
        {
            // set
            var userID = "userID";
            string emailCode = null;
            var passwordCode = "password";

            var identityResult = IdentityResult.Success;

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(m => m.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(identityResult);

            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();
            var urlHelperMock = new Mock<IUrlHelper>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);
            controller.Url = urlHelperMock.Object;

            // act
            var result = await controller.ConfirmEmail(userID, emailCode, passwordCode);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
        }

        [Fact]
        public async Task ConfirmEmail_ReturnsError_IfResultFailed()
        {
            // set
            var userID = "userID";
            var emailCode = "email";
            var passwordCode = "password";

            var identityResult = IdentityResult.Failed();

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(m => m.ConfirmEmail(
                It.IsAny<string>(),
                It.IsAny<string>()))
                .ReturnsAsync(identityResult);

            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();
            var urlHelperMock = new Mock<IUrlHelper>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);
            controller.Url = urlHelperMock.Object;

            // act
            var result = await controller.ConfirmEmail(userID, emailCode, passwordCode);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
        }

        [Fact]
        public async Task DeleteGET_ReturnsNotFound_IfIdIsNull()
        {
            // set
            var usersServiceMock = new Mock<IUsersService>();
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.Delete(null);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteGET_ReturnsNotFound_IfIdIsEmpty()
        {
            // set
            var usersServiceMock = new Mock<IUsersService>();
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.Delete("");

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteGET_ReturnsNotFound_IfUserNotFound()
        {
            // set
            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(m => m.GetUser(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser) null);
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.Delete("userID");

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteGET_ReturnsViewResult_IfSuccess()
        {
            // set
            var user = new IdentityUser()
            {
                Email = "test",
            };

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(m => m.GetUser(It.IsAny<string>()))
                .ReturnsAsync(user);
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.Delete(user.Id);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<UserInfo>(viewResult.ViewData.Model);
            Assert.Equal(user.Id, model.ID);
            Assert.Equal(user.Email, model.Email);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsNotFound_IfIsNull()
        {
            // set
            var usersServiceMock = new Mock<IUsersService>();
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.DeleteConfirmed(null);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsNotFound_IfIsEmpty()
        {
            // set
            var usersServiceMock = new Mock<IUsersService>();
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.DeleteConfirmed("");

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsFordbid_IfUserDeleteFails()
        {
            // set
            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(m => m.TryDelete(It.IsAny<string>()))
                .ReturnsAsync(false);
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.DeleteConfirmed("userID");

            // assert
            var viewResult = Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToAction_IfSuccess()
        {
            // set
            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(m => m.TryDelete(It.IsAny<string>()))
                .ReturnsAsync(true);
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.DeleteConfirmed("userID");

            // assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(redirectResult.ActionName);
            Assert.Equal(nameof(controller.Index), actionName);
        }

        [Fact]
        public async Task EditRoleGET_ReturnsNotFound_IfNull()
        {
            // set
            var usersServiceMock = new Mock<IUsersService>();
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.EditRole(null);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditRoleGET_ReturnsNotFound_IfEmpty()
        {
            // set
            var usersServiceMock = new Mock<IUsersService>();
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.EditRole("");

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Theory]
        [InlineData(Roles.Systemadmin)]
        [InlineData(Roles.Reader)]
        public async Task EditRoleGET_ReturnsIActionResult(string value)
        {
            // set
            var list = new List<string>()
            {
                value
            };
            var userID = "123";

            var usersServiceMock = new Mock<IUsersService>();
            usersServiceMock.Setup(m => m.GetRoles(It.IsAny<string>()))
                .ReturnsAsync(list);

            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.EditRole(userID);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task EditRoleConfirmed_ReturnsNotFound_IfNull()
        {
            // set
            var usersServiceMock = new Mock<IUsersService>();
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.EditRoleConfirmed(null, It.IsAny<Permissions>());

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditRoleConfirmed_ReturnsNotFound_IfEmpty()
        {
            // set
            var usersServiceMock = new Mock<IUsersService>();
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            // act
            var result = await controller.EditRoleConfirmed("", It.IsAny<Permissions>());

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditRoleConfirmed_ReturnsRedirectToAction_IfSuccess()
        {
            // set
            var userID = "123";

            var usersServiceMock = new Mock<IUsersService>();
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);

            Permissions dto = new Permissions();

            // act
            var result = await controller.EditRoleConfirmed(userID, dto);

            // assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);
            Assert.Equal(nameof(controller.Index), actionName);
        }

        [Fact]
        public async Task EditRoleConfirmed_ReturnsViewResult_IfModelInvalid()
        {
            // set
            var userID = "123";

            var usersServiceMock = new Mock<IUsersService>();
            var emailServiceMock = new Mock<IEmailService>();
            var configurationMock = new Mock<IConfiguration>();

            var controller = new UsersController(usersServiceMock.Object, emailServiceMock.Object, configurationMock.Object);
            controller.ModelState.AddModelError("Bool", "All bool must be set!");

            Permissions dto = new Permissions();

            // act
            var result = await controller.EditRoleConfirmed(userID, dto);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Permissions>(viewResult.ViewData.Model);
            Assert.Equal(dto, model);
        }

        // idea from: https://edi.wang/post/2021/4/26/aspnet-core-unit-test-how-to-mock-urlpage
        private Mock<IUrlHelper> CreateUrlHelperMock(ActionContext context = null)
        {
            context ??= GetActionContextForPage("/Page");

            var urlHelper = new Mock<IUrlHelper>();
            urlHelper.SetupGet(h => h.ActionContext)
                .Returns(context);
            return urlHelper;
        }

        private static ActionContext GetActionContextForPage(string page)
        {
            return new()
            {
                ActionDescriptor = new()
                {
                    RouteValues = new Dictionary<string, string>
            {
                { "page", page },
            }
                },
                RouteData = new()
                {
                    Values =
            {
                [ "page" ] = page
            }
                }
            };
        }
    }
}
