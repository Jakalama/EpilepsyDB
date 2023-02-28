using EpilepsieDB.Authorization;
using EpilepsieDB.Web.View.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Attributes.View
{
    public class RecordingsControllerAttributeTest : AbstractTest
    {
        public RecordingsControllerAttributeTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Has_Cookie()
        {
            string expected = RoleSet.AllowRead;

            string actual = Helper.Helper.TypeHasAttributeValue<RecordingsController, AuthorizeAttribute, string>(attr => attr.Roles);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Has_ApiControllerAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<ControllerAttribute>(typeof(RecordingsController)));
        }

        [Fact]
        public void Has_RouteAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<RouteAttribute>(typeof(RecordingsController)));
        }

        [Fact]
        public void Has_CorrectRoute()
        {
            string expected = "[controller]/[action]";

            string actual = Helper.Helper.TypeHasAttributeValue<RecordingsController, RouteAttribute, string>(attr => attr.Template);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Has_CorrectApiExplorerSettings()
        {
            bool expected = true;

            bool actual = Helper.Helper.TypeHasAttributeValue<RecordingsController, ApiExplorerSettingsAttribute, bool>(attr => attr.IgnoreApi);

            Assert.Equal(expected, actual);
        }
    }
}
