using EpilepsieDB.Models;
using EpilepsieDB.Services;
using EpilepsieDB.Web.API.APIModels;
using EpilepsieDB.Web.API.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.API.Controller
{
    public class SearchControllerTest : AbstractTest
    {
        public SearchControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Get()
        {
            // set
            var list = new List<Scan>()
            {
                new Scan(),
            };

            var serviceMock = new Mock<ISearchService>();
            serviceMock.Setup(m => m.Search(It.IsAny<SearchQuery>()))
                .ReturnsAsync(list);

            var controller = new SearchController(serviceMock.Object);

            // act
            var result = await controller.Get();

            // assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ScanApiDto>>>(result);
            var data = Assert.IsType<List<ScanApiDto>>(((OkObjectResult)actionResult.Result).Value);
            Assert.NotNull(data);
            Assert.Single(data);
        }

        [Fact]
        public async Task Get_QueryIsSetCorrect()
        {
            // set
            var s1 = "1";
            var s2 = "2";
            var s3 = "3";
            var s4 = "4";
            var s5 = "5";
            var s6 = "6";
            var s7 = "7";
            var s8 = "8";
            var s9 = "9";
            var s10 = "10";
            var s11 = "11";
            var s12 = "12";
            
            var list = new List<Scan>()
            {
                new Scan(),
            };
            SearchQuery query = null;

            var serviceMock = new Mock<ISearchService>();
            serviceMock.Setup(m => m.Search(It.IsAny<SearchQuery>()))
                .ReturnsAsync(list)
                .Callback<SearchQuery>(q => query = q);

            var controller = new SearchController(serviceMock.Object);

            // act
            var result = await controller.Get(s1, s2, s3, s4, s5, s6, s7, s8, s9, s10, s11, s12);

            // assert
            Assert.NotNull(query);
            Assert.Equal(s1, query.Name);
            Assert.Equal(s2, query.Rec);
            Assert.Equal(s3, query.Scan);
            Assert.Equal(s4, query.Ver);
            Assert.Equal(s5, query.PInfo);
            Assert.Equal(s6, query.RecInfo);
            Assert.Equal(s7, query.Date);
            Assert.Equal(s8, query.Time);
            Assert.Equal(s9, query.Label);
            Assert.Equal(s10, query.Type);
            Assert.Equal(s11, query.Dim);
            Assert.Equal(s12, query.Annot);
        }
    }
}
