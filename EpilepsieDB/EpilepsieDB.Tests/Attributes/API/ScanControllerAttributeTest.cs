using EpilepsieDB.Authorization;
using EpilepsieDB.Web.API.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static EpilepsieDB.Web.API.Controllers.ScansController;

namespace EpilepsieDB.Tests.Attributes.API
{
    public class ScanControllerAttributeTest : AbstractTest
    {
        public ScanControllerAttributeTest(ITestOutputHelper output) : base(output)
        {
        }


        [Fact]
        public void Has_AuthorizeAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<AuthorizeAttribute>(typeof(ScansController)));
        }

        [Fact]
        public void Has_JwtBearer()
        {
            string expected = JwtBearerDefaults.AuthenticationScheme;

            string actual = Helper.Helper.TypeHasAttributeValue<ScansController, AuthorizeAttribute, string>(attr => attr.AuthenticationSchemes);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Has_CorrectRoles()
        {
            string expected = RoleSet.AllowRead;

            string actual = Helper.Helper.TypeHasAttributeValue<ScansController, AuthorizeAttribute, string>(attr => attr.Roles);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Has_ApiControllerAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<ApiControllerAttribute>(typeof(ScansController)));
        }

        [Fact]
        public void Has_RouteAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<RouteAttribute>(typeof(ScansController)));
        }

        [Fact]
        public void Has_CorrectRoute()
        {
            string expected = "api/[controller]";

            string actual = Helper.Helper.TypeHasAttributeValue<ScansController, RouteAttribute, string>(attr => attr.Template);

            Assert.Equal(expected, actual);
        }
    }
}
