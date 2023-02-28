using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Services.Impl;
using EpilepsieDB.Services;
using EpilepsieDB.Source;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using System.Linq.Expressions;
using EpilepsieDB.Source.Wrapper;
using System.IO.Abstractions.TestingHelpers;
using System.IO.Abstractions;

namespace EpilepsieDB.Tests.Services
{
    public class DownloadServiceTest : AbstractTest
    {

        private readonly IFileSystem fileSystem;
        private IFileSystemWrapper fileSystemWrapper;

        public DownloadServiceTest(ITestOutputHelper output) : base(output)
        {
            fileSystem = new MockFileSystem();
            fileSystemWrapper = new FileSystemWrapper(fileSystem);
        }

        [Fact]
        public void Constructor()
        {
            // set
            var patientRepoMock = new Mock<IRepository<Patient>>();
            var recordingRepoMock = new Mock<IRepository<Recording>>();
            var scanRepoMock = new Mock<IRepository<Scan>>();
            var fileServiceMock = new Mock<IFileService>();

            // act
            var service = new DownloadService(patientRepoMock.Object, recordingRepoMock.Object, scanRepoMock.Object, fileServiceMock.Object, fileSystemWrapper, null);
        }

        [Fact]
        public async Task GetScanFile_Returns_FileRequest()
        {
            // set
            var scan = new Scan()
            {
                ID = 1,
                EdfDisplayName = "Test",
            };

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(scan);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromFile(It.IsAny<string>()))
                .Returns(new byte[0]);

            // act
            var service = new DownloadService(null, null, repoMock.Object, fileServiceMock.Object, fileSystemWrapper, null);
            var res = await service.GetScanFile(It.IsAny<int>());

            // assert
            Assert.NotNull(res);
            Assert.IsType<FileRequest>(res);
        }

        [Fact]
        public async Task GetScanFile_Returns_FileRequest_WithCorrectBytes()
        {
            // set
            var expected = new byte[] { 1, 2, 3 };

            var displayName = "Test";
            var scan = new Scan()
            {
                ID = 1,
                EdfDisplayName = displayName,
            };

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(scan);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromFile(It.IsAny<string>()))
                .Returns(expected);

            // act
            var service = new DownloadService(null, null, repoMock.Object, fileServiceMock.Object, fileSystemWrapper, null);
            var res = await service.GetScanFile(It.IsAny<int>());

