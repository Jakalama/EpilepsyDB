using EpilepsieDB.Models;
using EpilepsieDB.Repositories;
using EpilepsieDB.Services.Impl;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace EpilepsieDB.Tests.Services
{
    public class SignalServiceTest : AbstractTest
    {
        public SignalServiceTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Constructor()
        {
            // set
            var repoMock = new Mock<IRepository<Signal>>();

            // act
            var service = new SignalService(repoMock.Object);
        }
    }
}
