using EpilepsieDB.Data;
using EpilepsieDB.Source.Wrapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Data
{
    public class SeedUserTest : AbstractTest
    {
        public SeedUserTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task SetSystemadmin_AddsUser()
        {
            // set
            IdentityUser storedUser = null;

            var userManagerMock = MockUserManager<IdentityUser>();
            userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser) null);
            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<IdentityUser, string>((u, s) => storedUser = u);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser());
            userManagerMock.Setup(m => m.AddToRoleAsync(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);


            var serviceMock = new Mock<IServiceProviderWrapper>();
            serviceMock.SetupSequence(m => m.GetService<UserManager<IdentityUser>>())
                .Returns(userManagerMock.Object)
                .Returns(userManagerMock.Object);
            serviceMock.Setup(m => m.GetService<RoleManager<IdentityRole>>())
                .Returns(roleManagerMock.Object);

            // act
            await SeedUser.SetSystemadmin(serviceMock.Object, "test@epilepsie.de", "strongPW_123");

            // assert
            Assert.NotNull(storedUser);
        }

        [Fact]
        public async Task SetSystemadmin_ThrowError_IfUserManagerIsNull()
        {
            // set
            var serviceMock = new Mock<IServiceProviderWrapper>();
            serviceMock.SetupSequence(m => m.GetService<UserManager<IdentityUser>>())
                .Returns((UserManager<IdentityUser>) null);

            // act
            await Assert.ThrowsAsync<Exception>(async () => await SeedUser.SetSystemadmin(serviceMock.Object, "test@epilepsie.de", "strongPW_123"));
        }

        [Fact]
        public async Task SetSystemadmin_ThrowError_IfUserCantBeCreated()
        {
            // set
            var userManagerMock = MockUserManager<IdentityUser>();
            userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser)null);
            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser) null);
            userManagerMock.Setup(m => m.AddToRoleAsync(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);


            var serviceMock = new Mock<IServiceProviderWrapper>();
            serviceMock.SetupSequence(m => m.GetService<UserManager<IdentityUser>>())
                .Returns(userManagerMock.Object)
                .Returns(userManagerMock.Object);
            serviceMock.Setup(m => m.GetService<RoleManager<IdentityRole>>())
                .Returns(roleManagerMock.Object);

            // act
            await Assert.ThrowsAsync<Exception>(async () => await SeedUser.SetSystemadmin(serviceMock.Object, "test@epilepsie.de", "strongPW_123"));
        }

        [Fact]
        public async Task SetSystemadmin_ThrowsError_IfUserCantBeFound()
        {
            // set
            IdentityUser storedUser = null;

            var userManagerMock = MockUserManager<IdentityUser>();
            userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser)null);
            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<IdentityUser, string>((u, s) => storedUser = u);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser)null);
            userManagerMock.Setup(m => m.AddToRoleAsync(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);


            var serviceMock = new Mock<IServiceProviderWrapper>();
            serviceMock.SetupSequence(m => m.GetService<UserManager<IdentityUser>>())
                .Returns(userManagerMock.Object)
                .Returns(userManagerMock.Object);
            serviceMock.Setup(m => m.GetService<RoleManager<IdentityRole>>())
                .Returns(roleManagerMock.Object);

            // act
            await Assert.ThrowsAsync<Exception>(async () => await SeedUser.SetSystemadmin(serviceMock.Object, "test@epilepsie.de", "strongPW_123"));
        }

        [Fact]
        public async Task SetSystemadmin_CreatesRoleIfNotExistentYet()
        {
            // set
            IdentityUser storedUser = null;

            var userManagerMock = MockUserManager<IdentityUser>();
            userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser)null);
            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<IdentityUser, string>((u, s) => storedUser = u);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser());
            userManagerMock.Setup(m => m.AddToRoleAsync(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);
            roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(false);


            var serviceMock = new Mock<IServiceProviderWrapper>();
            serviceMock.SetupSequence(m => m.GetService<UserManager<IdentityUser>>())
                .Returns(userManagerMock.Object)
                .Returns(userManagerMock.Object);
            serviceMock.Setup(m => m.GetService<RoleManager<IdentityRole>>())
                .Returns(roleManagerMock.Object);

            // act
            await SeedUser.SetSystemadmin(serviceMock.Object, "test@epilepsie.de", "strongPW_123");

            // assert
            Assert.NotNull(storedUser);
            roleManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityRole>()), Times.Once());
        }

        [Fact]
        public async Task SetSystemadmin_CreatesNoRoleIfRoleExists()
        {
            // set
            IdentityUser storedUser = null;

            var userManagerMock = MockUserManager<IdentityUser>();
            userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser)null);
            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<IdentityUser, string>((u, s) => storedUser = u);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser());
            userManagerMock.Setup(m => m.AddToRoleAsync(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);
            roleManagerMock.Setup(m => m.RoleExistsAsync(It.IsAny<string>()))
                .ReturnsAsync(true);


            var serviceMock = new Mock<IServiceProviderWrapper>();
            serviceMock.SetupSequence(m => m.GetService<UserManager<IdentityUser>>())
                .Returns(userManagerMock.Object)
                .Returns(userManagerMock.Object);
            serviceMock.Setup(m => m.GetService<RoleManager<IdentityRole>>())
                .Returns(roleManagerMock.Object);

            // act
            await SeedUser.SetSystemadmin(serviceMock.Object, "test@epilepsie.de", "strongPW_123");

            // assert
            Assert.NotNull(storedUser);
            roleManagerMock.Verify(m => m.CreateAsync(It.IsAny<IdentityRole>()), Times.Never());
        }

        [Fact]
        public async Task SetSystemadmin_ThrowsExceptionIfRoleManagerIsNull()
        {
            // set
            IdentityUser storedUser = null;

            var userManagerMock = MockUserManager<IdentityUser>();
            userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser)null);
            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<IdentityUser, string>((u, s) => storedUser = u);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser)null);
            userManagerMock.Setup(m => m.AddToRoleAsync(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var serviceMock = new Mock<IServiceProviderWrapper>();
            serviceMock.SetupSequence(m => m.GetService<UserManager<IdentityUser>>())
                .Returns(userManagerMock.Object);
            serviceMock.Setup(m => m.GetService<RoleManager<IdentityRole>>())
                .Returns((RoleManager<IdentityRole>) null);

            // act
            await Assert.ThrowsAsync<Exception>(async () => await SeedUser.SetSystemadmin(serviceMock.Object, "test@epilepsie.de", "strongPW_123"));
        }

        [Fact]
        public async Task SetUser_AddsUser()
        {
            // set
            IdentityUser storedUser = null;

            var userManagerMock = MockUserManager<IdentityUser>();
            userManagerMock.Setup(m => m.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser)null);
            userManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<IdentityUser, string>((u, s) => storedUser = u);
            userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new IdentityUser());
            userManagerMock.Setup(m => m.AddToRoleAsync(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var roleManagerMock = MockRoleManager<IdentityRole>();
            roleManagerMock.Setup(m => m.CreateAsync(It.IsAny<IdentityRole>()))
                .ReturnsAsync(IdentityResult.Success);


            var serviceMock = new Mock<IServiceProviderWrapper>();
            serviceMock.SetupSequence(m => m.GetService<UserManager<IdentityUser>>())
                .Returns(userManagerMock.Object)
                .Returns(userManagerMock.Object);
            serviceMock.Setup(m => m.GetService<RoleManager<IdentityRole>>())
                .Returns(roleManagerMock.Object);

            // act
            await SeedUser.SetUser(serviceMock.Object, "test@epilepsie.de", "strongPW_123");

            // assert
            Assert.NotNull(storedUser);
        }


        public static Mock<UserManager<T>> MockUserManager<T>() where T : class
        {
            var store = new Mock<IUserStore<T>>();
            var mgr = new Mock<UserManager<T>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<T>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<T>());

            return mgr;
        }

        public static Mock<RoleManager<T>> MockRoleManager<T>() where T : class
        {
            var store = new Mock<IRoleStore<T>>();
            var mgr = new Mock<RoleManager<T>>(store.Object, null, null, null, null);
            mgr.Object.RoleValidators.Add(new RoleValidator<T>());
            
            return mgr;
        }
    }
}
