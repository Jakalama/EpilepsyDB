using EpilepsieDB.Authorization;
using EpilepsieDB.Services;
using EpilepsieDB.Services.Impl;
using EpilepsieDB.Tests.Helper;
using Microsoft.AspNetCore.Identity;
using Moq;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Services
{
    public class UsersServiceTest : AbstractTest
    {
        public UsersServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor()
        {
            // set
            var list = new List<IdentityUser>();
            var userManagerMock = MockUserManager<IdentityUser>(list);
            var roleManagerMock = MockRoleManager<IdentityRole>();

            // act
            new UsersService(userManagerMock.Object, roleManagerMock.Object);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task CheckPassword(bool expected)
        {
            // set
            var list = new List<IdentityUser>();
            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(expected);

            var service = new UsersService(userManagerMock.Object, null);

            // act
            var result = await service.CheckPassword(It.IsAny<IdentityUser>(), It.IsAny<string>());

            // assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task FindByName()
        {
            // set
            var name = "Test";
            var expected = new IdentityUser()
            {
                UserName = name,
            };
            var list = new List<IdentityUser>()
            {
                expected
            };
            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(
                    (string s) => 
                    {
                        return list.Where(u => u.UserName == s).FirstOrDefault(); 
                    });

            var service = new UsersService(userManagerMock.Object, null);

            // act
            var result = await service.FindByName(name);

            // assert
            Assert.Equal(expected, result);
            Assert.Equal(name, result.UserName);
        }

        [Fact]
        public async Task GetUser()
        {
            // set
            var user = new IdentityUser();
            var id = user.Id;

            var list = new List<IdentityUser>()
            {
                user
            };
            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(
                    (string s) =>
                    {
                        return list.Where(u => u.Id == s).FirstOrDefault();
                    });

            var service = new UsersService(userManagerMock.Object, null);

            // act
            var result = await service.GetUser(id);

            // assert
            Assert.Equal(user, result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetUsers()
        {
            // set
            var expected = 2;
            var list = new List<IdentityUser>()
            {
                new IdentityUser(),
                new IdentityUser()
            }.AsAsyncQueryable();

            var userManagerMock = MockUserManager<IdentityUser>(list.ToList());
            userManagerMock.Setup(m => m.Users)
                .Returns(list);

            var service = new UsersService(userManagerMock.Object, null);

            // act
            var result = await service.GetUsers();

            // assert
            Assert.Equal(expected, result.Count());
        }

        [Fact]
        public async Task TryDelete_ReturnsTrue_OnSuccess()
        {
            // set
            var user1 = new IdentityUser();
            var user2 = new IdentityUser();
            var id1 = user1.Id;
            var id2 = user1.Id;

            var list = new List<IdentityUser>()
            {
                user1, user2
            };
            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(
                    (string s) =>
                    {
                        return list.Where(u => u.Id == s).FirstOrDefault();
                    });
            userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            var service = new UsersService(userManagerMock.Object, null);

            // act
            var result = await service.TryDelete(id1);

            // assert
            Assert.True(result);
            Assert.Equal(1, list.Count());
            userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<IdentityUser>()), Times.Once());
        }

        [Fact]
        public async Task TryDelete_ReturnsFalse_IfUserNotExists()
        {
            // set
            var user = new IdentityUser();
            var id = user.Id;

            var list = new List<IdentityUser>()
            {
                user
            };
            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser) null);

            var service = new UsersService(userManagerMock.Object, null);

            // act
            var result = await service.TryDelete(id);

            // assert
            Assert.False(result);
            Assert.Equal(1, list.Count());
            userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<IdentityUser>()), Times.Never());
        }

        [Fact]
        public async Task TryDelete_ReturnsFalse_IfSystemadmin()
        {
            // set
            var user = new IdentityUser();
            var id = user.Id;

            var list = new List<IdentityUser>()
            {
                user
            };
            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            userManagerMock.Setup(m => m.IsInRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var service = new UsersService(userManagerMock.Object, null);

            // act
            var result = await service.TryDelete(id);

            // assert
            Assert.False(result);
            Assert.Equal(1, list.Count());
            userManagerMock.Verify(m => m.DeleteAsync(It.IsAny<IdentityUser>()), Times.Never());
        }

        [Fact]
        public async Task ConfirmEmail_ReturnsFailed_IfUserNotFound()
        {
            // set
            var list = new List<IdentityUser>();
            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser) null);

            var service = new UsersService(userManagerMock.Object, null);

            // act
            var result = await service.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>());

            // assert
            Assert.False(result.Succeeded);
            userManagerMock.Verify(m => m.ConfirmEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task ConfirmEmail_ReturnsSucceeded()
        {
            // set
            var user = new IdentityUser();
            var id = user.Id;

            var list = new List<IdentityUser>()
            {
                user
            };
            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            userManagerMock.Setup(m => m.ConfirmEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var service = new UsersService(userManagerMock.Object, null);

            // act
            var result = await service.ConfirmEmail(It.IsAny<string>(), It.IsAny<string>());

            // assert
            Assert.True(result.Succeeded);
            userManagerMock.Verify(m => m.ConfirmEmailAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once());
        }

        [Theory]
        [InlineData(true, false, false, false, false)]
        [InlineData(false, true, false, false, false)]
        [InlineData(false, false, true, false, false)]
        [InlineData(false, false, false, true, false)]
        [InlineData(false, false, false, false, true)]
        public async Task CreateNewUser_ReturnsUserResult_OnSuccess(bool admin, bool user, bool creator, bool downloader, bool reader)
        {
            // set
            var emailToken = "emailToken";
            var passwordToken = "passwordToken";
            var list = new List<IdentityUser>();

            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(emailToken);
            userManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(passwordToken);
            userManagerMock.Setup(m => m.AddToRoleAsync(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var invite = new UserInvite()
            {
                Email = "testMail",
                IsSystemadmin = admin,
                IsUser = user,
                IsScanCreator = creator,
                IsScanDownloader = downloader,
                IsScanReader = reader
            };

            var service = new UsersService(userManagerMock.Object, roleManagerMock.Object);

            // act
            var result = await service.CreateNewUser(invite);

            // assert
            Assert.NotNull(result);
            Assert.Equal(1, list.Count());
            userManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once());
            userManagerMock.Verify(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<IdentityUser>()), Times.Once());
            userManagerMock.Verify(m => m.GeneratePasswordResetTokenAsync(It.IsAny<IdentityUser>()), Times.Once());
        }

        [Theory]
        [InlineData(true, false, false, false, false)]
        [InlineData(false, true, false, false, false)]
        [InlineData(false, false, true, false, false)]
        [InlineData(false, false, false, true, false)]
        [InlineData(false, false, false, false, true)]
        public async Task CreateNewUser_ChecksIfRoleExists_OnSuccess(bool admin, bool user, bool creator, bool downloader, bool reader)
        {
            // set
            var emailToken = "emailToken";
            var passwordToken = "passwordToken";
            var list = new List<IdentityUser>();

            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(emailToken);
            userManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(passwordToken);
            userManagerMock.Setup(m => m.AddToRoleAsync(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var invite = new UserInvite()
            {
                Email = "testMail",
                IsSystemadmin = admin,
                IsUser = user,
                IsScanCreator = creator,
                IsScanDownloader = downloader,
                IsScanReader = reader
            };

            var service = new UsersService(userManagerMock.Object, roleManagerMock.Object);

            // act
            var result = await service.CreateNewUser(invite);

            // assert
            roleManagerMock.Verify(m => m.RoleExistsAsync(It.IsAny<string>()), Times.Once());
        }

        [Theory]
        [InlineData(true, false, false, false, false)]
        [InlineData(false, true, false, false, false)]
        [InlineData(false, false, true, false, false)]
        [InlineData(false, false, false, true, false)]
        [InlineData(false, false, false, false, true)]
        public async Task CreateNewUser_AddsRoleIfNotExists_OnSuccess(bool admin, bool user, bool creator, bool downloader, bool reader)
        {
            // set
            var emailToken = "emailToken";
            var passwordToken = "passwordToken";
            var list = new List<IdentityUser>();

            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(emailToken);
            userManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(passwordToken);
            userManagerMock.Setup(m => m.AddToRoleAsync(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);
            roleManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);

            var invite = new UserInvite()
            {
                Email = "testMail",
                IsSystemadmin = admin,
                IsUser = user,
                IsScanCreator = creator,
                IsScanDownloader = downloader,
                IsScanReader = reader
            };

            var service = new UsersService(userManagerMock.Object, roleManagerMock.Object);

            // act
            var result = await service.CreateNewUser(invite);

            // assert
            roleManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityRole>()), Times.Once());
        }

        [Fact]
        public async Task CreateNewUser_ReturnsNull_IfFailure()
        {
            // set
            var list = new List<IdentityUser>();
            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());

            var service = new UsersService(userManagerMock.Object, null);

            var invite = new UserInvite()
            {
                Email = "testMail",
                IsSystemadmin = true,
                IsUser = true,
                IsScanCreator = true,
                IsScanReader = true
            };

            // act
            var result = await service.CreateNewUser(invite);

            // assert
            Assert.Null(result);
            Assert.Equal(0, list.Count());
            userManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once());
            userManagerMock.Verify(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<IdentityUser>()), Times.Never());
            userManagerMock.Verify(m => m.GeneratePasswordResetTokenAsync(It.IsAny<IdentityUser>()), Times.Never());
        }

        [Theory]
        [InlineData(true, false, false, false, false, Roles.Systemadmin)]
        [InlineData(false, true, false, false, false, Roles.User)]
        [InlineData(false, false, true, false, false, Roles.Creator)]
        [InlineData(false, false, false, true, false, Roles.Downloader)]
        [InlineData(false, false, false, false, true, Roles.Reader)]
        public async Task CreateNewUser_AssignsCorrectRole_OnSuccess(bool admin, bool user, bool creator, bool downloader, bool reader, string expected)
        {
            // set
            var emailToken = "emailToken";
            var passwordToken = "passwordToken";
            var list = new List<IdentityUser>();
            var role = "";

            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.GenerateEmailConfirmationTokenAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(emailToken);
            userManagerMock.Setup(m => m.GeneratePasswordResetTokenAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(passwordToken);
            userManagerMock.Setup(m => m.AddToRoleAsync(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<IdentityUser, string>((i, r) => role = r);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var invite = new UserInvite()
            {
                Email = "testMail",
                IsSystemadmin = admin,
                IsUser = user,
                IsScanCreator = creator,
                IsScanDownloader = downloader,
                IsScanReader = reader
            };

            var service = new UsersService(userManagerMock.Object, roleManagerMock.Object);

            // act
            var result = await service.CreateNewUser(invite);

            // assert
            Assert.NotNull(result);
            Assert.Equal(expected, role);
        }

        [Fact]
        public async Task GetRoles_ReturnsEmptyListIfUserNotFound()
        {
            // set
            var list = new List<IdentityUser>();
            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser)null);

            var userService = new UsersService(userManagerMock.Object, null);

            // act
            var result = await userService.GetRoles(It.IsAny<string>());

            // assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count());
        }

        [Theory]
        [InlineData("test1")]
        [InlineData("test2")]
        public async Task GetRoles_ListOfRoles(string value)
        {
            // set
            var roles = new List<string>()
            {
                value
            };
            var list = new List<IdentityUser>();

            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser());
            userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(roles);

            var userService = new UsersService(userManagerMock.Object, null);

            // act
            var result = await userService.GetRoles(It.IsAny<string>());

            // assert
            Assert.NotNull(result);
            Assert.Equal(roles, result);
        }

        [Fact]
        public async Task ChangeRole_ReturnsWithoutChange_IfUserNotFound()
        {
            // set
            var list = new List<IdentityUser>();
            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser)null);

            var userService = new UsersService(userManagerMock.Object, null);

            // act
            await userService.ChangeRole(It.IsAny<string>(), It.IsAny<UserPermissions>());

            // assert
            userManagerMock.Verify(m => m.RemoveFromRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
            userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Never);
        }

        [Theory]
        [InlineData("test1", true, false, false, false, false)]
        [InlineData("test2", true, false, false, false, false)]
        public async Task ChangeRole_SetsNewRole(string value, bool admin, bool user, bool creator, bool downloader, bool reader)
        {
            // set
            var roles = new List<string>()
            {
                value
            };
            var list = new List<IdentityUser>();

            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser());
            userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(roles);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            UserPermissions permissions = new UserPermissions()
            {
                IsSystemadmin = admin,
                IsUser = user,
                IsScanCreator = creator,
                IsScanDownloader = downloader,
                IsScanReader = reader
            };

            var userService = new UsersService(userManagerMock.Object, roleManagerMock.Object);

            // act
            await userService.ChangeRole(It.IsAny<string>(), permissions);

            // assert
            userManagerMock.Verify(m => m.AddToRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once());
        }

        [Theory]
        [InlineData("test1", true, false, false, false, false)]
        [InlineData("test2", true, false, false, false, false)]
        public async Task ChangeRole_CreatesNewRole_IfNotExistendBefore(string value, bool admin, bool user, bool creator, bool downloader, bool reader)
        {
            // set
            var roles = new List<string>()
            {
                value
            };
            var list = new List<IdentityUser>();

            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser());
            userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(roles);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            UserPermissions permissions = new UserPermissions()
            {
                IsSystemadmin = admin,
                IsUser = user,
                IsScanCreator = creator,
                IsScanDownloader = downloader,
                IsScanReader = reader
            };

            var userService = new UsersService(userManagerMock.Object, roleManagerMock.Object);

            // act
            await userService.ChangeRole(It.IsAny<string>(), permissions);

            // assert
            roleManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityRole>()), Times.Once);
        }

        [Theory]
        [InlineData("test1", true, false, false, false, false)]
        [InlineData("test2", true, false, false, false, false)]
        public async Task ChangeRole_DeletesOldRole(string value, bool admin, bool user, bool creator, bool downloader, bool reader)
        {
            // set
            var roles = new List<string>()
            {
                value
            };
            var list = new List<IdentityUser>();

            var userManagerMock = MockUserManager<IdentityUser>(list);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser());
            userManagerMock.Setup(m => m.GetRolesAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(roles);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            UserPermissions permissions = new UserPermissions()
            {
                IsSystemadmin = admin,
                IsUser = user,
                IsScanCreator = creator,
                IsScanDownloader = downloader,
                IsScanReader = reader
            };


            var userService = new UsersService(userManagerMock.Object, roleManagerMock.Object);

            // act
            await userService.ChangeRole(It.IsAny<string>(), permissions);

            // assert
            userManagerMock.Verify(m => m.RemoveFromRoleAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()), Times.Once());
        }

        public Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> list) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<TUser>(x => list.Remove(x));
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<TUser, string>((x, y) => list.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>()))
                .ReturnsAsync(IdentityResult.Success);

            return mgr;
        }

        public Mock<RoleManager<T>> MockRoleManager<T>() where T : class
        {
            var store = new Mock<IRoleStore<T>>();
            var mgr = new Mock<RoleManager<T>>(store.Object, null, null, null, null);
            mgr.Object.RoleValidators.Add(new RoleValidator<T>());

            return mgr;
        }
    }
}
