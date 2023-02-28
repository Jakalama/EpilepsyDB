using EpilepsieDB.Web.View.Controllers;
using EpilepsieDB.Models;
using EpilepsieDB.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using System.Linq.Expressions;
using EpilepsieDB.Web.View.ViewModels;
using Microsoft.AspNetCore.Http;
using EpilepsieDB.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace EpilepsieDB.Tests.Web.View.Controller
{
    public class ScanControllerTest : AbstractTest
    {
        public ScanControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithScanDetailDto()
        {
            // set
            Scan scan = new Scan()
            {
                ID = 1
            };
            var scans = new List<Scan>()
            {
                scan
            };

            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(scans);

            var signalService = new Mock<ISignalService>();
            signalService.Setup(m => m.Get(
                It.IsAny<Expression<Func<Signal, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(new List<Signal>());

            var blockService = new Mock<IBlockService>();
            blockService.Setup(m => m.Get(
                It.IsAny<Expression<Func<Block, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(new List<Block>());

            var controller = new ScansController(serviceMock.Object, blockService.Object, signalService.Object, null);

            // act
            var result = await controller.Details(scan.ID);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ScanDetailDto>(viewResult.ViewData.Model);
            Assert.Equal(scan.ID, model.ScanID);
        }

        [Fact]
        public async Task Details_ReturnsNotFoundResult_ForNull()
        {
            // set
            var controller = new ScansController(null, null, null, null);

            // act
            var result = await controller.Details(null);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFoundResult_ForInvalidID()
        {
            // set
            var scans = new List<Scan>()
            {
            };

            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(scans);

            var controller = new ScansController(serviceMock.Object, null, null, null);

            // act
            var result = await controller.Details(2);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateGET_ReturnsViewResult()
        {
            // set
            var controller = new ScansController(null, null, null, null);

            // act
            var result = await controller.Create(It.IsAny<int>());

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ScanDto>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Create_ReturnsRedirectToAction_IfValid()
        {
            // set
            var fileMock = new Mock<IFormFile>();
            var dto = new ScanDto()
            {
                ScanID = 1,
                EdfFile = fileMock.Object,
            };
            var recordingServiceMock = new Mock<IRecordingService>();
            recordingServiceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync((Recording)null);

            var scanServiceMock = new Mock<IScanService>();
            scanServiceMock.Setup(m => m.Create(It.IsAny<Scan>()));

            var controller = new ScansController(scanServiceMock.Object, null, null, recordingServiceMock.Object);

            // act
            var result = await controller.Create(dto);

            // assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);
            Assert.Equal(nameof(RecordingsController.Details), actionName);
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WithScanDto_IfInvalid()
        {
            // set
            var dto = new ScanDto()
            {
                ScanID = 1
            };

            var scanServiceMock = new Mock<IScanService>();
            scanServiceMock.Setup(m => m.Create(It.IsAny<Scan>()));

            var controller = new ScansController(scanServiceMock.Object, null, null, null);
            controller.ModelState.AddModelError("PatientID", "PatientID is required");

            // act
            var result = await controller.Create(dto);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ScanDto>(viewResult.ViewData.Model);
            Assert.Equal(dto.ScanID, model.ScanID);
        }

        [Fact]
        public async Task EditGET_ReturnsViewResult_WithScanDto()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var scanServiceMock = new Mock<IScanService>();
            scanServiceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(scan);

            var controller = new ScansController(scanServiceMock.Object, null, null, null);

            // act
            var result = await controller.Edit(It.IsAny<int>());

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ScanDto>(viewResult.ViewData.Model);
            Assert.Equal(scan.ID, model.ScanID);
        }

        [Fact]
        public async Task EditGET_ReturnsNotFoundResult_ForNull()
        {
            // set
            var controller = new ScansController(null, null, null, null);

            // act
            var result = await controller.Edit(null);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGET_ReturnsNotFoundResult_ForInvalidID()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(1))
                .ReturnsAsync(scan);
            var controller = new ScansController(serviceMock.Object, null, null, null);

            // act
            var result = await controller.Edit(2);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFoundResult_ForInvalidIdAndScanDtoCombo()
        {
            // set
            var dto = new ScanDto()
            {
                ScanID = 1
            };
            var serviceMock = new Mock<IScanService>();
            var controller = new ScansController(serviceMock.Object, null, null, null);

            // act
            var result = await controller.Edit(2, dto);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFoundResult_ForFailure()
        {
            // set
            var scan = new Scan();
            var dto = new ScanDto()
            {
                ScanID = 1
            };
            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(scan);
            serviceMock.Setup(m => m.Update(It.IsAny<Scan>()))
                .ReturnsAsync(false);


            var controller = new ScansController(serviceMock.Object, null, null, null);

            // act
            var result = await controller.Edit(dto.ScanID, dto);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithScanDto_ForInvalidModel()
        {
            // set
            var dto = new ScanDto()
            {
                ScanID = 1
            };

            var controller = new ScansController(null, null, null, null);
            controller.ModelState.AddModelError("PatientID", "PatientID is required");

            // act
            var result = await controller.Edit(dto.ScanID, dto);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ScanDto>(viewResult.ViewData.Model);
            Assert.Equal(dto, model);
        }

        [Fact]
        public async Task Edit_ReturnsRedirectToAction_ForSuccess()
        {
            // set
            var scan = new Scan();
            var dto = new ScanDto()
            {
                ScanID = 1
            };
            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(scan);
            serviceMock.Setup(m => m.Update(It.IsAny<Scan>()))
                .ReturnsAsync(true);

            var controller = new ScansController(serviceMock.Object, null, null, null);

            // act
            var result = await controller.Edit(dto.ScanID, dto);

            // assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(redirectResult.ActionName);
            Assert.Equal(nameof(RecordingsController.Details), actionName);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_ForNull()
        {
            // set
            var controller = new ScansController(null, null, null, null);

            // act
            var result = await controller.Delete(null);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_ForInvalidID()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(1))
                .ReturnsAsync(scan);

            var controller = new ScansController(serviceMock.Object, null, null, null);

            // act
            var result = await controller.Delete(2);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithScanDto()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(scan);

            var controller = new ScansController(serviceMock.Object, null, null, null);

            // act
            var result = await controller.Delete(1);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ScanDto>(viewResult.ViewData.Model);
            Assert.Equal(scan.ID, model.ScanID);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsNotFoundResult_ForInvalidID()
        {
            // set
            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(1))
                .ReturnsAsync((Scan)null);
            serviceMock.Setup(m => m.Delete(1))
                .ReturnsAsync(true);
            var controller = new ScansController(serviceMock.Object, null, null, null);

            // act
            var result = await controller.DeleteConfirmed(2);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToAction_ForSuccess()
        {
            // set
            var scan = new Scan()
            {
                RecordingID = 1
            };

            var serviceMock = new Mock<IScanService>();
            serviceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(scan);
            serviceMock.Setup(m => m.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            var controller = new ScansController(serviceMock.Object, null, null, null);

            // act
            var result = await controller.DeleteConfirmed(1);

            // assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(redirectResult.ActionName);
            Assert.Equal(nameof(RecordingsController.Details), actionName);
        }
    }
}
