using EpilepsieDB.Authorization;
using EpilepsieDB.Web.View.Controllers;
using EpilepsieDB.Web.View.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.View.Controller
{
    public class HomeControllerTest : AbstractTest
    {
        public HomeControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Index()
        {
            // set
            var controller = new HomeController(null);

            // act
            var result = controller.Index();

            // assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);
            var controllerName = Assert.IsAssignableFrom<string>(viewResult.ControllerName);
            Assert.Equal("Index", actionName);
            Assert.Equal("Search", controllerName);
        }

        [Fact]
        public void Privacy()
        {
            // set
            var controller = new HomeController(null);

            // act
            var result = controller.Privacy();

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error()
        {
            // set
            var controller = new HomeController(null);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
            };

            // act
            var result = controller.Error();

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ErrorViewModel>(viewResult.ViewData.Model);
        }
    }
}
