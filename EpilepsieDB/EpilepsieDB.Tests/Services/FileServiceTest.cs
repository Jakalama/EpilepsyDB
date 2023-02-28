using EpilepsieDB.Services.Impl;
using EpilepsieDB.Source;
using EpilepsieDB.Source.Wrapper;
using Moq;
using System;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using static System.Net.Mime.MediaTypeNames;

namespace EpilepsieDB.Tests.Services
{
    public class FileServiceTest : AbstractTest
    {
        private readonly string baseDir;
        private readonly string testingDir;

        private readonly IFileSystem fileSystem;
        private IFileSystemWrapper fileSystemWrapper;

        public FileServiceTest(ITestOutputHelper output) : base(output)
        {
            baseDir = "test";
            testingDir = Path.Combine(baseDir, "patientData");

            fileSystem = new MockFileSystem();
            fileSystemWrapper = new FileSystemWrapper(fileSystem);
        }

        [Fact]
        public void Constructor()
        {
            // act
            var service = new FileService(fileSystemWrapper, baseDir, "web");
        }

        [Fact]
        public void Constructor_SetsWorkingDirCorrect()
        {
            // set
            var baseDir = "test";
            var expected = Path.Combine(baseDir, "patientData");

            // act
            var service = new FileService(fileSystemWrapper, baseDir, null);

            FieldInfo info = typeof(FileService).GetField("workingDir", BindingFlags.NonPublic | BindingFlags.Instance);
            var actual = info.GetValue(service);

            // assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CreateDirectory_CreatesDir_WithRandomName()
        {
            var service = new FileService(fileSystemWrapper, baseDir, null);

            // act
            var result = service.CreateDirectory();

            var expected = fileSystemWrapper.PathCombine(testingDir, result);

            // assert
            Assert.NotNull(result);
            Assert.Equal(1, fileSystemWrapper.DirectoryGetDirectories(testingDir).Length);
            Assert.True(fileSystemWrapper.DirectoryExists(expected));

        }

        [Fact]
        public void CreateDirectory_CreatesMultipleDir_IfCalledMultipleTimes()
        {
            // set
            var service = new FileService(fileSystemWrapper, baseDir, null);

            // act
            var result1 = service.CreateDirectory();
            var result2 = service.CreateDirectory();
            var result3 = service.CreateDirectory();

            var expected1 = fileSystemWrapper.PathCombine(testingDir, result1);
            var expected2 = fileSystemWrapper.PathCombine(testingDir, result2);
            var expected3 = fileSystemWrapper.PathCombine(testingDir, result3);

            // assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.NotNull(result3);
            Assert.Equal(3, fileSystemWrapper.DirectoryGetDirectories(testingDir).Length);
            Assert.True(fileSystemWrapper.DirectoryExists(expected1));
            Assert.True(fileSystemWrapper.DirectoryExists(expected2));
            Assert.True(fileSystemWrapper.DirectoryExists(expected3));
        }

        [Fact]
        public void CreateDirectory_CreatesDir_WithOutNamingConflict_IfAlreadyPresent()
        {
            // set
            var fileSystemMock = new Mock<FileSystemWrapper>(fileSystem);
            fileSystemMock.CallBase = true;
            fileSystemMock.SetupSequence(m => m.PathGetRandomFileName())
                .Returns("name1")
                .Returns("name2");

            fileSystemWrapper = fileSystemMock.Object;

            var service = new FileService(fileSystemWrapper, baseDir, null);

            var alreadyPresentDir = fileSystemWrapper.PathCombine(testingDir, "name1");
            fileSystemWrapper.DirectoryCreateDirectory(alreadyPresentDir);

            // act
            var result1 = service.CreateDirectory();

            var expected1 = fileSystemWrapper.PathCombine(testingDir, result1);

            // assert
            Assert.NotNull(result1);
            Assert.Equal(2, fileSystemWrapper.DirectoryGetDirectories(testingDir).Length);
            Assert.True(fileSystemWrapper.DirectoryExists(expected1));
        }

        [Fact]
        public void CreateDirectory_CreatesDir_Throw()
        {
            // set
            var fileSystemMock = new Mock<FileSystemWrapper>(fileSystem);
            fileSystemMock.CallBase = true;
            fileSystemMock.SetupSequence(m => m.DirectoryCreateDirectory(It.IsAny<string>()))
                .Throws(new Exception());

            fileSystemWrapper = fileSystemMock.Object;

            var service = new FileService(fileSystemWrapper, baseDir, null);

            // act
            var result1 = service.CreateDirectory();

            var expected1 = fileSystemWrapper.PathCombine(testingDir, result1);

            // assert
            // cloud be tested differently ?
            fileSystemMock.Verify(m => m.DirectoryCreateDirectory(It.IsAny<string>()), Times.Exactly(2));
        }

        [Fact]
        public void DeleteDirectory_DeletesDir_IfExists()
        {
            // set
            string contentDir = "test";
            string expected = fileSystemWrapper.PathCombine(testingDir, contentDir);

            var service = new FileService(fileSystemWrapper, baseDir, null);

            fileSystemWrapper.DirectoryCreateDirectory(expected);
            Assert.True(fileSystemWrapper.DirectoryExists(expected));

            // act
            service.DeleteDirectory(contentDir);

            // assert
            Assert.False(fileSystemWrapper.DirectoryExists(expected));
        }

        [Fact]
        public async Task CreateFile_CreatesFile()
        {
            // set
            string contentDir = "test";

            var fileMock = new Mock<IFileStream>();
            fileMock.Setup(m => m.FileName)
                .Returns("test.edf");
            fileMock.Setup(m => m.CopyToAsync(It.IsAny<Stream>(), default));

            var service = new FileService(fileSystemWrapper, baseDir, null);

            string workingDir = fileSystemWrapper.PathCombine(testingDir, contentDir);
            fileSystemWrapper.DirectoryCreateDirectory(workingDir);
            Assert.True(fileSystemWrapper.DirectoryExists(workingDir));

            // act
            var result = await service.CreateFile(fileMock.Object, contentDir);

            string expected = fileSystemWrapper.PathCombine(testingDir, result);

            // assert
           Assert.True(fileSystemWrapper.FileExists(expected));
        }

        [Fact]
        public async Task CreateFile_CreatesFile_LongPath()
        {
            // set
            string contentDir1 = "test";
            string contentDir2 = "multiple";

            var fileMock = new Mock<IFileStream>();
            fileMock.Setup(m => m.FileName)
                .Returns("test.edf");
            fileMock.Setup(m => m.CopyToAsync(It.IsAny<Stream>(), default));

            var service = new FileService(fileSystemWrapper, baseDir, null);

            string workingDir = fileSystemWrapper.PathCombine(testingDir, contentDir1, contentDir2);
            fileSystemWrapper.DirectoryCreateDirectory(workingDir);
            Assert.True(fileSystemWrapper.DirectoryExists(workingDir));

            // act
            var result = await service.CreateFile(fileMock.Object, contentDir1, contentDir2);

            string expected = fileSystemWrapper.PathCombine(testingDir, result);

            // assert
            Assert.True(result.Contains(contentDir1));
            Assert.True(result.Contains(contentDir2));
            Assert.True(fileSystemWrapper.FileExists(expected));
        }

        [Fact]
        public async Task CreateFile_CreatesFiles_WithoutNamingConflict_1()
        {
            // set
            string contentDir = "test";

            var fileMock1 = new Mock<IFileStream>();
            fileMock1.Setup(m => m.FileName)
                .Returns("test.edf");
            fileMock1.Setup(m => m.CopyToAsync(It.IsAny<Stream>(), default));

            var fileMock2 = new Mock<IFileStream>();
            fileMock2.Setup(m => m.FileName)
                .Returns("test.edf");
            fileMock2.Setup(m => m.CopyToAsync(It.IsAny<Stream>(), default));

            var service = new FileService(fileSystemWrapper, baseDir, null);

            string workingDir = fileSystemWrapper.PathCombine(testingDir, contentDir);
            fileSystemWrapper.DirectoryCreateDirectory(workingDir);
            Assert.True(fileSystemWrapper.DirectoryExists(workingDir));

            // act
            var result1 = await service.CreateFile(fileMock1.Object, contentDir);
            var result2 = await service.CreateFile(fileMock2.Object, contentDir);

            string expected1 = fileSystemWrapper.PathCombine(testingDir, result1);
            string expected2 = fileSystemWrapper.PathCombine(testingDir, result2);

            // assert
            Assert.True(fileSystemWrapper.FileExists(expected1));
            Assert.True(fileSystemWrapper.FileExists(expected2));
        }

        [Fact]
        public async Task CreateFile_CreatesFiles_WithoutNamingConflict__IfFileWithNameAlreadyPresent()
        {
            // set
            string contentDir = "test";

            var fileMock1 = new Mock<IFileStream>();
            fileMock1.Setup(m => m.FileName)
                .Returns("test.edf");
            fileMock1.Setup(m => m.CopyToAsync(It.IsAny<Stream>(), default));

            var fileSystemMock = new Mock<FileSystemWrapper>(fileSystem);
            fileSystemMock.CallBase = true;
            fileSystemMock.SetupSequence(m => m.PathGetRandomFileName())
                .Returns("name1")
                .Returns("name2");

            fileSystemWrapper = fileSystemMock.Object;

            var service = new FileService(fileSystemWrapper, baseDir, null);

            string workingDir = fileSystemWrapper.PathCombine(testingDir, contentDir);
            fileSystemWrapper.DirectoryCreateDirectory(workingDir);
            Assert.True(fileSystemWrapper.DirectoryExists(workingDir));

            var previousPresentFile = fileSystemWrapper.PathCombine(workingDir, "name1.edf");
            fileSystemWrapper.FileWriteAllText(previousPresentFile, "test");

            // act
            var result1 = await service.CreateFile(fileMock1.Object, contentDir);

            string expected1 = fileSystemWrapper.PathCombine(testingDir, result1);

            // assert
            Assert.True(fileSystemWrapper.FileExists(expected1));
        }

        [Fact]
        public void DeleteFile_RemovesFile()
        {
            // set
            var file = "bliblablub.txt";
            var path = fileSystemWrapper.PathCombine(testingDir, file);

            fileSystemWrapper.DirectoryCreateDirectory(testingDir);
            fileSystemWrapper.FileWriteAllText(path, "");

            var service = new FileService(fileSystemWrapper, baseDir, null);

            // act
            service.DeleteFile(file);

            // assert
            Assert.False(fileSystemWrapper.FileExists(path));
        }

        [Fact]
        public void DeleteFile_DontRemovesFile_IfNotExist()
        {
            // set
            var file1 = "bliblablub1.txt";
            var path1 = fileSystemWrapper.PathCombine(testingDir, file1);
            var file2 = "bliblablub2.txt";
            var path2 = fileSystemWrapper.PathCombine(testingDir, file2);

            fileSystemWrapper.DirectoryCreateDirectory(testingDir);
            fileSystemWrapper.FileWriteAllText(path1, "");

            var service = new FileService(fileSystemWrapper, baseDir, null);

            // act
            service.DeleteFile(path2);

            // assert
            Assert.True(fileSystemWrapper.FileExists(path1));
            Assert.False(fileSystemWrapper.FileExists(path2));
        }

        [Fact]
        public void GetFullFilePath_ReturnsFullPath()
        {
            // set
            var path = "bliblablub.txt";
            var expected = fileSystemWrapper.PathCombine(testingDir, path);

            fileSystemWrapper.DirectoryCreateDirectory(testingDir);

            var service = new FileService(fileSystemWrapper, baseDir, null);

            // act
            var res = service.GetFullFilePath(path);

            // assert
            Assert.Equal(expected, res);
        }

        [Fact]
        public void ReadAllBytes_ReturnsCorrectNumberOfBytes()
        {
            // set
            var text = "Hello World!";
            var path = "bliblablub.txt";
            var expected = fileSystemWrapper.PathCombine(testingDir, path);

            fileSystemWrapper.DirectoryCreateDirectory(testingDir);
            fileSystemWrapper.FileWriteAllText(expected, text);

            var service = new FileService(fileSystemWrapper, baseDir, null);

            // act
            var res = service.ReadAllBytesFromFile(path);

            // assert
            Assert.NotNull(res);
            Assert.Equal(text.Length, res.Length);
        }

        [Fact]
        public void ReadAllBytes_ReturnsEmptyArray_IfFileNotExists()
        {
            // set
            var path = "bliblablub.txt";
            var expected = fileSystemWrapper.PathCombine(testingDir, path);

            fileSystemWrapper.DirectoryCreateDirectory(testingDir);

            var service = new FileService(fileSystemWrapper, baseDir, null);

            // act
            var res = service.ReadAllBytesFromFile(path);

            // assert
            Assert.NotNull(res);
            Assert.Equal(0, res.Length);
        }

        [Fact]
        public void ReadAllBytesFromZip_ReturnsCorrectNumberOfBytes()
        {
            // set
            var expected = 140;
            var text = "Hello World!";
            var filename = "bliblablub.txt";

            var data = String.Concat(Enumerable.Repeat("1", expected));

            var fileSystemMock = new Mock<FileSystemWrapper>(fileSystem);
            fileSystemMock.CallBase = true;
            fileSystemMock.Setup(m => m.ZipFileCreateFromDirectory(
                It.IsAny<string>(),
                It.IsAny<string>()));
            fileSystemWrapper = fileSystemMock.Object;

            var path = fileSystemWrapper.PathCombine(testingDir, "test");
            var filePath = fileSystemWrapper.PathCombine(path, filename);
            var temp = fileSystemWrapper.PathCombine(testingDir, "tempTest.txt");

            fileSystemWrapper.DirectoryCreateDirectory(path);
            fileSystemWrapper.FileWriteAllText(filePath, text);
            fileSystemWrapper.FileWriteAllText(temp, data);

            var service = new FileService(fileSystemWrapper, baseDir, null);

            // act
            var res = service.ReadAllBytesFromDir("test", temp);

            // assert
            Assert.NotNull(res);
            Assert.Equal(expected, res.Length);
        }

        [Fact]
        public void ReadAllBytesFromZip_ReturnsEmptyArray_IfDirectoryNotExists()
        {
            // set
            var expected = 0;

            var path = fileSystemWrapper.PathCombine(testingDir, "test");
            var temp = fileSystemWrapper.PathCombine(testingDir, "tempTest");

            fileSystemWrapper.DirectoryCreateDirectory(testingDir);

            var service = new FileService(fileSystemWrapper, baseDir, null);

            // act
            var res = service.ReadAllBytesFromDir(path, temp);

            // assert
            Assert.NotNull(res);
            Assert.Equal(expected, res.Length);
        }

        [Fact]
        async Task CreateWebResource_ReturnsFilename()
        {
            // set
            var filename = "test.png";
            var expected = "nifti";

            var file = new Mock<IFileStream>();
            file.Setup(m => m.FileName)
                .Returns(filename);

            var streamMock = new Mock<Stream>();

            var fileSystemMock = new Mock<FileSystemWrapper>(fileSystem);
            fileSystemMock.CallBase = true;
            fileSystemMock.Setup(m => m.FileExists(It.IsAny<string>()))
                .Returns(false);
            fileSystemMock.Setup(m => m.PathGetExtension(
                It.IsAny<string>()))
                .Returns(".png");
            fileSystemMock.Setup(m => m.FileCreate(
                It.IsAny<string>(),
                It.IsAny<FileMode>()))
                .Returns(streamMock.Object);

            fileSystemWrapper = fileSystemMock.Object;

            var service = new FileService(fileSystemWrapper, "", "www");

            // act
            var result = await service.CreateWebResource(file.Object);

            // assert
            Assert.NotNull(result);
            Assert.True(result.Contains(expected));
        }

        [Fact]
        async Task CreateWebResource_CreatesNewResource()
        {
            // set
            var filename = "test.png";

            var file = new Mock<IFileStream>();
            file.Setup(m => m.FileName)
                .Returns(filename);

            var streamMock = new Mock<Stream>();

            var fileSystemMock = new Mock<FileSystemWrapper>(fileSystem);
            fileSystemMock.CallBase = true;
            fileSystemMock.Setup(m => m.FileExists(It.IsAny<string>()))
                .Returns(false);
            fileSystemMock.Setup(m => m.PathGetExtension(
                It.IsAny<string>()))
                .Returns(".png");
            fileSystemMock.Setup(m => m.FileCreate(
                It.IsAny<string>(),
                It.IsAny<FileMode>()))
                .Returns(streamMock.Object);

            fileSystemWrapper = fileSystemMock.Object;

            var service = new FileService(fileSystemWrapper, "", "www");

            // act
            var result = await service.CreateWebResource(file.Object);

            // assert
            fileSystemMock.Verify(m => m.FileCreate(It.IsAny<string>(), It.IsAny<FileMode>()), Times.Once());
        }

        [Fact]
        void DeleteWebResource_DeletesResource()
        {
            // set
            var filename = "test.png";

            var fileSystemMock = new Mock<FileSystemWrapper>(fileSystem);
            fileSystemMock.CallBase = true;
            fileSystemMock.Setup(m => m.FileDelete(
                It.IsAny<string>()));

            fileSystemWrapper = fileSystemMock.Object;

            var service = new FileService(fileSystemWrapper, "", "www");

            // act
            service.DeleteWebResource(filename);

            // assert
            fileSystemMock.Verify(m => m.FileDelete(It.IsAny<string>()), Times.Once());
        }
    }
}
