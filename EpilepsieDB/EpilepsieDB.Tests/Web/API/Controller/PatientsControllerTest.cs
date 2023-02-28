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
    public class PatientsControllerTest : AbstractTest
    {
        public PatientsControllerTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task GetPatients()
        {
            // set
            var list = new List<Patient>()
            {
                new Patient()
            };
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.GetAll())
                .ReturnsAsync(list);

            var controller = new PatientsController(serviceMock.Object);

            // act
            var result = await controller.GetPatients();

            // assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<PatientApiDto>>>(result);
            var data = Assert.IsType<List<PatientApiDto>>(((OkObjectResult)actionResult.Result).Value);
            Assert.NotNull(data);
            Assert.Single(data);
        }

        [Fact]
        public async Task GetPatient()
        {
            // set
            var list = new List<Patient>()
            {
                new Patient()
            };
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Patient, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(list);

            var controller = new PatientsController(serviceMock.Object);

            // act
            var result = await controller.GetPatient(It.IsAny<int>());

            // assert
            var actionResult = Assert.IsType<ActionResult<PatientApiDto>>(result);
            var data = Assert.IsType<PatientApiDto>(actionResult.Value);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task GetPatient_ReturnsNotFound_IfScanNotFound()
        {
            // set
            var list = new List<Patient>();
            var serviceMock = new Mock<IPatientService>();
            serviceMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Patient, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(list);

            var controller = new PatientsController(serviceMock.Object);

            // act
            var result = await controller.GetPatient(It.IsAny<int>());

            // assert
            var actionResult = Assert.IsType<ActionResult<PatientApiDto>>(result);
            var data = Assert.IsType<NotFoundResult>(actionResult.Result);
        }
    }
}
