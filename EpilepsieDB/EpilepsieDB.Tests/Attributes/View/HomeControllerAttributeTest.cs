using EpilepsieDB.Web.View.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Attributes.View
{
    public class HomeControllerAttributeTest : AbstractTest
    {
        public HomeControllerAttributeTest(ITestOutputHelper output) : base(output)
        {
        }


        [Fact]
        public void Has_AllowAnonymousAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<AllowAnonymousAttribute>(typeof(HomeController)));
        }

        [Fact]
        public void Has_ControllerAttribute()
        {
            Assert.True(Helper.Helper.TypeHasAttribute<ControllerAttribute>(typeof(HomeController)));
        }

        [Fact]
        public void Has_CorrectApiExplorerSettings()
        {
            bool expected = true;

            bool actual = Helper.Helper.TypeHasAttributeValue<HomeController, ApiExplorerSettingsAttribute, bool>(attr => attr.IgnoreApi);

            Assert.Equal(expected, actual);
        }
    }
}
