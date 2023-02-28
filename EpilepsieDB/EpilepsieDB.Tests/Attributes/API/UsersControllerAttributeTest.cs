using EpilepsieDB.Web.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Attributes.API
{
    public class UsersControllerAttributeTest : AbstractTest
    {
        public UsersControllerAttributeTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Has_AllowAnonymousAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<AllowAnonymousAttribute>(typeof(UsersController)));
        }

        [Fact]
        public void Has_ApiControllerAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<ApiControllerAttribute>(typeof(UsersController)));
        }

        [Fact]
        public void Has_RouteAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<RouteAttribute>(typeof(UsersController)));
        }

        [Fact]
        public void Has_CorrectRoute()
        {
            string expected = "api/[controller]";

            string actual = Helper.Helper.TypeHasAttributeValue<UsersController, RouteAttribute, string>(attr => attr.Template);

            Assert.Equal(expected, actual);
        }
    }
}
