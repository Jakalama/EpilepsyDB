using EpilepsieDB.EDF;
using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Services;
using EpilepsieDB.Services.Impl;
using EpilepsieDB.Source;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Services
{

    [Collection("Sequential")]
    public class ScanServiceTest : AbstractTest
    {
        public ScanServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor()
        {
            // set
            var scanRepoMock = new Mock<IRepository<Scan>>();
            var recordingRepoMock = new Mock<IRepository<Recording>>();
            var fileServiceMock = new Mock<IFileService>();
            var edfServiceMock = new Mock<IEdfService>();

            // act
            new ScanService(scanRepoMock.Object, recordingRepoMock.Object, fileServiceMock.Object, edfServiceMock.Object);
        }

        [Fact]
        public async Task GetScan_ReturnsScan()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(scan);

            var service = new ScanService(repoMock.Object, null, null, null);

            // act
            var result = await service.Get(It.IsAny<int>());

            // assert
            Assert.Equal(scan, result);
        }

        [Fact]
        public async Task GetScans_ReturnsListOfScans()
        {
            // set
            var list = new List<Scan>()
            {
                new Scan() { ID = 1 },
                new Scan() { ID = 2 },
            };

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.GetAll()).
                ReturnsAsync(list);

            var service = new ScanService(repoMock.Object, null, null, null);

            // act
            var result = await service.GetAll();

            // assert
            Assert.Equal(2, result.Count);
            Assert.Equal(list, result);
        }

        //[Fact]
        //public async Task Create_Scan_AddsScan()
        //{
        //    // set
        //    var scan = new Scan()
        //    {
        //        ID = 1
        //    };
        //    var list = new List<Scan>();
        //    var patient = new Patient() { ContentDir = "test"};
        //    var recording = new Recording() { Patient = patient, RecordingNumber = "A001" };

        //    var recordingRepoMock = new Mock<IRepository<Recording>>();
        //    recordingRepoMock.Setup(m => m.Get(It.IsAny<int>()))
        //        .ReturnsAsync(recording);

        //    var repoMock = new Mock<IRepository<Scan>>();
        //    repoMock.Setup(m => m.Add(It.IsAny<Scan>()))
        //        .Callback<Scan>((s) => list.Add(s));

        //    var service = new ScanService(repoMock.Object, recordingRepoMock.Object, null);

        //    // act
        //    await service.Create(scan);

        //    // assert
        //    Assert.Equal(1, list.Count);
        //    Assert.Equal(scan, list[0]);
        //}

        [Fact]
        public async Task Create_AddNoScan_IfFileIsNull()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var list = new List<Scan>();
            var patient = new Patient() { ContentDir = "test" };
            var recording = new Recording() { Patient = patient, RecordingNumber = "A001" };

            var recordingRepoMock = new Mock<IRepository<Recording>>();
            recordingRepoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Add(It.IsAny<Scan>()))
                .Callback<Scan>((s) => list.Add(s));

            var service = new ScanService(repoMock.Object, recordingRepoMock.Object, null, null);

            // act
            await service.Create(scan, null);

            // assert
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task Create_AddNoScan_IfRecordingNotFound()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var list = new List<Scan>();
            var patient = new Patient() { ContentDir = "test" };

            var file = new Mock<IFileStream>();
           
            var recordingRepoMock = new Mock<IRepository<Recording>>();
            recordingRepoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync((Recording) null);

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Add(It.IsAny<Scan>()))
                .Callback<Scan>((s) => list.Add(s));

            var service = new ScanService(repoMock.Object, recordingRepoMock.Object, null, null);

            // act
            await service.Create(scan, file.Object);

            // assert
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task Create_AddsScan()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var list = new List<Scan>();
            var recording = new Recording() { ContentDir = "test" };

            var fileMock = new Mock<IFileStream>();

            var recordingRepoMock = new Mock<IRepository<Recording>>();
            recordingRepoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Add(It.IsAny<Scan>()))
                .Callback<Scan>((s) => list.Add(s));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()))
                .ReturnsAsync(recording.ContentDir);
            fileServiceMock.Setup(m => m.GetFullFilePath(It.IsAny<string>()))
                .Returns(recording.ContentDir);

            var edfServiceMock = new Mock<IEdfService>();
            edfServiceMock.Setup(m => m.WriteToScan(
                It.IsAny<Scan>(),
                It.IsAny<string>()))
                .Returns(true);

            var service = new ScanService(repoMock.Object, recordingRepoMock.Object, fileServiceMock.Object, edfServiceMock.Object);

            // act
            await service.Create(scan, fileMock.Object);

            // assert
            Assert.Equal(1, list.Count);
            Assert.Equal(scan, list[0]);
        }

        [Fact]
        public async Task Create_AddsNotScan_IfFileCantBeRead()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var list = new List<Scan>();
            var recording = new Recording() { ContentDir = "test" };

            var fileMock = new Mock<IFileStream>();

            var recordingRepoMock = new Mock<IRepository<Recording>>();
            recordingRepoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Add(It.IsAny<Scan>()))
                .Callback<Scan>((s) => list.Add(s));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string>()))
                .ReturnsAsync(recording.ContentDir);
            fileServiceMock.Setup(m => m.GetFullFilePath(It.IsAny<string>()))
                .Returns(recording.ContentDir);

            var edfServiceMock = new Mock<IEdfService>();
            edfServiceMock.Setup(m => m.WriteToScan(
                It.IsAny<Scan>(),
                It.IsAny<string>()))
                .Returns(false);

            var service = new ScanService(repoMock.Object, recordingRepoMock.Object, fileServiceMock.Object, edfServiceMock.Object);

            // act
            await service.Create(scan, fileMock.Object);

            // assert
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task Create_CreatesNoFiles()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var patient = new Patient() { ContentDir = "test" };
            var recording = new Recording() { Patient = patient, RecordingNumber = "A001" };

            var recordingRepoMock = new Mock<IRepository<Recording>>();
            recordingRepoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Add(It.IsAny<Scan>()));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string[]>()));

            var service = new ScanService(repoMock.Object, recordingRepoMock.Object, null, null);

            // act
            await service.Create(scan, null);

            // assert
            fileServiceMock.Verify(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string[]>()), Times.Never);
        }


        [Fact]
        public async Task Create_CreatesFile()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var recording = new Recording() { ContentDir = "test" };

            var recordingRepoMock = new Mock<IRepository<Recording>>();
            recordingRepoMock.Setup(
                m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Add(It.IsAny<Scan>()));

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string[]>()))
                .ReturnsAsync(recording.ContentDir);
            fileServiceMock.Setup(m => m.GetFullFilePath(It.IsAny<string>()))
                .Returns(recording.ContentDir);

            var edfServiceMock = new Mock<IEdfService>();

            var service = new ScanService(repoMock.Object, recordingRepoMock.Object, fileServiceMock.Object, edfServiceMock.Object);

            var fileMock = new Mock<IFileStream>();

            // act
            await service.Create(scan, fileMock.Object);

            // assert
            fileServiceMock.Verify(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string[]>()), Times.Once);
        }

        [Fact]
        public async Task Create_SetsFileName()
        {
            // set
            var fileName = "test.edf";
            var scan = new Scan()
            {
                ID = 1
            };
            var recording = new Recording() { ContentDir = "test" };

            var recordingRepoMock = new Mock<IRepository<Recording>>();
            recordingRepoMock.Setup(
                m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(recording);

            var fileMock = new Mock<IFileStream>();
            fileMock.Setup(m => m.FileName)
                .Returns(fileName);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.CreateFile(It.IsAny<IFileStream>(), It.IsAny<string[]>()))
                .ReturnsAsync(fileName);

            var edfServiceMock = new Mock<IEdfService>();

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Add(It.IsAny<Scan>()));

            var service = new ScanService(repoMock.Object, recordingRepoMock.Object, fileServiceMock.Object, edfServiceMock.Object);

            // act
            await service.Create(scan, fileMock.Object);

            // assert
            Assert.Equal(fileName, scan.EdfDisplayName);
            Assert.Equal(fileName, scan.EdfFilePath);
        }

        [Fact]
        public async Task Update_ReturnsTrue_IfScanExists()
        {
            // set
            var scan = new Scan()
            {
                ID = 1,
            };
            var updatedScan = new Scan()
            {
                ID = 1,
            };
            var storedScan = scan;

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(storedScan);
            repoMock.Setup(m => m.Update(updatedScan))
                .Callback<Scan>((s) => storedScan = s);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);

            var service = new ScanService(repoMock.Object, null, null, null);

            // act
            var result = await service.Update(updatedScan);

            // assert
            Assert.Equal(true, result);
        }

        [Fact]
        public async Task Update_ReturnsFalse_IfScanNotExists()
        {
            // set
            int expectedValue = 123;
            var scan = new Scan()
            {
                ID = 1,
                RecordingID = expectedValue,
            };
            var updatedScan = new Scan()
            {
                ID = 1,
                RecordingID = 987,
            };
            var storedScan = scan;

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(storedScan);
            repoMock.Setup(m => m.Update(updatedScan))
                .Callback<Scan>((s) => storedScan = s);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(false);

            var service = new ScanService(repoMock.Object, null, null, null);

            // act
            var result = await service.Update(updatedScan);

            // assert
            Assert.Equal(false, result);
            Assert.Equal(expectedValue, storedScan.RecordingID);
        }

        [Fact]
        public async Task Update_SetsRecordingNumber()
        {
            // set
            var expected = "002";
            var scan = new Scan()
            {
                ID = 1,
                ScanNumber = "001"
            };
            var updatedScan = new Scan()
            {
                ID = 1,
                ScanNumber = expected,
            };
            var storedScan = scan;

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(storedScan);
            repoMock.Setup(m => m.Update(updatedScan))
                .Callback<Scan>((s) => storedScan = s);
            repoMock.Setup(m => m.Exists(It.IsAny<int>()))
                .ReturnsAsync(true);

            var service = new ScanService(repoMock.Object, null, null, null);

            // act
            var result = await service.Update(updatedScan);

            // assert
            Assert.Equal(expected, storedScan.ScanNumber);
        }

        [Fact]
        public async Task Delete_ReturnsTrue_IfScanExists()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var list = new List<Scan>()
            {
                scan
            };

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Delete(scan))
                .Callback<Scan>((p) => list.Remove(scan));
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(scan);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.DeleteFile(It.IsAny<string>()));

            var service = new ScanService(repoMock.Object, null, fileServiceMock.Object, null);

            // act
            var result = await service.Delete(It.IsAny<int>());

            // assert
            Assert.Equal(true, result);
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public async Task Delete_ReturnsFalse_IfScanNotExists()
        {
            // set
            var scan = new Scan()
            {
                ID = 1,
                RecordingID = 123

            };
            var list = new List<Scan>()
            {
                new Scan()
                {
                    ID = 2,
                    RecordingID = 456
                }
            };

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Delete(scan))
                .Callback<Scan>((p) => list.Remove(scan));
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync((Scan)null);
            var service = new ScanService(repoMock.Object, null, null, null);

            // act
            var result = await service.Delete(It.IsAny<int>());

            // assert
            Assert.Equal(false, result);
            Assert.Equal(1, list.Count);
        }

        [Fact]
        public async Task Delete_RemovesScanFile_IfScanExists()
        {
            // set
            var scan = new Scan()
            {
                ID = 1
            };
            var list = new List<Scan>()
            {
                scan
            };

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Delete(scan))
                .Callback<Scan>((p) => list.Remove(scan));
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync(scan);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.DeleteFile(It.IsAny<string>()));

            var service = new ScanService(repoMock.Object, null, fileServiceMock.Object, null);

            // act
            var result = await service.Delete(It.IsAny<int>());

            // assert
            fileServiceMock.Verify(m => m.DeleteFile(It.IsAny<string>()), Times.Once());
        }

        [Fact]
        public async Task Delete_DoesntRemoveScanFile_IfScanNotExists()
        {
            // set
            var scan = new Scan()
            {
                ID = 1,
                RecordingID = 123

            };
            var list = new List<Scan>()
            {
                new Scan()
                {
                    ID = 2,
                    RecordingID = 456
                }
            };

            var repoMock = new Mock<IRepository<Scan>>();
            repoMock.Setup(m => m.Delete(scan))
                .Callback<Scan>((p) => list.Remove(scan));
            repoMock.Setup(m => m.Get(It.IsAny<int>()))
                .ReturnsAsync((Scan)null);

            var fileServiceMock = new Mock<IFileService>();
            fileServiceMock.Setup(m => m.DeleteFile(It.IsAny<string>()));

            var service = new ScanService(repoMock.Object, null, fileServiceMock.Object, null);

            // act
            var result = await service.Delete(It.IsAny<int>());

            // assert
            fileServiceMock.Verify(m => m.DeleteFile(It.IsAny<string>()), Times.Never());
        }
    }
}
