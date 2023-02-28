using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using EpilepsieDB.Models;
using EpilepsieDB.Web.View.Controllers;
using EpilepsieDB.Services;
using Moq;
using Xunit.Abstractions;
using System.Linq.Expressions;
using System;
using EpilepsieDB.Web.View.ViewModels;
using Microsoft.AspNetCore.Http;
using EpilepsieDB.Source;
using Microsoft.AspNetCore.Authorization;
using EpilepsieDB.Authorization;

namespace EpilepsieDB.Tests.Web.View.Controller
{
    public class PatientsControllerTest : AbstractTest
    {
        public PatientsControllerTest(ITestOutputHelper output) : base(output) { }

        [Fact]
        public async Task Index_ReturnsViewResult_WithListOfPatientDtos()
        {
            // set
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.GetAll())
                .ReturnsAsync(new List<Patient>()
                {
                    new Patient(), new Patient(), new Patient()
                });
            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.Index();

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<PatientDto>>(viewResult.ViewData.Model);
            Assert.Equal(3, model.Count());
        }

        [Fact]
        public async Task Details_ReturnsViewResult_WithPatientDto()
        {
            // set
            var patient = new Patient()
            {
                ID = 1
            };
            var recordings = new List<Recording>();

            var patientServiceMock = new Mock<IPatientService>();
            patientServiceMock.Setup(m => m.Get(patient.ID))
                .ReturnsAsync(patient);

            var recordingServiceMock = new Mock<IRecordingService>();
            recordingServiceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Recording, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(recordings);

            var controller = new PatientsController(patientServiceMock.Object, recordingServiceMock.Object);

            // act
            var result = await controller.Details(patient.ID);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PatientDetailDto>(viewResult.ViewData.Model);
            Assert.Equal(patient.ID, model.PatientID);
        }

        [Fact]
        public async Task Details_ReturnsNotFoundResult_ForNull()
        {
            // set
            var patient = new Patient()
            {
                ID = 1
            };

            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Get(1))
                .ReturnsAsync(patient);

            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.Details(null);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFoundResult_ForInvalidID()
        {
            // set
            var patient = new Patient()
            {
                ID = 1
            };

            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Get(1))
                .ReturnsAsync(patient);

            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.Details(2);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateGET_ReturnsViewResult()
        {
            // set
            var controller = new PatientsController(null, null);

            // act
            var result = controller.Create();

            // assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsRedirectToAction_IfValid()
        {
            // set
            var fileMock = new Mock<IFormFile>();
            var dto = new PatientDto()
            {
                PatientID = 1,
                NiftiFile = fileMock.Object,
                MriImage = fileMock.Object,
            };
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Create(It.IsAny<Patient>()));

            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.Create(dto);

            // assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(viewResult.ActionName);
            Assert.Equal(nameof(controller.Index), actionName);
        }

        [Fact]
        public async Task Create_ReturnsViewResult_WithPatientDto_IfInvalid()
        {
            // set
            var dto = new PatientDto()
            {
                PatientID = 1
            };
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Create(It.IsAny<Patient>()));

            var controller = new PatientsController(serviceMock.Object, null);
            controller.ModelState.AddModelError("PatientID", "PatientID is required");

            // act
            var result = await controller.Create(dto);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PatientDto>(viewResult.ViewData.Model);
            Assert.Equal(dto.PatientID, model.PatientID);
        }

        [Fact]
        public async Task EditGET_ReturnsViewResult_WithPatientDto()
        {
            // set
            var patient = new Patient()
            {
                ID = 1
            };
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);
            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.Edit(It.IsAny<int>());

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PatientEditDto>(viewResult.ViewData.Model);
            Assert.Equal(patient.ID, model.PatientID);
        }

        [Fact]
        public async Task EditGET_ReturnsNotFoundResult_ForNull()
        {
            // set
            var controller = new PatientsController(null, null);

            // act
            var result = await controller.Edit(null);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task EditGET_ReturnsNotFoundResult_ForInvalidID()
        {
            // set
            var patient = new Patient()
            {
                ID = 1
            };
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Get(1))
                .ReturnsAsync(patient);
            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.Edit(2);

            // assert
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFoundResult_ForInvalidIdAndPatientCombo()
        {
            // set
            var dto = new PatientEditDto()
            {
                PatientID = 1
            };
            var serviceMock = new Mock<IPatientService>();
            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.Edit(2, dto);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFoundResult_ForFailure()
        {
            // set
            var dto = new PatientEditDto()
            {
                PatientID = 1
            };
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Update(
                It.IsAny<Patient>(),
                It.IsAny<IFileStream>(),
                It.IsAny<IFileStream>()))
                .ReturnsAsync(false);
            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.Edit(dto.PatientID, dto);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithPatientEditDto_ForInvalidModel()
        {
            // set
            var dto = new PatientEditDto()
            {
                PatientID = 1
            };
            var controller = new PatientsController(null, null);
            controller.ModelState.AddModelError("PatientID", "PatientID is required");

            // act
            var result = await controller.Edit(dto.PatientID, dto);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PatientEditDto>(viewResult.ViewData.Model);
            Assert.Equal(dto, model);
        }

        [Fact]
        public async Task Edit_ReturnsRedirectToAction_ForSuccess()
        {
            // set
            var fileMock = new Mock<IFormFile>();
            var dto = new PatientEditDto()
            {
                PatientID = 1,
                NiftiFile = fileMock.Object,
                MriImage = fileMock.Object,
            };
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Update(
                It.IsAny<Patient>(),
                It.IsAny<IFileStream>(),
                It.IsAny<IFileStream>()))
                .ReturnsAsync(true);

            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.Edit(dto.PatientID, dto);

            // assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(redirectResult.ActionName);
            Assert.Equal(nameof(controller.Index), actionName);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_ForNull()
        {
            // set
            var controller = new PatientsController(null, null);

            // act
            var result = await controller.Delete(null);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_ForInvalidID()
        {
            // set
            var patient = new Patient()
            {
                ID = 1
            };
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Get(1))
                .ReturnsAsync(patient);
            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.Delete(2);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithPatient()
        {
            // set
            var patient = new Patient()
            {
                ID = 1
            };
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Get(1))
                .ReturnsAsync(patient);
            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.Delete(1);

            // assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PatientDto>(viewResult.ViewData.Model);
            Assert.Equal(patient.ID, model.PatientID);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsNotFoundResult_ForInvalidID()
        {
            // set
            var patient = new Patient()
            {
                ID = 1
            };
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Delete(1))
                .ReturnsAsync(true);
            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.DeleteConfirmed(2);

            // assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteConfirmed_ReturnsRedirectToAction_ForSuccess()
        {
            // set
            var patient = new Patient()
            {
                ID = 1
            };
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Delete(1))
                .ReturnsAsync(true);
            var controller = new PatientsController(serviceMock.Object, null);

            // act
            var result = await controller.DeleteConfirmed(1);

            // assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            var actionName = Assert.IsAssignableFrom<string>(redirectResult.ActionName);
            Assert.Equal(nameof(controller.Index), actionName);
        }
    }
}
