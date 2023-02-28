using EpilepsieDB.Web.Common;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Web.Common
{
    public class FormFileProxyTest : AbstractTest
    {
        public FormFileProxyTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Name_ReturnsCorrect()
        {
            // set
            var name = "Test";
            var mock = new Mock<IFormFile>();
            mock.Setup(m => m.Name)
                .Returns(name);

            var proxy = new FormFileProxy(mock.Object);

            // act
            var result = proxy.Name;

            // assert
            Assert.Equal(name, result);
        }

        [Fact]
        public void FileName_ReturnsCorrect()
        {
            // set
            var fileName = "Test";
            var mock = new Mock<IFormFile>();
            mock.Setup(m => m.FileName)
                .Returns(fileName);

            var proxy = new FormFileProxy(mock.Object);

            // act
            var result = proxy.FileName;

            // assert
            Assert.Equal(fileName, result);
        }

        [Fact]
        public void ContentType_ReturnsCorrect()
        {
            // set
            var type = "Test";
            var mock = new Mock<IFormFile>();
            mock.Setup(m => m.ContentType)
                .Returns(type);

            var proxy = new FormFileProxy(mock.Object);

            // act
            var result = proxy.ContentType;

            // assert
            Assert.Equal(type, result);
        }

        [Fact]
        public void Length_ReturnsCorrect()
        {
            // set
            var length =2048;
            var mock = new Mock<IFormFile>();
            mock.Setup(m => m.Length)
                .Returns(length);

            var proxy = new FormFileProxy(mock.Object);

            // act
            var result = proxy.Length;

            // assert
            Assert.Equal(length, result);
        }

        [Fact]
        public async Task CopyToAsync_CallsCopyToAsync()
        {
            // set
            var mock = new Mock<IFormFile>();
            var proxy = new FormFileProxy(mock.Object);

            // act
            await proxy.CopyToAsync(null);

            // assert
            mock.Verify(m => m.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