            // assert
            Assert.Equal(expected, res.Data);
        }

        [Fact]
        public async Task GetScanFile_Returns_FileRequest_WithCorrectName()
        {
            // set
            var displayName = "Test";
            var expected = displayName;
            var scan = new Scan()
            {
                ID = 1,
                EdfDisplayName = displayName,
            };

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(scan);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromFile(It.IsAny<string>()))
                .Returns(new byte[] {1, 2, 3});

            // act
            var service = new DownloadService(null, null, repoMock.Object, fileServiceMock.Object, fileSystemWrapper, null);
            var res = await service.GetScanFile(It.IsAny<int>());

            // assert
            Assert.Equal(expected, res.Name);
        }

        [Fact]
        public async Task GetScanFile_Returns_NullIfScanIDIsInvalid()
        {
            // set
            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync((Scan)null);

            // act
            var service = new DownloadService(null, null, repoMock.Object, null, fileSystemWrapper, null);
            var res = await service.GetScanFile(It.IsAny<int>());

            // assert
            Assert.Null(res);

        }

        [Fact]
        public async Task GetScanFilesByRecording_Returns_FileRequest()
        {

            // set
            var recordings = new List<Recording>()
            {
                new Recording()
                {
                    ID = 1,
                    Patient = new Patient
                    {
                        ID = 1,
                        Acronym = "Olaf"
                    },
                    RecordingNumber = "Rec001"
                }
            };

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Recording, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(recordings);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromDir(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new byte[] { 1, 2, 3});

            var service = new DownloadService(null, repoMock.Object, null, fileServiceMock.Object, fileSystemWrapper, "path");

            // act
            var result = await service.GetScanFilesByRecording(It.IsAny<int>());

            // assert
            Assert.NotNull(result);
            Assert.IsType<FileRequest>(result);
        }

        [Fact]
        public async Task GetScanFilesByRecording_Returns_FileRequest_WithCorrectData()
        {

            // set
            var expected = new byte[] { 1, 2, 3 };
            var recordings = new List<Recording>()
            {
                new Recording()
                {
                    ID = 1,
                    Patient = new Patient
                    {
                        ID = 1,
                        Acronym = "Olaf"
                    },
                    RecordingNumber = "Rec001"
                }
            };

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Recording, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(recordings);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromDir(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expected);

            var service = new DownloadService(null, repoMock.Object, null, fileServiceMock.Object, fileSystemWrapper, "path");

            // act
            var result = await service.GetScanFilesByRecording(It.IsAny<int>());

            // assert
            Assert.Equal(expected, result.Data);
        }

        [Fact]
        public async Task GetScanFilesByRecording_Returns_FileRequest_WithCorrectName()
        {

            // set
            var recordingName = "Rec001";
            var patientName = "Olaf";
            var recordings = new List<Recording>()
            {
                new Recording()
                {
                    ID = 1,
                    Patient = new Patient
                    {
                        ID = 1,
                        Acronym = patientName
                    },
                    RecordingNumber = recordingName
                }
            };

            var expected = patientName + "_" +recordingName + ".zip";

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Recording, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(recordings);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromDir(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new byte[] { 1, 2, 3 });

            var service = new DownloadService(null, repoMock.Object, null, fileServiceMock.Object, fileSystemWrapper, "path");

            // act
            var result = await service.GetScanFilesByRecording(It.IsAny<int>());

            // assert
            Assert.Equal(expected, result.Name);
        }

        [Fact]
        public async Task GetScanFilesByRecording_Returns_Null_IfRecordingIsNotFound()
        {

            // set
            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Get(
                It.IsAny<Expression<Func<Recording, bool>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(new List<Recording>());

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromDir(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new byte[] { 1, 2, 3 });

            var service = new DownloadService(null, repoMock.Object, null, fileServiceMock.Object, fileSystemWrapper, "path");

            // act
            var result = await service.GetScanFilesByRecording(It.IsAny<int>());

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetFilesFromPatient_Returns_FileRequest()
        {

            // set
            var patient = new Patient
            {
                ID = 1,
                Acronym = "Olaf",
                MriImagePath = "mri.png"
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromDir(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new byte[] { 1, 2, 3 });

            var fileSystemMock = new Mock<FileSystemWrapper>(fileSystem);
            fileSystemMock.CallBase = true;
            fileSystemMock.Setup(m => m.FileCopy(
                It.IsAny<string>(),
                It.IsAny<string>()));
            fileSystemMock.Setup(m => m.DirectoryDelete(
                It.IsAny<string>(),
                It.IsAny<bool>()));

            var service = new DownloadService(repoMock.Object, null, null, fileServiceMock.Object, fileSystemMock.Object, "path");

            // act
            var result = await service.GetFilesFromPatient(It.IsAny<int>());

            // assert
            Assert.NotNull(result);
            Assert.IsType<FileRequest>(result);
        }

        [Fact]
        public async Task GetFilesFromPatient_Returns_FileRequest_WithCorrectData()
        {

            // set
            var expected = new byte[] { 1, 2, 3 };
            var patient = new Patient
            {
                ID = 1,
                Acronym = "Olaf",
                MriImagePath = "mri.png"
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromDir(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(expected);

            var fileSystemMock = new Mock<FileSystemWrapper>(fileSystem);
            fileSystemMock.CallBase = true;
            fileSystemMock.Setup(m => m.FileCopy(
                It.IsAny<string>(),
                It.IsAny<string>()));
            fileSystemMock.Setup(m => m.DirectoryDelete(
                It.IsAny<string>(),
                It.IsAny<bool>()));

            var service = new DownloadService(repoMock.Object, null, null, fileServiceMock.Object, fileSystemMock.Object, "path");

            // act
            var result = await service.GetFilesFromPatient(It.IsAny<int>());

            // assert
            Assert.Equal(expected, result.Data);
        }

        [Fact]
        public async Task GetFilesFromPatient_Returns_FileRequest_WithCorrectName()
        {

            // set
            var name = "Olaf";
            var expected = name + ".zip";
            var patient = new Patient
            {
                ID = 1,
                Acronym = name,
                MriImagePath = "mri.png"
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromDir(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new byte[] { 1, 2, 3 });

            var fileSystemMock = new Mock<FileSystemWrapper>(fileSystem);
            fileSystemMock.CallBase = true;
            fileSystemMock.Setup(m => m.FileCopy(
                It.IsAny<string>(),
                It.IsAny<string>()));
            fileSystemMock.Setup(m => m.DirectoryDelete(
                It.IsAny<string>(),
                It.IsAny<bool>()));

            var service = new DownloadService(repoMock.Object, null, null, fileServiceMock.Object, fileSystemMock.Object, "path");

            // act
            var result = await service.GetFilesFromPatient(It.IsAny<int>());

            // assert
            Assert.Equal(expected, result.Name);
        }

        [Fact]
        public async Task GetScanFilesByPatient_Returns_Null_IfPatientIsNotFound()
        {

            // set
            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync((Patient)null);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromDir(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new byte[] { 1, 2, 3 });

            var service = new DownloadService(repoMock.Object, null, null, fileServiceMock.Object, fileSystemWrapper, "path");

            // act
            var result = await service.GetFilesFromPatient(It.IsAny<int>());

            // assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetNiftiFile_Returns_FileRequest()
        {
            // set
            var patient = new Patient
            {
                ID = 1,
                Acronym = "Olaf"
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromFile(It.IsAny<string>()))
                .Returns(new byte[] { 1, 2, 3 });

            var service = new DownloadService(repoMock.Object, null, null, fileServiceMock.Object, fileSystemWrapper, "path");

            // act
            var result = await service.GetNiftiFile(It.IsAny<int>());

            // assert
            Assert.NotNull(result);
            Assert.IsType<FileRequest>(result);
        }

        [Fact]
        public async Task GetNiftiFile_Returns_FileRequest_WithCorrectData()
        {

            // set
            var expected = new byte[] { 1, 2, 3 };
            var patient = new Patient
            {
                ID = 1,
                Acronym = "Olaf"
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromFile(It.IsAny<string>()))
                .Returns(expected);

            var service = new DownloadService(repoMock.Object, null, null, fileServiceMock.Object, fileSystemWrapper, "path");

            // act
            var result = await service.GetNiftiFile(It.IsAny<int>());

            // assert
            Assert.Equal(expected, result.Data);
        }

        [Fact]
        public async Task GetNiftiFile_Returns_FileRequest_WithCorrectName()
        {

            // set
            var name = "Olaf";
            var expected = name + ".nii";
            var patient = new Patient
            {
                ID = 1,
                Acronym = name
            };

            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromFile(It.IsAny<string>()))
                .Returns(new byte[] { 1, 2, 3 });

            var service = new DownloadService(repoMock.Object, null, null, fileServiceMock.Object, fileSystemWrapper, "path");

            // act
            var result = await service.GetNiftiFile(It.IsAny<int>());

            // assert
            Assert.Equal(expected, result.Name);
        }

        [Fact]
        public async Task GetNiftiFile_Returns_Null_IfPatientIsNotFound()
        {

            // set
            var repoMock = new Mock<IRepository<Patient>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync((Patient)null);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.ReadAllBytesFromFile(It.IsAny<string>()))
                .Returns(new byte[] { 1, 2, 3 });

            var service = new DownloadService(repoMock.Object, null, null, fileServiceMock.Object, fileSystemWrapper, "path");

            // act
            var result = await service.GetNiftiFile(It.IsAny<int>());

            // assert
            Assert.Null(result);
        }
    }
}
