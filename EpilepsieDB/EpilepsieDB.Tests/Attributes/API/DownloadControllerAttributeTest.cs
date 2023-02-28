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

namespace EpilepsieDB.Tests.Attributes.API
{
    public class DownloadControllerAttributeTest : AbstractTest
    {
        public DownloadControllerAttributeTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Has_AuthorizeAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<AuthorizeAttribute>(typeof(DownloadController)));
        }

        [Fact]
        public void Has_JetBearer()
        {
            string expected = JwtBearerDefaults.AuthenticationScheme;

            string actual = Helper.Helper.TypeHasAttributeValue<DownloadController, AuthorizeAttribute, string>(attr => attr.AuthenticationSchemes);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Has_CorrectRole()
        {
            string expected = RoleSet.AllowDownload;

            string actual = Helper.Helper.TypeHasAttributeValue<DownloadController, AuthorizeAttribute, string>(attr => attr.Roles);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Has_ApiControllerAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<ApiControllerAttribute>(typeof(DownloadController)));
        }

        [Fact]
        public void Has_RouteAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<RouteAttribute>(typeof(DownloadController)));
        }

        [Fact]
        public void Has_CorrectRoute()
        {
            string expected = "api/[controller]";

            string actual = Helper.Helper.TypeHasAttributeValue<DownloadController, RouteAttribute, string>(attr => attr.Template);

            Assert.Equal(expected, actual);
        }
    }
}
