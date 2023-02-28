using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Services;
using EpilepsieDB.Services.Impl;
using EpilepsieDB.Source;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Services
{
    public class RecordingServiceTest : AbstractTest
    {
        public RecordingServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor()
        {
            // set
            var recordingRepoMock = new Mock<IRepository<Recording>>();
            var patientRepoMock = new Mock<IRepository<Patient>>();
            var fileServiceMock = new Mock<IFileService>();

            // act
            var service = new RecordingService(recordingRepoMock.Object, patientRepoMock.Object, fileServiceMock.Object);
        }

        [Fact]
        public async Task GetRecording_ReturnsRecording()
        {
            // set
            var recording = new Recording()
            {
                ID = 1
            };

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);

            var service = new RecordingService(repoMock.Object, null, null);

            // act
            var result = await service.Get(It.IsAny<int>());

            // assert
            Assert.Equal(recording, result);
        }

        [Fact]
        public async Task GetAllRecordings_ReturnsListOfRecording()
        {
            // set
            var list = new List<Recording>()
            {
                new Recording() { ID = 1 },
                new Recording() { ID = 2 },
            };

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.GetAll()).
                ReturnsAsync(list);

            var service = new RecordingService(repoMock.Object, null, null);

            // act
            var result = await service.GetAll();

            // assert
            Assert.Equal(2, result.Count);
            Assert.Equal(list, result);
        }

        [Fact]
        public async Task CreateNew_Recording_AddsRecording()
        {
            // set
            var recording = new Recording()
            {
                ID = 1
            };
            var list = new List<Recording>();
            var patient = new Patient() { ContentDir = "test" };

            var patientRepoMock = new Mock<IRepository<Patient>>();
            patientRepoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Add(It.IsAny<Recording>()))
                .Callback<Recording>((s) => list.Add(s)); ;

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string>()));

            var service = new RecordingService(repoMock.Object, patientRepoMock.Object, fileServiceMock.Object);

            // act
            await service.Create(recording);

            // assert
            Assert.Equal(1, list.Count);
            Assert.Equal(recording, list[0]);
        }

        [Fact]
        public async Task CreateNew_AddNoRecording_IfPatientNotFound()
        {
            // set
            var recording = new Recording()
            {
                ID = 1
            };
            var list = new List<Recording>();

            var patientRepoMock = new Mock<IRepository<Patient>>();
            patientRepoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync((Patient) null);

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Add(It.IsAny<Recording>()))
                .Callback<Recording>((s) => list.Add(s)); ;

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string>()));

            var service = new RecordingService(repoMock.Object, patientRepoMock.Object, fileServiceMock.Object);

            // act
            await service.Create(recording);

            // assert
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task CreateNew_Recording_AddsDirectory()
        {
            // set
            var randonPath = "bnfsdbf";
            var recording = new Recording()
            {
                ID = 1
            };
            var list = new List<Recording>();
            var patient = new Patient() { ContentDir = "test" };

            var patientRepoMock = new Mock<IRepository<Patient>>();
            patientRepoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Add(It.IsAny<Recording>()))
                .Callback<Recording>((s) => list.Add(s)); ;

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string[]>()))
                .Returns(randonPath);

            var service = new RecordingService(repoMock.Object, patientRepoMock.Object, fileServiceMock.Object);

            // act
            await service.Create(recording);

            // assert
            fileServiceMock.Verify(m => m.CreateDirectory(It.IsAny<string[]>()), Times.Once);
        }

        [Fact]
        public async Task CreateNew_Recording_SetsContentDir()
        {
            // set
            var randonPath = "bnfsdbf";
            var recording = new Recording()
            {
                ID = 1
            };
            var list = new List<Recording>();
            var patient = new Patient() { ContentDir = "test" };

            var patientRepoMock = new Mock<IRepository<Patient>>();
            patientRepoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Add(It.IsAny<Recording>()))
                .Callback<Recording>((s) => list.Add(s)); ;

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateDirectory(It.IsAny<string[]>()))
                .Returns(randonPath);

            var service = new RecordingService(repoMock.Object, patientRepoMock.Object, fileServiceMock.Object);

            // act
            await service.Create(recording);

            // assert
            Assert.Equal(randonPath, recording.ContentDir);
        }

        [Fact]
        public async Task Update_ReturnsTrue_IfRecordingExists()
        {
            // set
            var recording = new Recording()
            {
                ID = 1,
            };
            var updatedRecording = new Recording()
            {
                ID = 1,
            };
            var storedRecording = recording;

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(storedRecording);
            repoMock.Setup(m => m.Update(updatedRecording))
                .Callback<Recording>((r) => storedRecording = r);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);

            var service = new RecordingService(repoMock.Object, null, null);

            // act
            var result = await service.Update(updatedRecording);

            // assert
            Assert.Equal(true, result);
        }

        [Fact]
        public async Task Update_ReturnsFalse_IfRecordingNotExists()
        {
            // set
            int expectedValue = 123;
            var recording = new Recording()
            {
                ID = 1,
                PatientID = expectedValue,
            };
            var updatedRecording = new Recording()
            {
                ID = 1,
                PatientID = 987,
            };
            var storedRecording = recording;

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(storedRecording);
            repoMock.Setup(m => m.Update(updatedRecording))
                .Callback<Recording>((r) => storedRecording = r);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(false);

            var service = new RecordingService(repoMock.Object, null, null);

            // act
            var result = await service.Update(updatedRecording);

            // assert
            Assert.Equal(false, result);
            Assert.Equal(expectedValue, storedRecording.PatientID);
        }

        [Fact]
        public async Task Update_SetsRecordingNumber()
        {
            // set
            var expected = "002";
            var recording = new Recording()
            {
                ID = 1,
                RecordingNumber = "001"
            };
            var updatedScan = new Recording()
            {
                ID = 1,
                RecordingNumber = expected,
            };
            var storedRecording = recording;

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(storedRecording);
            repoMock.Setup(m => m.Update(updatedScan))
                .Callback<Recording>((s) => storedRecording = s);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);

            var service = new RecordingService(repoMock.Object, null, null);

            // act
            var result = await service.Update(updatedScan);

            // assert
            Assert.Equal(expected, storedRecording.RecordingNumber);
        }

        [Fact]
        public async Task Delete_ReturnsTrue_IfRecordingExists()
        {
            // set
            var recording = new Recording()
            {
                ID = 1
            };
            var list = new List<Recording>()
            {
                recording
            };
            var patient = new Patient() { ContentDir = "test" };

            var patientRepoMock = new Mock<IRepository<Patient>>();
            patientRepoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Delete(recording))
                .Callback<Recording>((p) => list.Remove(recording));
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.DeleteDirectory(It.IsAny<string[]>()));

            var service = new RecordingService(repoMock.Object, patientRepoMock.Object, fileServiceMock.Object);

            // act
            var result = await service.Delete(It.IsAny<int>());

            // assert
            Assert.Equal(true, result);
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task Delete_DeletesRecordingDirectory()
        {
            // set
            var recording = new Recording()
            {
                ID = 1
            };
            var list = new List<Recording>()
            {
                recording
            };
            var patient = new Patient() { ContentDir = "test" };

            var patientRepoMock = new Mock<IRepository<Patient>>();
            patientRepoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(patient);

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Delete(recording))
                .Callback<Recording>((p) => list.Remove(recording));
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.DeleteDirectory(It.IsAny<string[]>()));

            var service = new RecordingService(repoMock.Object, patientRepoMock.Object, fileServiceMock.Object);

            // act
            var result = await service.Delete(It.IsAny<int>());

            // assert
            fileServiceMock.Verify(m => m.DeleteDirectory(It.IsAny<string[]>()), Times.Once); 
        }

        [Fact]
        public async Task Delete_ReturnsFalse_IfScanNotExists()
        {
            // set
            var recording = new Recording()
            {
                ID = 1,
                PatientID = 123

            };
            var list = new List<Recording>()
            {
                new Recording()
                {
                    ID = 2,
                    PatientID = 456
                }
            };

            var repoMock = new Mock<IRepository<Recording>>();
            repoMock.Setup(m => m.Delete(recording))
                .Callback<Recording>((p) => list.Remove(recording));
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync((Recording)null);
            var service = new RecordingService(repoMock.Object, null, null);

            // act
            var result = await service.Delete(It.IsAny<int>());

            // assert
            Assert.Equal(false, result);
            Assert.Equal(1, list.Count);
        }
    }
}
