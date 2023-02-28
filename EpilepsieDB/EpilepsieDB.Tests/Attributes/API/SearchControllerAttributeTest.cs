using EpilepsieDB.Authorization;
using EpilepsieDB.Web.API.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Attributes.API
{
    public class SearchControllerAttributeTest : AbstractTest
    {
        public SearchControllerAttributeTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Has_AuthorizeAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<AuthorizeAttribute>(typeof(SearchController)));
        }

        [Fact]
        public void Has_JwtBearer()
        {
            string expected = JwtBearerDefaults.AuthenticationScheme;

            string actual = Helper.Helper.TypeHasAttributeValue<SearchController, AuthorizeAttribute, string>(attr => attr.AuthenticationSchemes);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Has_CorrectRoles()
        {
            string expected = RoleSet.AllowRead;

            string actual = Helper.Helper.TypeHasAttributeValue<SearchController, AuthorizeAttribute, string>(attr => attr.Roles);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Has_ApiControllerAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<ApiControllerAttribute>(typeof(SearchController)));
        }

        [Fact]
        public void Has_RouteAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<RouteAttribute>(typeof(SearchController)));
        }

        [Fact]
        public void Has_CorrectRoute()
        {
            string expected = "api/[controller]";

            string actual = Helper.Helper.TypeHasAttributeValue<SearchController, RouteAttribute, string>(attr => attr.Template);

            Assert.Equal(expected, actual);
        }
    }
}
