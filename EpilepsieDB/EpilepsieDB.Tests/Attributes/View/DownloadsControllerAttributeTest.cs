using EpilepsieDB.Authorization;
using EpilepsieDB.Web.View.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Attributes.View
{
    public class DownloadsControllerAttributeTest : AbstractTest
    {
        public DownloadsControllerAttributeTest(ITestOutputHelper output) : base(output)
        {
        }


        [Fact]
        public void Has_Cookie()
        {
            string expected = RoleSet.AllowDownload;

            string actual = Helper.Helper.TypeHasAttributeValue<DownloadsController, AuthorizeAttribute, string>(attr => attr.Roles);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Has_ApiControllerAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<ControllerAttribute>(typeof(DownloadsController)));
        }

        [Fact]
        public void Has_RouteAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<RouteAttribute>(typeof(DownloadsController)));
        }

        [Fact]
        public void Has_CorrectRoute()
        {
            string expected = "[controller]/[action]";

            string actual = Helper.Helper.TypeHasAttributeValue<DownloadsController, RouteAttribute, string>(attr => attr.Template);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Has_CorrectApiExplorerSettings()
        {
            bool expected = true;

            bool actual = Helper.Helper.TypeHasAttributeValue<DownloadsController, ApiExplorerSettingsAttribute, bool>(attr => attr.IgnoreApi);

            Assert.Equal(expected, actual);
        }
    }
}
