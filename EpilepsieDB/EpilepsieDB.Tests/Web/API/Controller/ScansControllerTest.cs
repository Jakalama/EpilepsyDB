using EpilepsieDB.Models;
using EpilepsieDB.Services;
using EpilepsieDB.Source;
using EpilepsieDB.Web.API.APIModels;
using EpilepsieDB.Web.API.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.API.Controller
{
    public class ScansControllerTest : AbstractTest
    {
        public ScansControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task GetScans()
        {
            // set
            var list = new List<Scan>()
            {
                new Scan()
            };
            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(list);

            var controller = new ScansController(serviceMock.Object);

            // act
            var result = await controller.GetScans();

            // assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ScanApiDto>>>(result);
            var data = Assert.IsType<List<ScanApiDto>>(((OkObjectResult)actionResult.Result).Value);
            Assert.NotNull(data);
            Assert.Single(data);
        }

        [Fact]
        public async Task GetScansOfPatient()
        {
            // set
            var list = new List<Scan>()
            {
                new Scan()
            };
            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(list);

            var controller = new ScansController(serviceMock.Object);

            // act
            var result = await controller.GetOfPatient(It.IsAny<int>());

            // assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ScanApiDto>>>(result);
            var data = Assert.IsType<List<ScanApiDto>>(((OkObjectResult)actionResult.Result).Value);
            Assert.NotNull(data);
            Assert.Single(data);
        }

        [Fact]
        public async Task GetScansOfRecording()
        {
            // set
            var list = new List<Scan>()
            {
                new Scan()
            };
            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(list);

            var controller = new ScansController(serviceMock.Object);

            // act
            var result = await controller.GetOfRecording(It.IsAny<int>());

            // assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<ScanApiDto>>>(result);
            var data = Assert.IsType<List<ScanApiDto>>(((OkObjectResult)actionResult.Result).Value);
            Assert.NotNull(data);
            Assert.Single(data);
        }

        [Fact]
        public async Task GetScan()
        {
            // set
            var list = new List<Scan>()
            {
                new Scan()
            };
            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(list);

            var controller = new ScansController(serviceMock.Object);

            // act
            var result = await controller.GetScan(It.IsAny<int>());

            // assert
            var actionResult = Assert.IsType<ActionResult<ScanApiDto>>(result);
            var data = Assert.IsType<ScanApiDto>(actionResult.Value);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task GetScan_ReturnsNotFound_IfScanNotFound()
        {
            // set
            var list = new List<Scan>();
            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(list);

            var controller = new ScansController(serviceMock.Object);

            // act
            var result = await controller.GetScan(It.IsAny<int>());

            // assert
            var actionResult = Assert.IsType<ActionResult<ScanApiDto>>(result);
            var data = Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task PostScan_ReturnsActionResult_WithCreatedAction()
        {
            // set
            var dto = new ScansController.CreateScanDto()
            {
                RecordingID = 1,
                ScanNumber = "Sc001",
                EdfFile = null,
            };
            Scan scan = null;

            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Create(
                It.IsAny<Scan>(),
                It.IsAny<IFileStream>()))
                .Callback<Scan, IFileStream>((s, f) => scan = s);

            var controller = new ScansController(serviceMock.Object);

            // act
            var result = await controller.PostScan(dto);

            // assert
            var actionResult = Assert.IsType<ActionResult<ScanApiDto>>(result);
            var data = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal("GetScan", data.ActionName);

            Assert.Equal(scan.RecordingID, dto.RecordingID);
            Assert.Equal(scan.ScanNumber, dto.ScanNumber);
        }
    }
}
