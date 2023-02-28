using EpilepsieDB.Models;
using EpilepsieDB.Services;
using EpilepsieDB.Web.API.APIModels;
using EpilepsieDB.Web.API.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.API.Controller
{
    public class RecordingsControllerTest : AbstractTest
    {
        public RecordingsControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task GetRecordings()
        {
            // set
            var list = new List<Recording>()
            {
                new Recording()
            };

            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Recording, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(list);

            var controller = new RecordingsController(serviceMock.Object);

            // act
            var result = await controller.GetRecordings();

            // assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<RecordingApiDto>>>(result);
            var data = Assert.IsType<List<RecordingApiDto>>(((OkObjectResult)actionResult.Result).Value);
            Assert.NotNull(data);
            Assert.Single(data);
        }

        [Fact]
        public async Task GetRecording()
        {
            // set
            var list = new List<Recording>()
            {
                new Recording()
            };
            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Recording, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(list);

            var controller = new RecordingsController(serviceMock.Object);

            // act
            var result = await controller.GetRecording(It.IsAny<int>());

            // assert
            var actionResult = Assert.IsType<ActionResult<RecordingApiDto>>(result);
            var data = Assert.IsType<RecordingApiDto>(actionResult.Value);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task GetRecording_ReturnsNotFound_IfScanNotFound()
        {
            // set
            var list = new List<Recording>();
            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Recording, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(list);

            var controller = new RecordingsController(serviceMock.Object);

            // act
            var result = await controller.GetRecording(It.IsAny<int>());

            // assert
            var actionResult = Assert.IsType<ActionResult<RecordingApiDto>>(result);
            var data = Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetOfPatient()
        {
            // set
            var list = new List<Recording>()
            {
                new Recording()
            };
            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Recording, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(list);

            var controller = new RecordingsController(serviceMock.Object);

            // act
            var result = await controller.GetOfPatient(It.IsAny<int>());

            // assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<RecordingApiDto>>>(result);
            var data = Assert.IsType<List<RecordingApiDto>>(((OkObjectResult)actionResult.Result).Value);
            Assert.NotNull(data);
            Assert.Single(data);
        }

        [Fact]
        public async Task PostRecording_ReturnsActionResult_WithCreatedAction()
        {
            // set
            var dto = new RecordingsController.CreateRecordingDto()
            {
                PatientID = 1,
                RecordingNumber = "Sc001"
            };
            Recording recording = null;

            var serviceMock = new Mock<IRecordingService>();
            serviceMock.Setup(m => m.Create(It.IsAny<Recording>()))
                .Callback<Recording>(s => recording = s);

            var controller = new RecordingsController(serviceMock.Object);

            // act
            var result = await controller.PostRecording(dto);

            // assert
            var actionResult = Assert.IsType<ActionResult<RecordingApiDto>>(result);
            var data = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal("GetRecording", data.ActionName);

            Assert.Equal(recording.PatientID, dto.PatientID);
            Assert.Equal(recording.RecordingNumber, dto.RecordingNumber);
        }
    }
}
