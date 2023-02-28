using EpilepsieDB.Services;
using EpilepsieDB.Web.API.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.API.Controller
{
    public class UsersControllerTest : AbstractTest
    {
        public UsersControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CreateBearerToken()
        {
            // set
            var request = new UsersController.AuthenticationRequest()
            {
                Username = "admin",
                Password = "password",
            };
            var user = new IdentityUser()
            {
                UserName = "admin"
            };
            var expected = new AuthenticationResponse()
            {
                Token = "bnnsbnksnbksden",
                Expiration = DateTime.Now
            };

            var userServiceMock = new Mock<IUsersService>();
            userServiceMock.Setup(m => m.FindByName(It.IsAny<string>()))
                .ReturnsAsync(user);
            userServiceMock.Setup(m => m.CheckPassword(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(true);

            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock.Setup(m => m.CreateToken(It.IsAny<IdentityUser>(), It.IsAny<List<string>>()))
                .Returns(expected);

            var controller = new UsersController(userServiceMock.Object, tokenServiceMock.Object);

            // act
            var result = await controller.CreateBearerToken(request);

            // assert
            var actionResult = Assert.IsType<ActionResult<AuthenticationResponse>>(result);
            var data = Assert.IsType<AuthenticationResponse>(actionResult.Value);
            Assert.NotNull(data);
            Assert.Equal(expected, data);
        }

        [Fact]
        public async Task CreateBearerToken_ReturnsBadRequest_IfAuthentificationFails()
        {
            // set
            var request = new UsersController.AuthenticationRequest()
            {
                Username = "admin",
                Password = "password",
            };
            var user = new IdentityUser()
            {
                UserName = "admin"
            };

            var userServiceMock = new Mock<IUsersService>();
            userServiceMock.Setup(m => m.FindByName(It.IsAny<string>()))
                .ReturnsAsync(user);
            userServiceMock.Setup(m => m.CheckPassword(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(false);

            var controller = new UsersController(userServiceMock.Object, null);

            // act
            var result = await controller.CreateBearerToken(request);

            // assert
            var actionResult = Assert.IsType<ActionResult<AuthenticationResponse>>(result);
            var data = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal(400, data.StatusCode);
        }

        [Fact]
        public async Task CreateBearerToken_ReturnsBadRequest_IfUserNotFound()
        {
            // set
            var request = new UsersController.AuthenticationRequest()
            {
                Username = "admin",
                Password = "password",
            };

            var userServiceMock = new Mock<IUsersService>();
            userServiceMock.Setup(m => m.FindByName(It.IsAny<string>()))
                .ReturnsAsync((IdentityUser)null);
            userServiceMock.Setup(m => m.CheckPassword(
                It.IsAny<IdentityUser>(),
                It.IsAny<string>()))
                .ReturnsAsync(false);

            var controller = new UsersController(userServiceMock.Object, null);

            // act
            var result = await controller.CreateBearerToken(request);

            // assert
            var actionResult = Assert.IsType<ActionResult<AuthenticationResponse>>(result);
            var data = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal(400, data.StatusCode);
        }
    }
}
