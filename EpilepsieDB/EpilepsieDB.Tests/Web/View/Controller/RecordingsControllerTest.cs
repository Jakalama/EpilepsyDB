using EpilepsieDB.Authorization;
using EpilepsieDB.Models;
using EpilepsieDB.Services;
using EpilepsieDB.Web.View.Controllers;
using EpilepsieDB.Web.View.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.View.Controller
{
    public class RecordingsControllerTest : AbstractTest
    {
        public RecordingsControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task CreateGET_ReturnsViewResult()
        {
            // set
            var controller = new RecordingsController(null, null, null);

            // act
            var result = await controller.Create(It.IsAny<int>());

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RecordingDto>(viewResult.ViewData.Model);
        }

        [Fact]
        public async Task Create_ReturnsRedirectToAction_IfValid()
        {
            // set
            var patient = new Patient()
            {
                ID = 1
            };
            var dto = new RecordingDto()
            {
                PatientID = 1,
                RecordingNumber = "001"
            };

            var patientServiceMock = new Mock<IPatientService>();
            patientServiceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var recordingServiceMock = new Mock<IRecordingService>();
            recordingServiceMock.Setup(m => m.Create(It.IsAny<Recording>()));

            var controller = new RecordingsController(recordingServiceMock.Object, patientServiceMock.Object, null);

            // act
            var result = await controller.Create(dto);

            // assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);
            Assert.Equal(nameof(PatientsController.Details), actionName);
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WithRecordingDto_IfInvalid()
        {
            // set
            var dto = new RecordingDto()
            {
                RecordingID = 1,
                RecordingNumber = "001"
            };

            var recordingServiceMock = new Mock<IRecordingService>();
            recordingServiceMock.Setup(m => m.Create(It.IsAny<Recording>()));

            var controller = new RecordingsController(recordingServiceMock.Object, null, null);
            controller.ModelState.AddModelError("PatientID", "PatientID is required");

            // act
            var result = await controller.Create(dto);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RecordingDto>(viewResult.ViewData.Model);
            Assert.Equal(dto.RecordingID, model.RecordingID);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithRecordingDetailDto()
        {
            // set
            Recording recording = new Recording()
            {
                ID = 1
            };
            var recordings = new List<Recording>()
            {
                recording
            };

            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Recording, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(recordings);

            var scanServiceMock = new Mock<IScanService>();
            scanServiceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(new List<Scan>());

            var controller = new RecordingsController(serviceMock.Object, null, scanServiceMock.Object);

            // act
            var result = await controller.Details(recording.ID);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RecordingDetailDto>(viewResult.ViewData.Model);
            Assert.Equal(recording.ID, model.RecordingID);
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithCorrectAmountOfScans()
        {
            // set
            Recording recording = new Recording()
            {
                ID = 1
            };
            var recordings = new List<Recording>()
            {
                recording
            };
            var scans = new List<Scan>()
            {
                new Scan(), new Scan()
            };

            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Recording, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(recordings);

            var scanServiceMock = new Mock<IScanService>();
            scanServiceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Scan, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(scans);

            var controller = new RecordingsController(serviceMock.Object, null, scanServiceMock.Object);

            // act
            var result = await controller.Details(recording.ID);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RecordingDetailDto>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Scans.Count());
        }

        [Fact]
        public async Task Details_ReturnsNotFoundResult_ForNull()
        {
            // set
            var controller = new RecordingsController(null, null, null);

            // act
            var result = await controller.Details(null);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFoundResult_ForInvalidID()
        {
            // set
            var recordings = new List<Recording>()
            {
            };

            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Recording, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(recordings);

            var controller = new RecordingsController(serviceMock.Object, null, null);

            // act
            var result = await controller.Details(2);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGET_ReturnsViewResult_WithRecordingDto()
        {
            // set
            var recording = new Recording()
            {
                ID = 1
            };
            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);

            var controller = new RecordingsController(serviceMock.Object, null, null);

            // act
            var result = await controller.Edit(It.IsAny<int>());

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RecordingDto>(viewResult.ViewData.Model);
            Assert.Equal(recording.ID, model.RecordingID);
        }

        [Fact]
        public async Task EditGET_ReturnsNotFoundResult_ForNull()
        {
            // set
            var controller = new RecordingsController(null, null, null);

            // act
            var result = await controller.Edit(null);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGET_ReturnsNotFoundResult_ForInvalidID()
        {
            // set
            var recording = new Recording()
            {
                ID = 1
            };
            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(1))
                .ReturnsAsync(recording);

            var controller = new RecordingsController(serviceMock.Object, null, null);

            // act
            var result = await controller.Edit(2);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFoundResult_ForInvalidIdAndScanDtoCombo()
        {
            // set
            var dto = new RecordingDto()
            {
                RecordingID = 1
            };
            var controller = new RecordingsController(null, null, null);

            // act
            var result = await controller.Edit(2, dto);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFoundResult_ForFailure()
        {
            // set
            var recording = new Recording();
            var dto = new RecordingDto()
            {
                RecordingID = 1
            };
            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);
            serviceMock.Setup(m => m.Update(It.IsAny<Recording>()))
                .ReturnsAsync(false);


            var controller = new RecordingsController(serviceMock.Object, null, null);

            // act
            var result = await controller.Edit(dto.RecordingID, dto);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithRecordingDto_ForInvalidModel()
        {
            // set
            var dto = new RecordingDto()
            {
                RecordingID = 1
            };

            var controller = new RecordingsController(null, null, null);
            controller.ModelState.AddModelError("RecordingID", "RecordingID is required");

            // act
            var result = await controller.Edit(dto.RecordingID, dto);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RecordingDto>(viewResult.ViewData.Model);
            Assert.Equal(dto, model);
        }

        [Fact]
        public async Task Edit_ReturnsRedirectToAction_ForSuccess()
        {
            // set
            var recording = new Recording();
            var dto = new RecordingDto()
            {
                RecordingID = 1
            };
            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);
            serviceMock.Setup(m => m.Update(It.IsAny<Recording>()))
                .ReturnsAsync(true);

            var controller = new RecordingsController(serviceMock.Object, null, null);

            // act
            var result = await controller.Edit(dto.RecordingID, dto);

            // assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(redirectResult.ActionName);
            Assert.Equal(nameof(RecordingsController.Details), actionName);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_ForNull()
        {
            // set
            var controller = new RecordingsController(null, null, null);

            // act
            var result = await controller.Delete(null);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_ForInvalidID()
        {
            // set
            var recording = new Recording()
            {
                ID = 1
            };

            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(1))
                .ReturnsAsync(recording);

            var controller = new RecordingsController(serviceMock.Object, null, null);

            // act
            var result = await controller.Delete(2);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithRecordingDto()
        {
            // set
            var recording = new Recording()
            {
                ID = 1
            };

            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);

            var controller = new RecordingsController(serviceMock.Object, null, null);

            // act
            var result = await controller.Delete(1);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<RecordingDto>(viewResult.ViewData.Model);
            Assert.Equal(recording.ID, model.RecordingID);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsNotFoundResult_ForInvalidID()
        {
            // set
            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(1))
                .ReturnsAsync((Recording)null);
            serviceMock.Setup(m => m.Delete(1))
                .ReturnsAsync(true);
            var controller = new RecordingsController(serviceMock.Object, null, null);

            // act
            var result = await controller.DeleteConfirmed(2);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToAction_ForSuccess()
        {
            // set
            var recording = new Recording()
            {
                PatientID = 1
            };

            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);
            serviceMock.Setup(m => m.Delete(It.IsAny<int>()))
                .ReturnsAsync(true);

            var controller = new RecordingsController(serviceMock.Object, null, null);

            // act
            var result = await controller.DeleteConfirmed(1);

            // assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(redirectResult.ActionName);
            Assert.Equal(nameof(RecordingsController.Details), actionName);
        }
    }
}
