using EpilepsieDB.Authorization;
using EpilepsieDB.Models;
using EpilepsieDB.Services;
using EpilepsieDB.Web.View.Controllers;
using EpilepsieDB.Web.View.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.View.Controller
{
    public class SearchControllerTest : AbstractTest
    {
        public SearchControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithSearchResult()
        {
            // set
            var serviceMock = new Mock<ISearchService>();
            serviceMock.Setup(m => m.Search(It.IsAny<SearchQuery>()))
                .ReturnsAsync(new List<Scan>());
            var controller = new SearchController(serviceMock.Object);

            // act
            var result = await controller.Index("", "", 1);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SearchResult>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithCorrectData()
        {
            // set
            var patient1 = new Patient();
            var patient2 = new Patient();
            var recording1 = new Recording()
            {
                Patient = patient1,
            };
            var recording2 = new Recording()
            {
                Patient = patient2,
            };
            var recording3 = new Recording()
            {
                Patient = patient2,
            };
            var list = new List<Scan>()
            {
                new Scan()
                {
                    Recording = recording1,
                },
                new Scan()
                {
                    Recording = recording1
                },
                new Scan()
                {
                    Recording = recording2
                },
                new Scan()
                {
                    Recording = recording3
                }
            };

            var serviceMock = new Mock<ISearchService>();
            serviceMock.Setup(m => m.Search(It.IsAny<SearchQuery>()))
                .ReturnsAsync(list);
            var controller = new SearchController(serviceMock.Object);

            // act
            var result = await controller.Index("", "", 1);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<SearchResult>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Results.Keys.Count);
            Assert.Equal(1, model.Results[patient1].Count);
            Assert.Equal(2, model.Results[patient2].Count);
            Assert.Equal(2, model.Results[patient1][recording1].Count());
            Assert.Equal(1, model.Results[patient2][recording2].Count());
            Assert.Equal(1, model.Results[patient2][recording3].Count());
        }

        [Fact]
        public async Task Index_SetsSearchStringIntoViewData()
        {
            // set
            var expected = "test";

            var serviceMock = new Mock<ISearchService>();
            serviceMock.Setup(m => m.Search(It.IsAny<SearchQuery>()))
                .ReturnsAsync(new List<Scan>());
            var controller = new SearchController(serviceMock.Object);

            // act
            var result = await controller.Index("", expected, 1);

            // assert
            Assert.Equal(expected, controller.ViewData["CurrentFilter"]);
        }

        [Fact]
        public async Task Index_SearchStringGetsTrimmed()
        {
            // set
            var expected = "  test  ";

            var serviceMock = new Mock<ISearchService>();
            serviceMock.Setup(m => m.Search(It.IsAny<SearchQuery>()))
                .ReturnsAsync(new List<Scan>());
            var controller = new SearchController(serviceMock.Object);

            // act
            var result = await controller.Index("", expected, 1);

            // assert
            Assert.Equal(expected.Trim(), controller.ViewData["CurrentFilter"]);
        }

        [Fact]
        public async Task Index_SetsSearchStringToCurrentFilter_IfSearchStringIsNull()
        {
            // set
            var expected = "test";

            var serviceMock = new Mock<ISearchService>();
            serviceMock.Setup(m => m.Search(It.IsAny<SearchQuery>()))
                .ReturnsAsync(new List<Scan>());
            var controller = new SearchController(serviceMock.Object);

            // act
            var result = await controller.Index(expected, null, 1);

            // assert
            Assert.Equal(expected, controller.ViewData["CurrentFilter"]);
        }
    }
}
