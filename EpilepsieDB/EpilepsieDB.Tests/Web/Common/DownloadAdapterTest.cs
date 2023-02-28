using EpilepsieDB.Services;
using EpilepsieDB.Source;
using EpilepsieDB.Web.Common;
using EpilepsieDB.Web.View.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.Common
{
    public class DownloadAdapterTest : AbstractTest
    {
        public DownloadAdapterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task DownloadScan_Returns_FileContentResult()
        {
            // set
            var file = new FileRequest()
            {
                Data = new byte[] { 1, 2, 3 },
                Name = "TestFile.zip"
            };

            var serviceMock = new Mock<IDownloadService>();
            serviceMock.Setup(m => m.GetScanFile(It.IsAny<int>()))
                .ReturnsAsync(file);

            var controller = new DownloadAdapter(serviceMock.Object);

            // act
            var result = await controller.DownloadScan(It.IsAny<int>());

            // assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal(file.Name, fileResult.FileDownloadName);
            Assert.Equal(file.Data, fileResult.FileContents);
        }

        [Fact]
        public async Task DownloadScan_Returns_JsonResult_IfScanIDIsNull()
        {
            // set
            var serviceMock = new Mock<IDownloadService>();

            var controller = new DownloadAdapter(serviceMock.Object);

            // act
            var result = await controller.DownloadScan(null);

            // assert
            var jsonResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task DownloadScan_Returns_JsonResult_IfFileIsNull()
        {
            // set
            var serviceMock = new Mock<IDownloadService>();
            serviceMock.Setup(m => m.GetScanFile(It.IsAny<int>()))
                .ReturnsAsync((FileRequest)null);

            var controller = new DownloadAdapter(serviceMock.Object);

            // act
            var result = await controller.DownloadScan(It.IsAny<int>());

            // assert
            var jsonResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task DownloadScansByRecording_Returns_FileContentResult()
        {
            // set
            var file = new FileRequest()
            {
                Data = new byte[] { 1, 2, 3 },
                Name = "TestFile.zip"
            };

            var serviceMock = new Mock<IDownloadService>();
            serviceMock.Setup(m => m.GetScanFilesByRecording(
                It.IsAny<int>()))
                .ReturnsAsync(file);

            var controller = new DownloadAdapter(serviceMock.Object);

            // act
            var result = await controller.DownloadScansByRecording(It.IsAny<int>());

            // assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal(file.Name, fileResult.FileDownloadName);
            Assert.Equal(file.Data, fileResult.FileContents);
        }

        [Fact]
        public async Task DownloadScansByRecording_Returns_JsonResult_IfFileIsNull()
        {
            // set
            var serviceMock = new Mock<IDownloadService>();
            serviceMock.Setup(m => m.GetScanFilesByRecording(It.IsAny<int>()))
                .ReturnsAsync((FileRequest)null);

            var controller = new DownloadAdapter(serviceMock.Object);

            // acts
            var result = await controller.DownloadScansByRecording(It.IsAny<int>());

            // assert
            var jsonResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task DownloadScansByRecording_Returns_JsonResult_IfRecordingIDIsNull()
        {
            // set
            var serviceMock = new Mock<IDownloadService>();

            var controller = new DownloadAdapter(serviceMock.Object);

            // act
            var result = await controller.DownloadScansByRecording(null);

            // assert
            var jsonResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task DownloadScansByPatient_Returns_FileContentResult()
        {
            // set
            var file = new FileRequest()
            {
                Data = new byte[] { 1, 2, 3 },
                Name = "TestFile.zip"
            };

            var serviceMock = new Mock<IDownloadService>();
            serviceMock.Setup(m => m.GetFilesFromPatient(
                It.IsAny<int>()))
                .ReturnsAsync(file);

            var controller = new DownloadAdapter(serviceMock.Object);

            // act
            var result = await controller.DownloadPatient(It.IsAny<int>());

            // assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal(file.Name, fileResult.FileDownloadName);
            Assert.Equal(file.Data, fileResult.FileContents);
        }

        [Fact]
        public async Task DownloadScansByPatient_Returns_JsonResult_IfFileIsNull()
        {
            // set
            var serviceMock = new Mock<IDownloadService>();
            serviceMock.Setup(m => m.GetFilesFromPatient(
                It.IsAny<int>()))
                .ReturnsAsync((FileRequest)null);

            var controller = new DownloadAdapter(serviceMock.Object);

            // acts
            var result = await controller.DownloadPatient(It.IsAny<int>());

            // assert
            var jsonResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task DownloadScansByPatient_Returns_JsonResult_IfRecordingIDIsNull()
        {
            // set
            var serviceMock = new Mock<IDownloadService>();

            var controller = new DownloadAdapter(serviceMock.Object);

            // act
            var result = await controller.DownloadPatient(null);

            // assert
            var jsonResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task DownloadNifti_Returns_FileContentResult()
        {
            // set
            var file = new FileRequest()
            {
                Data = new byte[] { 1, 2, 3 },
                Name = "TestFile.zip"
            };

            var serviceMock = new Mock<IDownloadService>();
            serviceMock.Setup(m => m.GetNiftiFile(
                It.IsAny<int>()))
                .ReturnsAsync(file);

            var controller = new DownloadAdapter(serviceMock.Object);

            // act
            var result = await controller.DownloadNifti(It.IsAny<int>());

            // assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal(file.Name, fileResult.FileDownloadName);
            Assert.Equal(file.Data, fileResult.FileContents);
        }

        [Fact]
        public async Task DownloadNifti_Returns_JsonResult_IfFileIsNull()
        {
            // set
            var serviceMock = new Mock<IDownloadService>();
            serviceMock.Setup(m => m.GetNiftiFile(
                It.IsAny<int>()))
                .ReturnsAsync((FileRequest) null);

            var controller = new DownloadAdapter(serviceMock.Object);

            // act
            var result = await controller.DownloadNifti(It.IsAny<int>());

            // assert
            var fileResult = Assert.IsType<JsonResult>(result);
        }

        [Fact]
        public async Task DownloadNifti_Returns_JsonResult_PatientIsNull()
        {
            // set
            var file = new FileRequest()
            {
                Data = new byte[] { 1, 2, 3 },
                Name = "TestFile.zip"
            };

            var serviceMock = new Mock<IDownloadService>();
            serviceMock.Setup(m => m.GetNiftiFile(
                It.IsAny<int>()))
                .ReturnsAsync(file);

            var controller = new DownloadAdapter(serviceMock.Object);

            // act
            var result = await controller.DownloadNifti(null);

            // assert
            var fileResult = Assert.IsType<JsonResult>(result);
        }
    }

}
