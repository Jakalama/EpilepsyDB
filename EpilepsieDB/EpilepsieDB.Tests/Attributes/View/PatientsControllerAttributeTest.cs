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
    public class PatientsControllerAttributeTest : AbstractTest
    {
        public PatientsControllerAttributeTest(ITestOutputHelper output) : base(output)
        {
        }


        [Fact]
        public void Has_AuthorizeAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<AuthorizeAttribute>(typeof(PatientsController)));
        }

        [Fact]
        public void Has_Cookie()
        {
            string expected = RoleSet.AllowRead;

            string actual = Helper.Helper.TypeHasAttributeValue<PatientsController, AuthorizeAttribute, string>(attr => attr.Roles);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Has_ApiControllerAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<ControllerAttribute>(typeof(PatientsController)));
        }

        [Fact]
        public void Has_RouteAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<RouteAttribute>(typeof(PatientsController)));
        }

        [Fact]
        public void Has_CorrectRoute()
        {
            string expected = "[controller]/[action]";

            string actual = Helper.Helper.TypeHasAttributeValue<PatientsController, RouteAttribute, string>(attr => attr.Template);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Has_CorrectApiExplorerSettings()
        {
            bool expected = true;

            bool actual = Helper.Helper.TypeHasAttributeValue<PatientsController, ApiExplorerSettingsAttribute, bool>(attr => attr.IgnoreApi);

            Assert.Equal(expected, actual);
        }
    }
}
